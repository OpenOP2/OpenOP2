#region Copyright & License Information
/*
 * Copyright 2007-2021 The OpenRA Developers (see AUTHORS)
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
	[Desc("Removes blight from its radius and prevents it from spreading.")]
	public class RepelsBlightInfo : ConditionalTraitInfo
	{
		[Desc("How far the light repulsion extends from the actor.")]
		public WDist Radius = new WDist(2048);

		[Desc("How often to update the light radius cells.")]
		public int UpdateEvery = 1;

		[Desc("How often to recalculate samples of cells to remove blight.")]
		public int UpdateSamplesEvery = 1;

		[Desc("The maximum number of sampled cells to de-blight each turn.")]
		public int MaxRepulsionSamples = 6;

		[Desc("Random weighting added to cell sampling metric.")]
		public int RepulsionRandom = 5;

		public override object Create(ActorInitializer init) { return new RepelsBlight(init.Self, this); }
	}

	public class RepelsBlight : ConditionalTrait<RepelsBlightInfo>, ITick, INotifyRemovedFromWorld
	{
		CPos lastPosition;
		readonly World world;
		Actor actor;
		readonly RepelsBlightInfo info;
		HashSet<CPos> repellingCells = new HashSet<CPos>();
		public List<CPos> RandomSamples { get; private set; } = new List<CPos>();
		public RepelsBlight(Actor self, RepelsBlightInfo info)
			: base(info)
		{
			actor = self;
			world = self.World;
			this.info = info;
		}

		protected override void Created(Actor self)
		{
			base.Created(self);
			lastPosition = self.Location;
		}

		void INotifyRemovedFromWorld.RemovedFromWorld(Actor self)
		{
			repellingCells.Clear();
			RandomSamples.Clear();
		}

		void UpdateCellsIfMoved()
		{
			if (lastPosition != actor.Location)
			{
				lastPosition = actor.Location;
				UpdateCells();
				UpdateRandomSamples();
			}
		}

		void UpdateCells()
		{
			repellingCells.Clear();
			repellingCells = world.Map.FindTilesInCircle(actor.Location, (info.Radius.Length + 1023) / 1024).ToHashSet();
		}

		void ITick.Tick(Actor self)
		{
			actor = self;

			if (IsTraitDisabled)
			{
				return;
			}

			if (world.WorldTick % info.UpdateEvery == 0)
			{
				UpdateCellsIfMoved();
			}

			if (world.WorldTick % info.UpdateSamplesEvery == 0)
			{
				UpdateRandomSamples();
			}
		}

		protected override void TraitEnabled(Actor self)
		{
			base.TraitEnabled(self);

			lastPosition = actor.Location;
			UpdateCells();
			UpdateRandomSamples();
		}

		protected override void TraitDisabled(Actor self)
		{
			base.TraitDisabled(self);
			RandomSamples.Clear();
			repellingCells.Clear();
		}

		void UpdateRandomSamples()
		{
			var radiusLength = info.Radius.Length;

			// Sample a set of repelling cells to remove from the blight
			var metrics = repellingCells.Select(x =>
			{
				var distanceFromCenter = x - actor.Location;
				var distancePercent = ((radiusLength - distanceFromCenter.Length) * 100) / radiusLength;
				var random = (info.RepulsionRandom / 2) - world.SharedRandom.Next(0, info.RepulsionRandom);
				return new
				{
					Position = x,
					Distance = distancePercent,
					Random = random,
					Actor = actor,
				};
			});

			RandomSamples = metrics
				.OrderByDescending(x => x.Distance + x.Random)
				.Select(x => x.Position)
				.Take(info.MaxRepulsionSamples)
				.ToList();
		}

		public bool IsTileRepulsed(CPos pos)
		{
			return repellingCells.Contains(pos);
		}
	}
}
