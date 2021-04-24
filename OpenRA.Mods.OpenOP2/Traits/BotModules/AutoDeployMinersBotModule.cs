#region Copyright & License Information
/*
 * Copyright 2007-2020 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common.Pathfinder;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Manages AI robo-miners.")]
	public class AutoDeployMinersBotModuleInfo : ConditionalTraitInfo
	{
		[Desc("Actor types that are considered miners (deploy into mines).")]
		public readonly HashSet<string> MinerTypes = new HashSet<string>();

		[Desc("The type of resource to look for.")]
		public string TargetResourceType = "ore";

		[Desc("The max cell radius to search for an available resource.")]
		public int MaxRange = 65;
		public override object Create(ActorInitializer init) { return new AutoDeployMinersBotModule(init.Self, this); }
	}

	public class AutoDeployMinersBotModule : ConditionalTrait<AutoDeployMinersBotModuleInfo>, IBotTick, INotifyKilled
	{
		private ResourceLayer resourceLayer;
		readonly ResourceClaimLayer claimLayer;
		readonly DomainIndex domainIndex;
		readonly IPathFinder pathFinder;
		private readonly int tickEvery = 50;

		readonly World world;
		readonly Player player;

		public AutoDeployMinersBotModule(Actor self, AutoDeployMinersBotModuleInfo info)
			: base(info)
		{
			world = self.World;
			player = self.Owner;

			resourceLayer = self.World.WorldActor.Trait<ResourceLayer>();
			claimLayer = self.World.WorldActor.Trait<ResourceClaimLayer>();
			pathFinder = self.World.WorldActor.Trait<IPathFinder>();
			domainIndex = self.World.WorldActor.Trait<DomainIndex>();
			var resourceTypes = self.World.WorldActor.TraitsImplementing<ResourceType>().ToArray();
			var resourceType = resourceTypes.FirstOrDefault(a =>
				string.Equals(a.Info.Type, info.TargetResourceType, StringComparison.InvariantCultureIgnoreCase));

			if (resourceType == null)
				throw new ArgumentException($"Couldn't find resource type: {info.TargetResourceType}");
		}

		void IBotTick.BotTick(IBot bot)
		{
			if (world.WorldTick % tickEvery != 0)
				return;

			var miners = world.ActorsHavingTrait<Transforms>()
				.Where(a => a.Owner == player && a.IsIdle && Info.MinerTypes.Contains(a.Info.Name));

			foreach (var miner in miners)
			{
				var closestMine = ClosestHarvestablePos(miner);
				if (!closestMine.HasValue)
					continue;

				var claimedOk = claimLayer.TryClaimCell(miner, closestMine.Value);
				if (!claimedOk)
					continue;

				bot.QueueOrder(new Order("Move", miner, Target.FromCell(world, closestMine.Value), true));
				bot.QueueOrder(new Order("DeployTransform", miner, true));
			}
		}

		/// <summary>
		/// Finds the closest harvestable pos between the current position of the harvester
		/// and the last order location
		/// </summary>
		CPos? ClosestHarvestablePos(Actor self)
		{
			var mobile = self.Trait<Mobile>();

			// Determine where to search from and how far to search:
			Func<CPos, bool> canHarvest = pos =>
			{
				var resType = resourceLayer.GetResourceType(pos);
				if (resType != null && string.Compare(resType.Info.Name, Info.TargetResourceType, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}

				return false;
			};

			// Find any harvestable resources:
			List<CPos> path;
			using (var search = PathSearch.Search(self.World, mobile.Locomotor, self, BlockedByActor.Stationary, loc =>
					domainIndex.IsPassable(self.Location, loc, mobile.Locomotor) && canHarvest(loc) && claimLayer.CanClaimCell(self, loc))
				.FromPoint(self.Location))
				path = pathFinder.FindPath(search);

			if (path.Count > 0)
				return path[0];

			return null;
		}

		public void Killed(Actor self, AttackInfo e)
		{
			claimLayer.RemoveClaim(self);
		}
	}
}
