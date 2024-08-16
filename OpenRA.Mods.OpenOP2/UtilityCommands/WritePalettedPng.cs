#region Copyright & License Information
/*
 * Copyright 2007-2018 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using OpenRA.FileFormats;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	sealed class WritePalettedPngCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--write-paletted-png";
		bool IUtilityCommand.ValidateArguments(string[] args) { return true; }

		[Desc("Writes a patterned indexed8 PNG out to c:\\temp\\!overlay.png.")]
		void IUtilityCommand.Run(Utility utility, string[] _) { Run(utility); }

		static void Run(Utility utility)
		{
			const byte EmptyIndex = 0;
			const byte ValidIndex = 150;
			const byte InvalidIndex = 149;
			const string FilePath = "C:\\temp\\!overlay.png";

			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = utility.ModData;

			var height = 32;
			var width = 64;
			var bytes = new byte[height * width];
			var index = 0;
			for (var y = 0; y < height; y++)
			{
				var xOffset = y % 2 != 0 ? 2 : 0;
				for (var x = 0; x < width; x++)
				{
					var pixelIndex = ValidIndex;
					if (x >= 32)
					{
						pixelIndex = InvalidIndex;
					}

					Console.WriteLine($"{index}: {x}, {y}");

					if ((x + xOffset) % 4 == 0)
					{
						bytes[index] = pixelIndex;
					}
					else
					{
						bytes[index] = EmptyIndex;
					}

					index++;
				}
			}

			var validColor = Color.White;
			var invalidColor = Color.Red;
			var palette = new Color[byte.MaxValue];
			for (var i = 0; i < palette.Length; i++)
			{
				palette[i] = validColor;
			}

			palette[0] = Color.Transparent;
			palette[ValidIndex] = validColor;
			palette[InvalidIndex] = invalidColor;

			var png = new Png(bytes, OpenRA.Graphics.SpriteFrameType.Indexed8, width, height, palette, null);
			png.Save(FilePath);

			Console.WriteLine($"'{FilePath}' saved.");
		}
	}
}
