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
	[Desc("Loads palettes from a .prt file.")]
	class PaletteFromPrtFileInfo : ITraitInfo, IProvidesCursorPaletteInfo
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

		[FieldLoader.Require]
		[Desc("Choose the palette ID to use. The file contains multiple.")]
		public readonly int Number = 0;

		public readonly bool AllowModifiers = true;

		[Desc("Index set to be fully transparent/invisible.")]
		public readonly int TransparentIndex = 0;

		[Desc("Whether this palette is available for cursors.")]
		public readonly bool CursorPalette = false;

		public object Create(ActorInitializer init) { return new PaletteFromPrtFile(init.World, this); }

		string IProvidesCursorPaletteInfo.Palette { get { return CursorPalette ? Name : null; } }

		ImmutablePalette IProvidesCursorPaletteInfo.ReadPalette(IReadOnlyFileSystem fileSystem)
		{
			using (var s = fileSystem.Open(Filename))
			{
				var cpal = s.ReadASCII(4);
				if (cpal != "CPAL")
					throw new InvalidDataException();

				var framePalettes = new List<uint[]>();
				var paletteCount = s.ReadUInt32();
				for (var p = 0; p < paletteCount; p++)
				{
					var ppal = s.ReadASCII(4);
					if (ppal != "PPAL")
						throw new InvalidDataException();

					var offset = s.ReadUInt32();

					var head = s.ReadASCII(4);
					if (head != "head")
						throw new InvalidDataException();

					var bytesPerEntry = s.ReadUInt32();
					var unknown = s.ReadUInt32();

					var data = s.ReadASCII(4);
					if (data != "data")
						throw new InvalidDataException();

					var paletteSize = s.ReadUInt32();
					var colors = paletteSize / bytesPerEntry;
					var paletteData = new Color[colors];
					for (var c = 0; c < colors; c++)
					{
						var red = s.ReadByte();
						var green = s.ReadByte();
						var blue = s.ReadByte();
						paletteData[c] = Color.FromArgb(red, green, blue);
						var reserved = s.ReadByte();
					}

					framePalettes.Add(paletteData.Select(d => (uint)d.ToArgb()).ToArray());
				}

				return new ImmutablePalette(Enumerable.Range(0, Palette.Size).Select(i => (i == TransparentIndex) ? 0 : framePalettes[Number][i]));
			}
		}
	}

	class PaletteFromPrtFile : ILoadsPalettes, IProvidesAssetBrowserPalettes
	{
		readonly World world;
		readonly PaletteFromPrtFileInfo info;

		public PaletteFromPrtFile(World world, PaletteFromPrtFileInfo info)
		{
			this.world = world;
			this.info = info;
		}

		public void LoadPalettes(WorldRenderer wr)
		{
			wr.AddPalette(info.Name, ((IProvidesCursorPaletteInfo)info).ReadPalette(world.Map), info.AllowModifiers);
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
