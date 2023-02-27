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

using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Creates the dreaded blight. Put this on your World actor.")]
	public class BlightOverlayInfo : ConditionalTraitInfo, ILobbyCustomRulesIgnore
	{
		[Desc("Name of the blight image.")]
		public readonly string ImageName = "shroud";

		[Desc("Name of the blight sequence.")]
		public readonly string Sequence = "shroud";

		[Desc("Whether the blight palette is a player palette or not.")]
		public readonly bool IsPlayerPalette = false;

		[PaletteReference(nameof(IsPlayerPalette))]
		[Desc("Palette used to render the blight.")]
		public readonly string Palette = "blight";

		[Desc("Attempt to propagate every this many ticks.")]
		public readonly int TickEvery = 15;

		[Desc("Fraction of frontier tiles to consider spreading from at each tick (1-100).")]
		public readonly int TilesToSpreadFraction = 10;

		[Desc("Maximum number of tiles we can spread from in one tick.")]
		public readonly int TilesToSpreadMax = 30;

		[Desc("Chance of spreading from a tile to another.")]
		public readonly int SpreadChance = 10;

		[Desc("Random value center for fuzzing weights of candidate tiles to spread to.")]
		public readonly int FuzzPivot = 6;

		[Desc("Random value max for fuzzing weights of candidate tiles to spread to.")]
		public readonly int FuzzMax = 12;

		[Desc("Frame order override.")]
		public readonly int[] Frames = Array.Empty<int>();

		[Desc("Try and damage any actors caught in the blight every this many ticks.")]
		public readonly int DamageEvery = 2;

		[Desc("Damage inflicted every damage tick.")]
		public readonly int Damage = 10;

		[Desc("Minimum number of actors to try and damage every damage tick.")]
		public readonly int ActorsToDamageMin = 4;

		[Desc("Maximum number of actors to try and damage every damage tick.")]
		public readonly int ActorsToDamageMax = 8;

		[Desc("Small random delay between each damage tick delivered so we don't have a rhythmic damage pattern.")]
		public readonly int RandomDamageDelay = 6;

		[Desc("Every this many ticks, search within random blighted cells for any actors that might have appeared.")]
		public readonly int RandomSearchEvery = 3;

		[Desc("Only search for this many positions in the blight every random search tick.")]
		public readonly int RandomSearchMax = 8;

		[Desc("Apply the damage using these DamageTypes.")]
		public readonly BitSet<DamageType> DamageTypes = default(BitSet<DamageType>);

		[Desc("Play this notification from the Speech section when the blight is triggered.")]
		public readonly string StartVoice;

		[Desc("Wait this many ticks before activating.")]
		public readonly int StartDelay = 0;

		[Desc("Add this many random ticks before activation.")]
		public readonly int StartDelayRandom = 0;

		[Desc("If we couldn't expand to a tile, put it on the blacklist for this many cycles so we don't check it again.")]
		public readonly int SpreadBlacklistTimeout = 10;

		[Desc("Every this many ticks, apply repulsion from RepelsBlight radii.")]
		public readonly int RepelEvery = 15;

		[LocomotorReference]
		[FieldLoader.Require]
		[Desc("Locomotor used by the blight. Must be defined on the World actor.")]
		public readonly string Locomotor = null;

		public override object Create(ActorInitializer init) { return new BlightOverlay(init.World, this); }
	}

	public class BlightOverlay : ConditionalTrait<BlightOverlayInfo>, IWorldLoaded, INotifyActorDisposing, IRenderAboveWorld, ITick
	{
		public class FrontierCell
		{
			public int NumBlightedNeighbors = 1;
			public int TerrainTypeWeighting = 0;
		}

		readonly BlightOverlayInfo info;
		readonly World world;
		readonly Map map;
		TerrainSpriteLayer blightLayer;
		WorldRenderer worldRenderer;
		Sprite[] blightSprites;
		readonly HashSet<CPos> blightedCells = new HashSet<CPos>();
		readonly Dictionary<CPos, FrontierCell> frontier = new Dictionary<CPos, FrontierCell>();
		readonly Dictionary<CPos, int> terrainCostMap = new Dictionary<CPos, int>();
		Locomotor locomotor;
		readonly List<Actor> blightedActors = new List<Actor>();
		List<RepelsBlight> repelsBlights = new List<RepelsBlight>();
		readonly Dictionary<CPos, int> spreadBlacklist = new Dictionary<CPos, int>();
		List<TraitPair<SpawnsBlight>> blightSpawnActors = new List<TraitPair<SpawnsBlight>>();
		int startDelayCount;
		bool started;

		public BlightOverlay(World world, BlightOverlayInfo info)
			: base(info)
		{
			this.info = info;
			this.world = world;
			map = world.Map;
		}

		void IWorldLoaded.WorldLoaded(World world, WorldRenderer wr)
		{
			startDelayCount = info.StartDelay + world.SharedRandom.Next(0, info.StartDelayRandom);

			locomotor = world.WorldActor.TraitsImplementing<Locomotor>()
				.Single(l => l.Info.Name == info.Locomotor);

			worldRenderer = wr;

			var sequenceProvider = map.Rules.Sequences;
			var frames = info.Frames;

			blightSprites = frames.Select(frame =>
				sequenceProvider.GetSequence(info.ImageName, info.Sequence).GetSprite(frame))
				.ToArray();

			var shroudSheet = blightSprites[0].Sheet;
			var emptySprite = new Sprite(shroudSheet, Rectangle.Empty, TextureChannel.Alpha);
			blightLayer = new TerrainSpriteLayer(world, wr, emptySprite, BlendMode.Alpha, false);
			foreach (var cell in map.AllCells)
				UpdateCell(cell);

			blightSpawnActors = world.ActorsWithTrait<SpawnsBlight>().Select(x => x).ToList();
			repelsBlights = world.ActorsWithTrait<RepelsBlight>().Select(x => x.Trait).ToList();
		}

		void INotifyActorDisposing.Disposing(Actor self)
		{
		}

		void UpdateCell(CPos pos)
		{
			var locomotorWeighting = locomotor.MovementSpeedForCell(pos);
			terrainCostMap.Add(pos, locomotorWeighting);
		}

		public void RenderAboveWorld(Actor self, WorldRenderer wr)
		{
			blightLayer.Draw(wr.Viewport);
		}

		void ITick.Tick(Actor self)
		{
			if (IsTraitDisabled)
			{
				return;
			}

			if (self.World.WorldTick % info.TickEvery == 0)
			{
				// Randomize start
				if (!started)
				{
					if (startDelayCount <= 0)
					{
						started = true;
					}
					else
					{
						startDelayCount--;
						return;
					}
				}

				// Spawn from each actor when ready
				if (blightSpawnActors.Count > 0)
				{
					if (!string.IsNullOrWhiteSpace(info.StartVoice))
					{
						Game.Sound.PlayNotification(world.Map.Rules, world.LocalPlayer, "Speech", info.StartVoice, null);
					}

					var removedSpawnActors = new List<TraitPair<SpawnsBlight>>();
					var readySpawnActors = blightSpawnActors.Where(x => x.Trait.IsReady);
					foreach (var actor in readySpawnActors)
					{
						removedSpawnActors.Add(actor);
						BlightTile(actor.Actor.Location);
					}

					if (removedSpawnActors.Count > 0)
					{
						foreach (var removed in removedSpawnActors)
						{
							blightSpawnActors.Remove(removed);
						}
					}

					return;
				}

				// Get a fraction of the frontier cells; at least one
				var numCells = Math.Min(info.TilesToSpreadMax, Math.Max(1, (int)Math.Ceiling(info.TilesToSpreadFraction / 100f * frontier.Count)));
				var randomCells = frontier
					.Where(x => !spreadBlacklist.ContainsKey(x.Key))
					.OrderByDescending(r =>
						(r.Value.TerrainTypeWeighting / 10) +
						r.Value.NumBlightedNeighbors + (info.FuzzPivot - world.SharedRandom.Next(0, info.FuzzMax)))
					.Take(numCells)
					.Select(x => x.Key)
					.ToArray();

				// Spread to them
				foreach (var cell in randomCells)
				{
					var spreadChance = world.SharedRandom.Next(0, 100);
					if (spreadChance < info.SpreadChance)
					{
						if (IsTileRepulsed(cell))
						{
							// If we're repulsed by a repulser or wall, add this to the blacklist
							// so we don't try to spread here again for a while.
							spreadBlacklist.Add(cell, world.WorldTick);
						}
						else
						{
							BlightTile(cell);
							frontier.Remove(cell);
						}
					}
				}

				// Check our blacklist for out of date entries and remove them
				var blacklistTimeout = world.WorldTick - info.SpreadBlacklistTimeout;
				var outOfDateEntries = spreadBlacklist.Where(x => x.Value <= blacklistTimeout).ToArray();
				foreach (var outOfDateEntry in outOfDateEntries)
				{
					spreadBlacklist.Remove(outOfDateEntry.Key);
				}
			}

			if (self.World.WorldTick % info.DamageEvery == 0)
			{
				TickDamageOnExistingActors();
			}

			if (self.World.WorldTick % info.RepelEvery == 0)
			{
				RepelBlight();
			}

			if (self.World.WorldTick % info.RandomSearchEvery == 0)
			{
				SearchForNewActorsInBlight();
			}
		}

		void RepelBlight()
		{
			repelsBlights = world.ActorsWithTrait<RepelsBlight>().Select(x => x.Trait).ToList();
			foreach (var repelsBlight in repelsBlights)
			{
				var sampleCells = repelsBlight.RandomSamples;
				foreach (var sampleCell in sampleCells)
				{
					if (blightedCells.Contains(sampleCell))
					{
						UnblightTile(sampleCell);
					}
				}
			}
		}

		bool IsTileRepulsed(CPos pos)
		{
			return repelsBlights.Any(x => x.IsTileRepulsed(pos));
		}

		void BlightTile(CPos pos)
		{
			if (blightedCells.Contains(pos))
			{
				throw new Exception($"Blighted cells already contains {pos}");
			}

			blightedCells.Add(pos);
			BlightAnyActorsAtPosition(pos);

			// Get all neighbors
			var neighbors = new HashSet<CPos>();
			for (var y = -1; y < 2; y++)
			{
				for (var x = -1; x < 2; x++)
				{
					var neighborPos = pos + new CVec(x, y);

					if (!world.Map.Contains(neighborPos))
					{
						continue;
					}

					blightLayer.Clear(neighborPos);
					if (GetSpriteForTile(neighborPos, out var tileIndex))
					{
						AddBlightSpriteToLayer(neighborPos, tileIndex);
					}

					if (x == 0 && y == 0)
					{
						continue;
					}

					var blightedCellsContains = blightedCells.Contains(neighborPos);

					// Increment neighbor count of cell if we add a blight cell near it
					var frontierContains = frontier.ContainsKey(neighborPos);
					if (!blightedCellsContains && frontierContains)
					{
						var frontierCell = frontier[neighborPos];
						frontierCell.NumBlightedNeighbors++;
					}

					if (!blightedCellsContains && !frontierContains)
					{
						neighbors.Add(neighborPos);
					}
				}
			}

			foreach (var validNeighbor in neighbors)
			{
				frontier.Add(validNeighbor, new FrontierCell()
				{
					NumBlightedNeighbors = 1,
					TerrainTypeWeighting = terrainCostMap[validNeighbor]
				});
			}
		}

		void UnblightTile(CPos pos)
		{
			blightedCells.Remove(pos);

			// BlightAnyActorsAtPosition(pos);
			// Get all neighbors
			var neighbors = new HashSet<CPos>();
			for (var y = -1; y < 2; y++)
			{
				for (var x = -1; x < 2; x++)
				{
					var neighborPos = pos + new CVec(x, y);

					if (!world.Map.Contains(neighborPos))
					{
						continue;
					}

					blightLayer.Clear(neighborPos);
					if (GetSpriteForTile(neighborPos, out var tileIndex))
					{
						AddBlightSpriteToLayer(neighborPos, tileIndex);
					}

					if (x == 0 && y == 0)
					{
						continue;
					}

					var blightedCellsContains = blightedCells.Contains(neighborPos);

					// Decrement neighbor count of cell if we remove a blight cell near it
					var frontierContains = frontier.ContainsKey(neighborPos);
					if (!blightedCellsContains && frontierContains)
					{
						var frontierCell = frontier[neighborPos];
						frontierCell.NumBlightedNeighbors--;

						// If it's empty, remove it from the frontier
						if (frontierCell.NumBlightedNeighbors == 0)
						{
							frontier.Remove(neighborPos);
							frontierContains = false;
						}
					}

					// if (!blightedCellsContains && !frontierContains)
					// {
					// 	neighbors.Add(neighborPos);
					// }
				}
			}

			// foreach (var validNeighbor in neighbors)
			// {
			// 	frontier.Add(validNeighbor, new FrontierCell()
			// 	{
			// 		NumBlightedNeighbors = 1,
			// 		TerrainTypeWeighting = terrainCostMap[validNeighbor]
			// 	});
			// }
		}

		void BlightAnyActorsAtPosition(CPos pos)
		{
			if (world.ActorMap.AnyActorsAt(pos))
			{
				var newBlightedActors = SearchForActors(pos);
				foreach (var actor in newBlightedActors)
				{
					blightedActors.Add(actor);
				}
			}
		}

		void AddBlightSpriteToLayer(CPos pos, int tileIndex)
		{
			var paletteReference = worldRenderer.Palette(info.Palette);
			var sequenceProvider = map.Rules.Sequences;
			blightLayer.Update(pos, sequenceProvider.GetSequence(info.ImageName, info.Sequence), paletteReference, tileIndex);
		}

		bool GetSpriteForTile(CPos pos, out int result)
		{
			var isBlighted = blightedCells.Contains(pos);
			var neighbors = new HashSet<CPos>();
			for (var y = -1; y < 1; y++)
			{
				for (var x = -1; x < 1; x++)
				{
					if (x == 0 && y == 0)
					{
						continue;
					}

					var neighborPos = pos + new CVec(x, y);
					if (!world.Map.Contains(neighborPos))
					{
						if (isBlighted)
						{
							// We're touching the edge of the map, return full sprite
							result = 0;
							return true;
						}

						continue;
					}

					var blightedCellsContains = blightedCells.Contains(neighborPos);
					if (blightedCellsContains)
					{
						neighbors.Add(neighborPos);
					}
				}
			}

			var topLeft = neighbors.Any(neighbor => (neighbor - pos) == new CVec(-1, -1));
			var top = neighbors.Any(neighbor => (neighbor - pos) == new CVec(0, -1));
			var left = neighbors.Any(neighbor => (neighbor - pos) == new CVec(-1, 0));

			// 0: Empty
			// 1: TR Inner
			// 2: TL Inner
			// 3: T
			// 4: BR Inner
			// 5: R
			// 6: Connector TR-BL
			// 7: TR
			// 8: BL Inner
			// 9: Connector TL-BR
			// 10: L
			// 11: TL
			// 12: B
			// 13: BR
			// 14: BL
			if (isBlighted)
			{
				// Top left outer
				if (!top && !left && !topLeft)
				{
					result = 11;
					return true;
				}

				// Bottom left inner
				else if (top && !left && topLeft)
				{
					result = 8;
					return true;
				}

				// Top left inner
				else if (!top && left && topLeft)
				{
					result = 1;
					return true;
				}

				// Top right inner
				else if (top && left && !topLeft)
				{
					result = 2;
					return true;
				}

				// BR-TL Connector
				else if (!top && !left)
				{
					result = 9;
					return true;
				}

				// North
				else if (!top)
				{
					result = 3;
					return true;
				}

				// West
				else if (!left)
				{
					result = 10;
					return true;
				}
			}
			else
			{
				// Bottom left outer
				if (top && !left && !topLeft)
				{
					result = 14;
					return true;
				}

				// Top right outer
				else if (!top && left && !topLeft)
				{
					result = 7;
					return true;
				}

				// Bottom right outer
				else if (!top && !left && topLeft)
				{
					result = 13;
					return true;
				}

				// Bottom right inner
				else if (top && left && topLeft)
				{
					result = 4;
					return true;
				}

				// BL-TR Connector
				else if (top && left)
				{
					result = 6;
					return true;
				}

				// South
				else if (top)
				{
					result = 12;
					return true;
				}

				// East
				else if (left)
				{
					result = 5;
					return true;
				}
			}

			if (isBlighted)
			{
				result = 0; // Full sprite
				return true;
			}

			result = -1;
			return false;
		}

		IEnumerable<Actor> SearchForActors(CPos pos)
		{
			var anyActors = world.ActorMap.AnyActorsAt(pos);
			if (!anyActors)
			{
				return Array.Empty<Actor>();
			}

			var actors = new List<Actor>();
			foreach (var actor in world.ActorMap.GetActorsAt(pos))
			{
				if (actor.IsInWorld && !actor.IsDead && !blightedActors.Contains(actor))
				{
					actors.Add(actor);
				}
			}

			return actors;
		}

		class BlightVictimActor
		{
			public Actor Actor;
			public int DelayCount;
			public bool IsCompleted => DelayCount == 0;
		}

		List<BlightVictimActor> blightVictims = new List<BlightVictimActor>();
		readonly Queue<Actor> randomlySelectedVictims = new Queue<Actor>();

		void TickDamageOnExistingActors()
		{
			if (blightedActors.Count == 0)
			{
				return;
			}

			// Go through our list of actors to be damaged after their time is up and damage them.
			// Keep track of victims and remove them.
			var damagedVictims = new List<BlightVictimActor>();
			foreach (var victim in blightVictims)
			{
				if (victim.IsCompleted)
				{
					if (!victim.Actor.IsInWorld || victim.Actor.IsDead)
					{
						// Bring out your dead
						blightedActors.Remove(victim.Actor);
					}
					else
					{
						var victimPos = victim.Actor.Location;
						var isActorInBlight = blightedCells.Contains(victimPos);
						if (isActorInBlight)
						{
							victim.Actor.InflictDamage(world.WorldActor, new Damage(info.Damage, info.DamageTypes));
						}
						else
						{
							blightedActors.Remove(victim.Actor);
						}
					}

					damagedVictims.Add(victim);
				}
				else
				{
					victim.DelayCount--;
				}
			}

			if (damagedVictims.Count > 0)
			{
				// Remove the expired
				blightVictims = blightVictims.Where(x => !damagedVictims.Contains(x)).ToList();
			}

			// Each damage tick, add (min to max) actors to be damaged next tick
			var numExpiredVictims = damagedVictims.Count;
			var numNewVictims = numExpiredVictims +
			                    world.SharedRandom.Next(info.ActorsToDamageMin, info.ActorsToDamageMax);
			if (numNewVictims > 0)
			{
				if (randomlySelectedVictims.Count < numNewVictims)
				{
					var randomActors = blightedActors.OrderBy(x => world.SharedRandom.Next());
					foreach (var newActor in randomActors)
					{
						randomlySelectedVictims.Enqueue(newActor);
					}
				}

				var newVictims = randomlySelectedVictims.Take(numNewVictims);
				blightVictims.AddRange(newVictims.Select((x) => new BlightVictimActor()
				{
					Actor = x,
					DelayCount = world.SharedRandom.Next(0, info.RandomDamageDelay),
				}));
			}
		}

		void SearchForNewActorsInBlight()
		{
			var tiles = blightedCells.OrderBy(x => world.SharedRandom.Next()).Take(info.RandomSearchMax);
			foreach (var tile in tiles)
			{
				BlightAnyActorsAtPosition(tile);
			}
		}
	}
}
