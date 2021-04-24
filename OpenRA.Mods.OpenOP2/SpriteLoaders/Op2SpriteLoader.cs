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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class Op2SpriteLoader : ISpriteLoader
	{
		private PrtFile prtFile;
		private List<uint[]> framePalettes;

		List<uint[]> LoadPalettes(Stream s)
		{
			var cpal = s.ReadASCII(4);
			if (cpal != "CPAL")
				throw new InvalidDataException();

			var palettes = new List<uint[]>();
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
					var blue = s.ReadByte();
					var green = s.ReadByte();
					var red = s.ReadByte();
					paletteData[c] = Color.FromArgb(red, green, blue);
					var reserved = s.ReadByte();
				}

				palettes.Add(paletteData.Select(d => (uint)d.ToArgb()).ToArray());
			}

			return palettes;
		}

		PrtFile LoadImageHeader(Stream s, out Dictionary<int, uint[]> palettes)
		{
			var spriteCount = s.ReadUInt32();
			var h = new PrtFile()
			{
				ImageCount = (int)spriteCount,
				ImageHeader = new Op2Image[spriteCount]
			};

			palettes = new Dictionary<int, uint[]>();
			var rawFrames = h.ImageHeader;
			for (var f = 0; f < rawFrames.Length; f++)
			{
				var paddedWidth = s.ReadUInt32();
				var dataOffset = s.ReadUInt32();
				var height = s.ReadUInt32();
				var width = s.ReadUInt32();
				var type = s.ReadUInt16();
				var palette = s.ReadUInt16();
				palettes.Add(f, framePalettes[palette]);

				var img = new Op2Image
				{
					PaddedWidth = paddedWidth,
					DataOffset = dataOffset,
					Height = height,
					Width = width,
					ImageType = type,
					Palette = palette,
				};

				h.ImageHeader[f] = img;
			}

			h.AllGroupCount = s.ReadInt32();
			h.AllFrameCount = s.ReadInt32();
			h.AllPicCount = s.ReadInt32();
			h.AllExtInfoCount = s.ReadInt32();
			return h;
		}

		public bool TryParseSprite(Stream s, out ISpriteFrame[] frames, out TypeDictionary metadata)
		{
			var start = s.Position;

			var signature = s.ReadASCII(2);
			if (signature != "BM")
			{
				s.Position = start;
				metadata = null;
				frames = null;
				return false;
			}

			var size = s.ReadUInt32();
			var reserved1 = s.ReadUInt16();
			var reserved2 = s.ReadUInt16();
			var dataStart = s.ReadUInt32();

			var prt = Game.ModData.DefaultFileSystem.Open("op2_art.prt");
			framePalettes = LoadPalettes(prt);
			prtFile = LoadImageHeader(prt, out var palettes);
			var frameList = new List<ISpriteFrame>();

			// Populate art data
			const byte shadowTileIndex = 1;
			var imgIndex = 0;
			foreach (var img in prtFile.ImageHeader)
			{
				var isShadow = img.ImageType == 4 || img.ImageType == 5;

				var dataSize = new Size((int)img.PaddedWidth, (int)img.Height);
				var frameSize = new Size((int)img.Width, (int)img.Height);
				s.Seek(dataStart + img.DataOffset, SeekOrigin.Begin);

				byte[] data;
				var numBytes = (int)(img.PaddedWidth * img.Height);

				if (isShadow)
				{
					var newSize = (int)(img.PaddedWidth * img.Height);
					var rowSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(img.Width) / Math.Log(2)));
					var hasExtraRow = rowSize == (int)img.PaddedWidth;
					var tempData = s.ReadBytes(numBytes);
					var bits = new BitArray(tempData);
					var processedData = new byte[newSize * 8];
					var paddedWidth = img.Width;
					const int bitsInAByte = 8;
					var numRows = (int)img.Height * 2; // TODO: This is a hack. Not sure why we need to double this
					if (hasExtraRow)
					{
						numRows *= 2;
					}

					// HACK: HACK HACK HACK
					// Obviously our rowSize calculation is borked, so correct some things here
					if (rowSize < 32)
					{
						rowSize = 32;
					}

					if (img.ImageType == 5)
					{
						if (rowSize == 128)
						{
							rowSize = 96;
						}
						else if (rowSize == 256)
						{
							rowSize = 128;
						}
					}

					for (var y = 0; y < numRows; y++)
					{
						for (var x = 0; x < img.Width; x++)
						{
							var i = (int)((y * paddedWidth) + x);
							var reversedBitIndex = 7 - (i % bitsInAByte);
							var byteFloor = i - (i % bitsInAByte);

							var lookupIndex = byteFloor + reversedBitIndex;
							processedData[i] = (byte)(bits[lookupIndex] ? shadowTileIndex : 0);
						}
					}

					var paddedDiff = (int)(img.PaddedWidth - img.Width);
					var newData = new List<byte>();

					for (var i = 0; i < img.Height; i++)
					{
						var startIndex = i * rowSize;

						var firstX = processedData.Skip(startIndex).Take((int)img.Width).ToArray();
						newData.AddRange(firstX);
						newData.AddRange(Enumerable.Repeat<byte>(0, paddedDiff));

						// if (imgIndex == 1758)
						// {
						// 	var ss = imgIndex;
						// }
					}

					if (img.PaddedWidth * img.Height > newData.Count)
					{
						throw new ArgumentException("Image size did not match number of bytes.");
					}

					data = newData.ToArray();
				}
				else
				{
					data = s.ReadBytes(numBytes);
				}

				img.SpriteFrame = new BitmapSpriteFrame
				{
					Size = dataSize,
					FrameSize = frameSize,
					Data = data,
					Type = SpriteFrameType.Indexed,
				};
				frameList.Add(img.SpriteFrame);

				imgIndex++;
			}

			metadata = new TypeDictionary { new EmbeddedSpritePalette(framePalettes: palettes) };

			frames = frameList.ToArray();

			return true;
		}
	}
}
