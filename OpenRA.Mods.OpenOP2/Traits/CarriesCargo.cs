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
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Carries a set of actor names to be picked up or disbursed.")]
	public class CarriesCargoInfo : TraitInfo
	{
		[Desc("Image of empty cargo.")]
		public string EmptyImage;

		[Desc("Sequence of empty cargo.")]
		public string EmptySequence;

		[Desc("Number of cargo bays.")]
		public int Capacity = 1;

		[Desc("List of actors the cargo bays are loaded with at start.")]
		public string[] StartsWith = Array.Empty<string>();

		public override object Create(ActorInitializer init) { return new CarriesCargo(this); }
	}

	public class CarriesCargo
	{
		readonly CarriesCargoInfo info;

		public CarriesCargoInfo Info => info;

		public readonly string[] Bays;

		public CarriesCargo(CarriesCargoInfo info)
		{
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
	}
}
