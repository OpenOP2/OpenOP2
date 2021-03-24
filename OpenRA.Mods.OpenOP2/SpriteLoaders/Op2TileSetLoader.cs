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
using System.Collections.Generic;
using System.IO;
using OpenRA.Graphics;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class Op2TileSetLoader : ISpriteLoader
	{
		private static bool IsBmp(Stream s)
		{
			var header = s.ReadASCII(4);
			return header == "BM88" || header == "PBMP";
		}

		private static BitmapSpriteFrame[] ParseFrames(Stream s)
		{
			var start = s.Position;
			var frames = new List<BitmapSpriteFrame>();
			const int tileWidth = 32;
			var dataSize = new Size(tileWidth * 8, tileWidth * 8);
			var size = new Size(tileWidth, tileWidth);
			var frameSize = new Size(0, 0);
			s.Seek(10, SeekOrigin.Begin);
			var wtf32 = s.ReadUInt16();
			var shit = s.ReadUInt32();
			var shit2 = s.ReadUInt32();
			s.Seek(1088, SeekOrigin.Begin);
			var wtf2 = s.ReadASCII(4);
			const int numTiles = 128;
			for (var i = 0; i < numTiles; i++)
			{
				var bytes = s.ReadBytes(tileWidth * tileWidth);
				var tile = new BitmapSpriteFrame
				{
					Size = size,
					FrameSize = size,
					Data = bytes,
					Type = SpriteFrameType.Indexed,
				};

				frames.Add(tile);
			}

			s.Position = start;

			return frames.ToArray();
		}

		public bool TryParseSprite(Stream s, out ISpriteFrame[] frames, out TypeDictionary metadata)
		{
			metadata = null;
			if (!IsBmp(s))
			{
				frames = null;
				return false;
			}

			frames = ParseFrames(s);
			return true;
		}
	}
}
