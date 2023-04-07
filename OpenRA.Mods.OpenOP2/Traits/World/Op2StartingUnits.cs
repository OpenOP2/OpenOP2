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
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Used by SpawnStartingUnits. Attach these to the world actor. You can have multiple variants by adding @suffixes.")]
	public class Op2StartingUnitsInfo : TraitInfo<Op2StartingUnits>
	{
		[Desc("Internal class ID.")]
		public readonly string Class = "none";

		[Desc("Exposed via the UI to the player.")]
		public readonly string ClassName = "Unlabeled";

		[Desc("Only available when selecting one of these factions.", "Leave empty for no restrictions.")]
		public readonly HashSet<string> Factions = new HashSet<string>();

		[Desc("The actor at the center, usually the mobile construction vehicle.")]
		[ActorReference]
		public readonly string BaseActor = null;

		[Desc("Offset from the spawn point, BaseActor will spawn at.")]
		public readonly CVec BaseActorOffset = CVec.Zero;

		[Desc("Additional buildings to add.")]
		[ActorReference]
		public readonly string[] AdditionalBuildings = Array.Empty<string>();

		[Desc("When checking for building/unit availability, increase radius by this much when checking a wider radius.")]
		public readonly int RadiusStep = 3;

		[Desc("Outer radius for spawning support actors")]
		public readonly WAngle AngleStep = new WAngle(16);

		[Desc("Max of this many radius increases.")]
		public readonly int MaxRadiusIncrease = 40;

		[FieldLoader.LoadUsing("LoadSupportActors")]
		public Dictionary<string, SupportActorsPlacementInfo> SupportActors;

		static object LoadSupportActors(MiniYaml yaml)
		{
			var retList = new Dictionary<string, SupportActorsPlacementInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "SupportActors");

			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("SupportActorsPlacement")))
			{
				var ret = new SupportActorsPlacementInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}
	}

	public class SupportActorsPlacementInfo
	{
		[Desc("Number of units to add.")]
		public int Count = 0;

		[Desc("The actor type to place.")]
		public string ActorName;

		[Desc("Number of columns.")]
		public int Width = 3;

		[Desc("Number of rows.")]
		public int Height = 3;
	}

	public class Op2StartingUnits { }
}
