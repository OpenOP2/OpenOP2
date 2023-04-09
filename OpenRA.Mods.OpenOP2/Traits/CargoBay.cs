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
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Stores a set of actors to be added or removed by other traits.")]
	public class CargoBayInfo : TraitInfo
	{
		[Desc("Image of an empty bay.")]
		public string BayIconImage;

		[Desc("Sequence of an empty bay.")]
		public string BayIconSequence;

		[Desc("Number of bays.")]
		public int Capacity = 6;

		[Desc("Where the bay square is located.")]
		public CVec BayOffset = CVec.Zero;

		[Desc("List of actors the cargo bay is loaded with at start.")]
		public string[] StartsWith = Array.Empty<string>();

		public override object Create(ActorInitializer init) { return new CargoBay(init.World, init.Self, this); }
	}

	public class CargoBay
	{
		readonly Actor actor;
		readonly World world;
		readonly CargoBayInfo info;

		public CargoBayInfo Info => info;

		public readonly string[] Bays;

		public CargoBay(World world, Actor actor, CargoBayInfo info)
		{
			this.world = world;
			this.actor = actor;
			this.info = info;
			Bays = new string[info.Capacity];

			if (info.StartsWith.Length > 0)
			{
				for (var i = 0; i < Bays.Length && i < info.StartsWith.Length; i++)
				{
					var name = info.StartsWith[i];
					Bays[i] = name;
				}
			}
		}

		public bool GetCargo(string name)
		{
			if (Bays.Contains(name))
			{
				var index = Bays.IndexOf(name);
				Bays[index] = null;

				return true;
			}

			return false;
		}

		public bool CanAddCargo()
		{
			return Bays.Any(x => string.IsNullOrWhiteSpace(x));
		}

		public bool AddCargo(string name)
		{
			for (var i = 0; i < Bays.Length; i++)
			{
				var existing = Bays[i];
				if (string.IsNullOrEmpty(existing))
				{
					Bays[i] = name;
					return true;
				}
			}

			return false;
		}

		public CarriesCargo GetCargoCarrier()
		{
			var buildingInfo = actor.Trait<Building>();
			var carrierCell = buildingInfo.TopLeft + info.BayOffset;
			var actorsAtCell = world.ActorMap.GetActorsAt(carrierCell);

			return actorsAtCell.SelectMany(x => x.TraitsImplementing<CarriesCargo>()).FirstOrDefault();
		}
	}
}
