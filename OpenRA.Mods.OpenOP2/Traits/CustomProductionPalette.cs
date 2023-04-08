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

using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Along with UseCustomProductionPalettes, shows a custom sidebar palette when selected.")]
	public class CustomProductionPaletteInfo : TraitInfo
	{
		[Desc("Name of custom palette. Must match entry in UseCustomProductionPalettes.CustomProductionPalettes.")]
		public readonly string Name;

		public override object Create(ActorInitializer init) { return new CustomProductionPalette(this); }
	}

	public class CustomProductionPalette
	{
		readonly CustomProductionPaletteInfo info;

		public string Name => info.Name;

		public CustomProductionPalette(CustomProductionPaletteInfo info)
		{
			this.info = info;
		}
	}
}
