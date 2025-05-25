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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class Op2RawLoader : ISpriteLoader
	{
		static bool IsRaw(Stream s)
		{
			var header = s.ReadASCII(4);
			return header == "????";
		}

		static BitmapSpriteFrame[] ParseFrames(Stream s)
		{
			const int NumTiles = 16;
			var start = s.Position;
			var frames = new List<BitmapSpriteFrame>();
			const int TileWidth = 32;

			const int NumPixels = TileWidth * TileWidth;
			const int NumBytesPerTile = NumPixels / 8;
			_ = new Size(TileWidth, TileWidth);
			var size = new Size(TileWidth, TileWidth);
			s.Seek(0, SeekOrigin.Begin);
			for (var i = 0; i < NumTiles; i++)
			{
				var bytes = s.ReadBytes(NumBytesPerTile);
				var swappedBytes = new List<byte>();
				for (var rowIndex = 0; rowIndex < TileWidth; rowIndex++)
				{
					var row = bytes.Skip(rowIndex * 4).Take(4).Reverse().ToList();
					swappedBytes.AddRange(row);
				}

				var newBytes = swappedBytes.ToArray();

				var bits = new BitArray(newBytes);
				var unpackedBytes = new byte[NumPixels];
				for (var byteI = 0; byteI < NumPixels; byteI++)
				{
					unpackedBytes[byteI] = (byte)(bits[byteI] ? 1 : 0);
				}

				var tile = new BitmapSpriteFrame
				{
					Size = size,
					FrameSize = size,
					Data = unpackedBytes,
					Type = SpriteFrameType.Indexed8,
				};

				frames.Add(tile);
			}

			s.Position = start;

			return frames.ToArray();
		}

		public bool TryParseSprite(Stream s, string filename, out ISpriteFrame[] frames, out TypeDictionary metadata)
		{
			metadata = null;
			if (!IsRaw(s))
			{
				frames = null;
				return false;
			}

			frames = ParseFrames(s);
			return true;
		}
	}
}
