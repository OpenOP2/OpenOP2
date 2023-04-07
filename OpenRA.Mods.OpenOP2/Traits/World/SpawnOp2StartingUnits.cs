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

		public override object Create(ActorInitializer init) { return new SpawnOp2StartingUnits(this); }
	}

	public class SpawnOp2StartingUnits : IWorldLoaded
	{
		readonly SpawnOp2StartingUnitsInfo info;

		public SpawnOp2StartingUnits(SpawnOp2StartingUnitsInfo info)
		{
			this.info = info;
		}

		public void WorldLoaded(World world, WorldRenderer wr)
		{
			foreach (var p in world.Players)
				if (p.Playable)
					SpawnUnitsForPlayer(world, p);
		}

		void SpawnUnitsForPlayer(World w, Player p)
		{
			var spawnClass = p.PlayerReference.StartingUnitsClass ?? w.LobbyInfo.GlobalSettings
				.OptionOrDefault("startingunits", info.StartingUnitsClass);

			var unitGroup = w.Map.Rules.Actors[SystemActors.World].TraitInfos<Op2StartingUnitsInfo>()
				.Where(g => g.Class == spawnClass && g.Factions != null && g.Factions.Contains(p.Faction.InternalName))
				.RandomOrDefault(w.SharedRandom);

			if (unitGroup == null)
				throw new InvalidOperationException($"No starting units defined for faction {p.Faction.InternalName} with class {spawnClass}");

			if (unitGroup.BaseActor != null)
			{
				var facing = unitGroup.BaseActorFacing.HasValue ? unitGroup.BaseActorFacing.Value : new WAngle(w.SharedRandom.Next(1024));
				w.CreateActor(unitGroup.BaseActor.ToLowerInvariant(), new TypeDictionary
				{
					new LocationInit(p.HomeLocation + unitGroup.BaseActorOffset),
					new OwnerInit(p),
					new SkipMakeAnimsInit(),
					new FacingInit(facing),
				});
			}

			if (unitGroup.SupportActors.Length == 0)
				return;

			// Create a grid for how many support actors we have
			var gridSize = new int2(11, 11);
			var numBuildings = unitGroup.SupportActors.Length;
			if (numBuildings > 8)
			{
				gridSize = new int2(4, 4);
			}

			var cellSize = new int2(6, 5);
			var padding = new int2(0, 0);

			var centerGridPos = new int2((int)Math.Floor((decimal)(gridSize.X / 2)), (int)Math.Floor((decimal)(gridSize.Y / 2)));

			var halfCellSize = new int2((int)Math.Floor((decimal)(cellSize.X / 2)), (int)Math.Floor((decimal)(cellSize.Y / 2)));

			var startPos = p.HomeLocation - new CVec(centerGridPos.X * (cellSize.X + padding.X), centerGridPos.Y * (cellSize.Y + padding.Y));

			var shuffledActors = unitGroup.SupportActors.Shuffle(w.SharedRandom).ToArray();

			var index = 0;
			for (var y = 0; y < gridSize.Y; y++)
			{
				for (var x = 0; x < gridSize.X; x++)
				{
					// Skip "center" cell containing our command center
					if (y == centerGridPos.X && x == centerGridPos.Y) continue;

					if (shuffledActors.Length <= index)
						continue;

					var cell = startPos + new CVec(x * (cellSize.X + padding.X), y * (cellSize.Y + padding.Y));

					var s = shuffledActors[index];
					w.CreateActor(s.ToLowerInvariant(), new TypeDictionary
					{
						new LocationInit(cell),
						new OwnerInit(p),
						new SkipMakeAnimsInit(),
						new FacingInit(WAngle.Zero),
					});

					index++;
				}
			}
		}
	}
}
