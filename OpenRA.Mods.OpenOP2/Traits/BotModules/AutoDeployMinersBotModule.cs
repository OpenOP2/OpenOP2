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

using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Manages AI robo-miners.")]
	public class AutoDeployMinersBotModuleInfo : ConditionalTraitInfo
	{
		[Desc("Actor types that are considered miners (deploy into mines).")]
		public readonly HashSet<string> MinerTypes = new();

		[Desc("The type of resource to look for.")]
		public string TargetResourceType = "ore";

		[Desc("The max cell radius to search for an available resource.")]
		public int MaxRange = 65;
		public override object Create(ActorInitializer init) { return new AutoDeployMinersBotModule(init.Self, this); }
	}

	public class AutoDeployMinersBotModule : ConditionalTrait<AutoDeployMinersBotModuleInfo>, IBotTick
	{
		readonly ResourceClaimLayer claimLayer;
		readonly int tickEvery = 50;

		readonly World world;
		readonly Player player;

		public AutoDeployMinersBotModule(Actor self, AutoDeployMinersBotModuleInfo info)
			: base(info)
		{
			world = self.World;
			player = self.Owner;

			claimLayer = self.World.WorldActor.Trait<ResourceClaimLayer>();
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
		/// and the last order location.
		/// </summary>
		CPos? ClosestHarvestablePos(Actor self)
		{
			var mobile = self.Trait<Mobile>();

			// Find any harvestable resources:
			var path = mobile.PathFinder.FindPathToTargetCellByPredicate(
				self,
				new[] { self.Location },
				loc =>
					claimLayer.CanClaimCell(self, loc),
				BlockedByActor.Stationary);

			if (path.Count > 0)
				return path[0];

			return null;
		}
	}
}
