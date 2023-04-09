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
using OpenRA.Graphics;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Defines icons and names to be used for a custom palette (see UseCustomProductionPalettes/CustomProductionPalette).")]
	public class UseCustomPaletteIconsInfo : TraitInfo
	{
		[Desc("The name of the custom palette icons group.")]
		public string Name;

		[FieldLoader.LoadUsing("LoadCustomPaletteIcons")]
		public Dictionary<string, CustomPaletteIconsInfo> CustomPaletteIcons;

		static object LoadCustomPaletteIcons(MiniYaml yaml)
		{
			var retList = new Dictionary<string, CustomPaletteIconsInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "CustomPaletteIcons");

			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("Icon")))
			{
				var ret = new CustomPaletteIconsInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}

		public override object Create(ActorInitializer init) { return new UseCustomPaletteIcons(this); }
	}

	public class CustomPaletteIconsInfo
	{
		[Desc("The name of the item.")]
		public string Name;

		[Desc("The name of the image.")]
		public string ImageName;

		[Desc("The name of the sequence.")]
		public string SequenceName;

		[PaletteReference]
		[Desc("The name of the palette.")]
		public string Palette;
	}

	public class UseCustomPaletteIcons
	{
		readonly UseCustomPaletteIconsInfo info;

		public UseCustomPaletteIconsInfo Info => info;

		public UseCustomPaletteIcons(UseCustomPaletteIconsInfo info)
		{
			this.info = info;
		}
	}
}
