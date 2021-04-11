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

		PrtFile LoadGroups(Stream s, out Dictionary<int, uint[]> palettes)
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
			h.Groups = new ImageGroup[h.AllGroupCount];

			for (var i = 0; i < h.AllGroupCount; i++)
			{
				var img = new ImageGroup
				{
					Unknown1 = s.ReadInt32(),
					SelLeft = s.ReadInt32(),
					SelTop = s.ReadInt32(),
					SelRight = s.ReadInt32(),
					SelBottom = s.ReadInt32(),
					CenterX = s.ReadInt32(),
					CenterY = s.ReadInt32(),
					Unknown8 = s.ReadInt32(),
					FrameCount = s.ReadInt32()
				};

				img.Frames = new Op2Frame[img.FrameCount];

				for (var j = 0; j < img.FrameCount; j++)
				{
					var frame = new Op2Frame
					{
						PicCount = s.ReadUInt8(),
						Unknown = s.ReadUInt8(),
					};

					frame.ExtUnknown1 = new BytePair[frame.PicCount >> 7];
					for (var k = 0; k < frame.PicCount >> 7; k++)
					{
						var bp = new BytePair
						{
							Byte1 = s.ReadUInt8(),
							Byte2 = s.ReadUInt8(),
						};

						frame.ExtUnknown1[k] = bp;
					}

					frame.ExtUnknown2 = new BytePair[frame.Unknown >> 7];
					for (var k = 0; k < frame.Unknown >> 7; k++)
					{
						var bp = new BytePair
						{
							Byte1 = s.ReadUInt8(),
							Byte2 = s.ReadUInt8(),
						};

						frame.ExtUnknown2[k] = bp;
					}

					frame.Pictures = new Op2Picture[frame.PicCount & 0x7F];
					for (var k = 0; k < frame.Pictures.Length; k++)
					{
						var pic = new Op2Picture
						{
							ImgNumber = s.ReadInt16(),
							Reserved = s.ReadUInt8(),
							PicOrder = s.ReadUInt8(),
							PosX = s.ReadInt16(),
							PosY = s.ReadInt16()
						};

						frame.Pictures[k] = pic;
					}

					img.Frames[j] = frame;
				}

				img.GroupExtCount = s.ReadInt32();
				img.Extended = new GroupExt[img.GroupExtCount];

				for (var j = 0; j < img.GroupExtCount; j++)
				{
					var ext = new GroupExt
					{
						Unknown1 = s.ReadInt32(),
						Unknown2 = s.ReadInt32(),
						Unknown3 = s.ReadInt32(),
						Unknown4 = s.ReadInt32(),
					};

					img.Extended[j] = ext;
				}

				h.Groups[i] = img;
			}

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
			prtFile = LoadGroups(prt, out var palettes);
			var frameList = new List<ISpriteFrame>();

			// Populate art data
			const byte shadowTileIndex = 1;
			foreach (var img in prtFile.ImageHeader)
			{
				var isShadow = img.ImageType is 4 or 5;

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
					var numRows = (int)img.Height;
					if (hasExtraRow)
					{
						numRows *= 2;
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
						if (hasExtraRow)
						{
							startIndex *= 2;
						}

						var firstX = processedData.Skip(startIndex).Take((int)img.Width);
						newData.AddRange(firstX);
						newData.AddRange(Enumerable.Repeat<byte>(0, paddedDiff));
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
			}

			metadata = new TypeDictionary { new EmbeddedSpritePalette(framePalettes: palettes) };

			frames = frameList.ToArray();

			return true;
		}

		private byte EncodeBool(IEnumerable<bool> array)
		{
			byte val = 0;
			foreach (var arrayValue in array)
			{
				val <<= 1;
				if (arrayValue)
				{
					val |= 1;
				}
			}

			return val;
		}
	}
}
