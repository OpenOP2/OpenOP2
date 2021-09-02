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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRA.FileSystem;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Loads palettes from a .bmp file.")]
	class PaletteFromBmpFileInfo : TraitInfo, IProvidesCursorPaletteInfo
	{
		[PaletteDefinition]
		[FieldLoader.Require]
		[Desc("Palette name used internally.")]
		public readonly string Name = null;

		[Desc("Defines for which tileset IDs this palette should be loaded.",
			"If none specified, it applies to all tileset IDs not explicitly excluded.")]
		public readonly HashSet<string> Tilesets = new HashSet<string>();

		[Desc("Don't load palette for these tileset IDs.")]
		public readonly HashSet<string> ExcludeTilesets = new HashSet<string>();

		[FieldLoader.Require]
		[Desc("Name of the file to load.")]
		public readonly string Filename = null;

		public readonly bool AllowModifiers = true;

		[Desc("Whether this palette is available for cursors.")]
		public readonly bool CursorPalette = false;

		public override object Create(ActorInitializer init) { return new PaletteFromBmpFile(init.World, this); }

		string IProvidesCursorPaletteInfo.Palette { get { return CursorPalette ? Name : null; } }

		ImmutablePalette IProvidesCursorPaletteInfo.ReadPalette(IReadOnlyFileSystem fileSystem)
		{
			using (var s = fileSystem.Open(Filename))
			{
				s.Seek(56, SeekOrigin.Begin);
				var cpal = s.ReadASCII(4);
				if (cpal != "data")
					throw new InvalidDataException();

				var something = s.ReadUInt32();

				var framePalettes = new List<uint>();
				var colors = 256;
				var paletteData = new Color[colors];
				for (var c = 0; c < colors; c++)
				{
					var red = s.ReadByte();
					var green = s.ReadByte();
					var blue = s.ReadByte();
					paletteData[c] = Color.FromArgb(red, green, blue);
					var reserved = s.ReadByte();
				}

				return new ImmutablePalette(paletteData.Select(d => (uint)d.ToArgb()).ToArray());
			}
		}
	}

	class PaletteFromBmpFile : ILoadsPalettes, IProvidesAssetBrowserPalettes
	{
		readonly World world;
		readonly PaletteFromBmpFileInfo info;

		public PaletteFromBmpFile(World world, PaletteFromBmpFileInfo info)
		{
			this.world = world;
			this.info = info;
		}

		public void LoadPalettes(WorldRenderer wr)
		{
			if (info.Tilesets?.Count > 0)
			{
				// If it's tileset-specific, only load it if it matches the tileset of this map
				if (info.Tilesets.Any(tileSet => string.Equals(tileSet, world.Map.Tileset, StringComparison.InvariantCultureIgnoreCase)))
					wr.AddPalette(info.Name, ((IProvidesCursorPaletteInfo)info).ReadPalette(world.Map), info.AllowModifiers);
			}
			else
			{
				// Else, add it as normal
				wr.AddPalette(info.Name, ((IProvidesCursorPaletteInfo)info).ReadPalette(world.Map), info.AllowModifiers);
			}
		}

		public IEnumerable<string> PaletteNames
		{
			get
			{
				// Only expose the palette if it is available for the shellmap's tileset (which is a requirement for its use).
				if ((info.Tilesets.Count == 0 || info.Tilesets.Contains(world.Map.Rules.TileSet.Id))
					&& !info.ExcludeTilesets.Contains(world.Map.Rules.TileSet.Id))
					yield return info.Name;
			}
		}
	}
}
