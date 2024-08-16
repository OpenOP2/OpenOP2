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

using System.Collections.Generic;
using System.IO;
using OpenRA.Graphics;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class Op2TileSetLoader : ISpriteLoader
	{
		static bool IsBmp(Stream s)
		{
			var header = s.ReadASCII(4);
			return header == "BM88" || header == "PBMP";
		}

		static BitmapSpriteFrame[] ParseFrames(Stream s)
		{
			var start = s.Position;
			var frames = new List<BitmapSpriteFrame>();
			const int TileWidth = 32;
			_ = new Size(TileWidth * 8, TileWidth * 8);
			var size = new Size(TileWidth, TileWidth);
			_ = new Size(0, 0);
			s.Seek(12, SeekOrigin.Begin);
			_ = s.ReadDouble();
			var width = s.ReadUInt32();
			var height = s.ReadUInt32();
			var numTiles = height / width;
			s.Seek(1088, SeekOrigin.Begin);
			_ = s.ReadASCII(4);
			_ = s.ReadUInt32();
			for (var i = 0; i < numTiles; i++)
			{
				var bytes = s.ReadBytes(TileWidth * TileWidth);
				var tile = new BitmapSpriteFrame
				{
					Size = size,
					FrameSize = size,
					Data = bytes,
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
