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
using OpenRA.FileFormats;
using OpenRA.FileSystem;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Loads palettes from a .prt file.")]
	class PaletteFromPrtFileInfo : TraitInfo, IProvidesCursorPaletteInfo
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

		public override object Create(ActorInitializer init) { return new PaletteFromPrtFile(init.World, this); }

		string IProvidesCursorPaletteInfo.Palette { get { return CursorPalette ? Name : null; } }

		ImmutablePalette IProvidesCursorPaletteInfo.ReadPalette(IReadOnlyFileSystem fileSystem)
		{
			using (var s = fileSystem.Open(Filename))
			{
				var cpal = s.ReadASCII(4);
				if (cpal != "CPAL")
					throw new InvalidDataException();

				uint[] framePalettes;
				var paletteCount = s.ReadUInt32();
				s.Seek(1052 * Number, SeekOrigin.Current);

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

				WritePaletteToPng(paletteData, Number);

				framePalettes = paletteData.Select(d => (uint)d.ToArgb()).ToArray();

				return new ImmutablePalette(Enumerable.Range(0, Palette.Size).Select(i => (i == TransparentIndex) ? 0 : framePalettes[i]));
			}
		}

		void WritePaletteToPng(Color[] colors, int number)
		{
			var blockSize = 8;
			var colorMap = new Color[16 * blockSize, 16 * blockSize];
			for (var y = 0; y < 16; y++)
			{
				for (var x = 0; x < 16; x++)
				{
					var color = colors[(y * 16) + x];
					for (var blockY = 0; blockY < blockSize; blockY++)
					{
						for (var blockX = 0; blockX < blockSize; blockX++)
						{
							colorMap[(x * blockSize) + blockX, (y * blockSize) + blockY] = color;
						}
					}
				}
			}

			var bytes = new byte[256 * blockSize * blockSize * 4];
			var i = 0;
			for (var y = 0; y < 16 * blockSize; y++)
			{
				for (var x = 0; x < 16 * blockSize; x++)
				{
					var thisColor = colorMap[x, y];
					bytes[i] = thisColor.R;
					bytes[i + 1] = thisColor.G;
					bytes[i + 2] = thisColor.B;
					bytes[i + 3] = thisColor.A;
					i += 4;
				}
			}

			var png = new Png(bytes, SpriteFrameType.Rgba32, 16 * blockSize, 16 * blockSize);
			png.Save($"..\\..\\palette{number}.png");
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
				if ((info.Tilesets.Count == 0 || info.Tilesets.Contains(world.Map.Rules.TerrainInfo.Id))
					&& !info.ExcludeTilesets.Contains(world.Map.Rules.TerrainInfo.Id))
					yield return info.Name;
			}
		}
	}
}
