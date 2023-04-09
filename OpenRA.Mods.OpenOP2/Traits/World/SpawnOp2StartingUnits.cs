#region Copyright & License Information
/*
 * Copyright 2007-2022 The OpenRA Developers (see AUTHORS)
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
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Spawn base actor at the spawnpoint and support units in an annulus around the base actor. Both are defined at MPStartUnits. Attach this to the world actor.")]
	public class SpawnOp2StartingUnitsInfo : TraitInfo, Requires<Op2StartingUnitsInfo>, NotBefore<LocomotorInfo>, ILobbyOptions
	{
		public readonly string StartingUnitsClass = "none";

		[Desc("Descriptive label for the starting units option in the lobby.")]
		public readonly string DropdownLabel = "Starting Units";

		[Desc("Tooltip description for the starting units option in the lobby.")]
		public readonly string DropdownDescription = "The units that players start the game with";

		[Desc("Prevent the starting units option from being changed in the lobby.")]
		public readonly bool DropdownLocked = false;

		[Desc("Whether to display the starting units option in the lobby.")]
		public readonly bool DropdownVisible = true;

		[Desc("Display order for the starting units option in the lobby.")]
		public readonly int DropdownDisplayOrder = 0;

		IEnumerable<LobbyOption> ILobbyOptions.LobbyOptions(MapPreview map)
		{
			var startingUnits = new Dictionary<string, string>();

			// Duplicate classes are defined for different race variants
			foreach (var t in map.WorldActorInfo.TraitInfos<Op2StartingUnitsInfo>())
				startingUnits[t.Class] = t.ClassName;

			if (startingUnits.Count > 0)
				yield return new LobbyOption("startingunits", DropdownLabel, DropdownDescription, DropdownVisible, DropdownDisplayOrder,
					startingUnits, StartingUnitsClass, DropdownLocked);
		}

		public override object Create(ActorInitializer init) { return new SpawnOp2StartingUnits(init.World, this); }
	}

	public class SpawnOp2StartingUnits : IWorldLoaded
	{
		readonly SpawnOp2StartingUnitsInfo info;
		readonly World world;

		public SpawnOp2StartingUnits(World world, SpawnOp2StartingUnitsInfo info)
		{
			this.world = world;
			this.info = info;
		}

		public void WorldLoaded(World world, WorldRenderer wr)
		{
			foreach (var p in world.Players)
				if (p.Playable)
					SpawnUnitsForPlayer(world, p);
		}

		class ActorPlacementInfo
		{
			public ActorInfo Actor;
			public List<CVec> Tiles;
		}

		bool PlaceActors(Op2StartingUnitsInfo unitsInfo, List<ActorPlacementInfo> actors, CPos startPos, Func<CPos, ActorPlacementInfo, bool> isAreaBuildable, Action<CPos, ActorInfo> createActor)
		{
			var radius = unitsInfo.RadiusStep;
			var angleStep = unitsInfo.AngleStep;
			var numAngles = 1024 / angleStep.Angle;
			var stepCount = 0;
			var done = false;
			while (stepCount < unitsInfo.MaxRadiusIncrease && done == false)
			{
				var angle = WAngle.Zero;
				for (var i = 0; i < numAngles; i++)
				{
					var actor = actors.FirstOrDefault();
					if (actor == null)
					{
						return true;
					}

					var x = (angle.Cos() * radius) / 1024;
					var y = (angle.Sin() * radius) / 1024;

					var vec = new CVec(x, y);
					var targetCell = startPos + vec;

					if (isAreaBuildable(targetCell, actor))
					{
						actors.Remove(actor);
						createActor(targetCell, actor.Actor);
					}

					angle += angleStep;
				}

				radius += unitsInfo.RadiusStep;
				stepCount++;
			}

			return false;
		}

		void SpawnUnitsForPlayer(World w, Player p)
		{
			var resourceLayer = w.WorldActor.TraitOrDefault<IResourceLayer>();

			var spawnClass = p.PlayerReference.StartingUnitsClass ?? w.LobbyInfo.GlobalSettings
				.OptionOrDefault("startingunits", info.StartingUnitsClass);

			var unitGroup = w.Map.Rules.Actors[SystemActors.World].TraitInfos<Op2StartingUnitsInfo>()
				.Where(g => g.Class == spawnClass && g.Factions != null && g.Factions.Contains(p.Faction.InternalName))
				.RandomOrDefault(w.SharedRandom);

			if (unitGroup == null)
				throw new InvalidOperationException($"No starting units defined for faction {p.Faction.InternalName} with class {spawnClass}");

			if (unitGroup.BaseActor != null)
			{
				w.CreateActor(unitGroup.BaseActor.ToLowerInvariant(), new TypeDictionary
				{
					new LocationInit(p.HomeLocation + unitGroup.BaseActorOffset),
					new OwnerInit(p),
					new SkipMakeAnimsInit(),
					new FacingInit(WAngle.Zero),
				});
			}

			if (unitGroup.AdditionalBuildings.Length == 0)
				return;

			var buildingList = unitGroup.AdditionalBuildings.Shuffle(w.SharedRandom)
				.Select(x => w.Map.Rules.Actors[x])
				.ToList();

			bool IsBuildingActorBuildable(CPos cell, ActorPlacementInfo actorInfo)
			{
				var actor = actorInfo.Actor;

				var bi = actor.TraitInfoOrDefault<BuildingInfo>();

				var buildingTiles = actorInfo.Tiles.Select(x => cell + x);

				return buildingTiles.All(t => w.Map.Contains(t) &&
					(bi.AllowPlacementOnResources || resourceLayer == null || resourceLayer.GetResource(t).Type == null) &&
						w.IsCellBuildable(t, actor, bi) &&
						!IsTileBulldozed(t));
			}

			bool IsTileClear(CPos pos)
			{
				var clearTerrains = new[] { "Clear", "ClearSand", "ClearRock" };
				return clearTerrains.Contains(w.Map.GetTerrainInfo(pos).Type);
			}

			bool IsTileBulldozed(CPos pos)
			{
				var tile = world.Map.Tiles[pos];
				var bulldozedTypes = new int[] { 2219, 2384, 2111 };
				if (bulldozedTypes.Contains(tile.Type))
				{
					return true;
				}

				return false;
			}

			var buildingPlacementInfo = buildingList
			.Select(buildingActor => {
				var bi = buildingActor.TraitInfoOrDefault<BuildingInfo>();
				var buildingTiles = new List<CVec>();
				for (var y = -1; y < bi.Dimensions.Y + 1; y++)
				{
					for (var x = -1; x < bi.Dimensions.X + 1; x++)
					{
						buildingTiles.Add(new CVec(x, y));
					}
				}

				return new ActorPlacementInfo
				{
					Actor = buildingActor,
					Tiles = buildingTiles
				};
			})
			.ToList();

			PlaceActors(unitGroup, buildingPlacementInfo, p.HomeLocation, IsBuildingActorBuildable, (pos, actorInfo) =>
			{
				world.CreateActor(actorInfo.Name.ToLowerInvariant(), new TypeDictionary
				{
					new LocationInit(pos),
					new OwnerInit(p),
					new SkipMakeAnimsInit(),
					new FacingInit(WAngle.Zero),
				});
			});

			// Support actors
			var actors = unitGroup.SupportActors.Values.Shuffle(w.SharedRandom)
				.Select(supportActor =>
				{
					var groupSize = new int2(supportActor.Width + 2, supportActor.Height + 2);
					var tileList = new List<CVec>();

					for (var y = -1; y < groupSize.Y; y++)
					{
						for (var x = -1; x < groupSize.X; x++)
						{
							tileList.Add(new CVec(x, y));
						}
					}

					return new ActorPlacementInfo
					{
						Actor = w.Map.Rules.Actors[supportActor.ActorName],
						Tiles = tileList
					};
				})
				.ToList();

			bool IsSupportActorBuildable(CPos cell, ActorPlacementInfo actorInfo)
			{
				var actor = actorInfo.Actor;

				var buildingTiles = actorInfo.Tiles.Select(x => cell + x);

				return buildingTiles.All(t => w.Map.Contains(t) &&
					(resourceLayer == null || resourceLayer.GetResource(t).Type == null) &&
						!world.ActorMap.GetActorsAt(t).Any() &&
						IsTileClear(t) &&
						!IsTileBulldozed(t));
			}

			PlaceActors(unitGroup, actors, p.HomeLocation, IsSupportActorBuildable, (pos, actorInfo) =>
			{
				var placementInfo = unitGroup.SupportActors.Values.First(x => x.ActorName == actorInfo.Name);

				var positions = new List<CPos>();
				for (var y = 0; y < placementInfo.Height; y++)
				{
					for (var x = 0; x < placementInfo.Width; x++)
					{
						positions.Add(pos + new CVec(x, y));
					}
				}

				var placementAngle = new WAngle(w.SharedRandom.Next(1024));
				for (var i = 0; i < placementInfo.Count; i++)
				{
					var placementPos = positions[i];
					world.CreateActor(actorInfo.Name.ToLowerInvariant(), new TypeDictionary
					{
						new LocationInit(placementPos),
						new OwnerInit(p),
						new SkipMakeAnimsInit(),
						new FacingInit(placementAngle),
					});
				}
			});
		}
	}
}
