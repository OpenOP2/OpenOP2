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
		const string OutputFilename = "..\\..\\mods\\openop2\\GROUP-START-VALUES.txt";

		// readonly StringBuilder sb = new StringBuilder();
		public bool TryParseSprite(Stream s, string filename, out ISpriteFrame[] frames, out TypeDictionary metadata)
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
			var frameList = new List<ISpriteFrame>();

			// Populate art data
			var prt = Prt.Instance;
			var prtFile = prt.PrtFile;
			var palettes = prt.Palettes;

			metadata = new TypeDictionary { new EmbeddedSpritePalette(framePalettes: palettes) };

			LoadAllImagesAsSpriteFrames(s, dataStart);

			frames = CombineGroupImagesIntoFrames(s);

			// using (var sw = new StreamWriter(OutputFilename))
			// {
			// 	sw.Write(sb.ToString());
			// }
			return true;
		}

		ISpriteFrame[] CombineGroupImagesIntoFrames(Stream s)
		{
			var prt = Prt.Instance;
			var prtFile = prt.PrtFile;
			var combinedFrames = new List<ISpriteFrame>();
			var shadowColor = Color.FromArgb(150, 0, 0, 0);
			var shadowColorInt = (uint)shadowColor.ToArgb();

			for (var groupIndex = 0; groupIndex < prtFile.Groups.Length; groupIndex++)
			{
				var group = prtFile.Groups[groupIndex];
				var groupWidth = group.SelLeft + group.SelRight + 10;
				var groupHeight = group.SelTop + group.SelBottom + 10;

				// sb.AppendLine($"Group index {groupIndex} starts at frame {frameTotal}");
				// Console.WriteLine($"GROUP: {groupIndex} RECT (LRTB): {group.SelLeft}, {group.SelRight}, {group.SelTop}, {group.SelBottom}  SIZE: {groupWidth} x {groupHeight}");
				for (var frameIndex = 0; frameIndex < group.Frames.Length; frameIndex++)
				{
					var frame = group.Frames[frameIndex];
					var frameColors = new uint[groupWidth, groupHeight];

					for (var picIndex = 0; picIndex < frame.Pictures.Length; picIndex++)
					{
						var picture = frame.Pictures[picIndex];
						var imgNumber = picture.ImgNumber;
						var imageInfo = prtFile.ImageHeader[imgNumber];
						var imgWidth = imageInfo.PaddedWidth;
						var picX = picture.PosX;
						var picY = picture.PosY;

						// Skip shadows for now
						var isShadow = imageInfo.ImageType == 4 || imageInfo.ImageType == 5;

						// Console.WriteLine($"  IMAGE: {imgWidth} x {imgHeight}");
						// Console.WriteLine($"  PIC X: {picX} PIC Y: {picY}");
						// Read every pixel of this image
						for (var i = 0; i < imageInfo.SpriteFrame.Data.Length; i++)
						{
							var y = (uint)i / imgWidth;
							var x = (uint)i % imgWidth;

							var colorIndex = imageInfo.SpriteFrame.Data[i];
							if (colorIndex == 0)
								continue;

							var palette = prt.FramePalettes[imageInfo.Palette];
							var pixelUint = palette[colorIndex];

							// Console.WriteLine($" Color (RGBA): {r} {g} {b} {a}");
							// Console.WriteLine($" Write color: {picX + x}, {picY + y}");
							var pixX = x + picX;
							var pixY = y + picY;
							if (pixX < 0 || pixY < 0 || pixX >= groupWidth || pixY >= groupHeight)
							{
								// Console.WriteLine($"Pixel out of range at: {pixX}, {pixY} (GROUP SIZE: {groupWidth} x {groupHeight})");
								// Console.WriteLine($"  Group index: {groupIndex} Frame index: {frameIndex}   Pic Index: {picIndex}   Img Number: {imgNumber}");
								continue;
							}

							frameColors[pixX, pixY] = isShadow ? shadowColorInt : pixelUint;
						}
					}

					// Read resulting group image into sprite frame
					var byteData = new byte[groupWidth * groupHeight * 4];
					for (var y = 0; y < groupHeight; y++)
					{
						for (var x = 0; x < groupWidth; x++)
						{
							var thisColor = frameColors[x, y];
							var colorIndex = ((y * groupWidth * 4) + (x * 4));
							var colorBytes = BitConverter.GetBytes(thisColor);

							byteData[colorIndex] = colorBytes[0];
							byteData[colorIndex + 1] = colorBytes[1];
							byteData[colorIndex + 2] = colorBytes[2];
							byteData[colorIndex + 3] = colorBytes[3];
						}
					}

					var spriteFrame = new BitmapSpriteFrame
					{
						Type = SpriteFrameType.Rgba32,
						Data = byteData,
						FrameSize = new Size(groupWidth, groupHeight),

						// Offset = new float2(group.SelLeft, group.SelTop),
						Size = new Size(groupWidth, groupHeight)
					};

					combinedFrames.Add(spriteFrame);
				}
			}

			return combinedFrames.ToArray();
		}

		ISpriteFrame[] LoadAllImagesAsSpriteFrames(Stream s, uint dataStart)
		{
			var prt = Prt.Instance;
			var prtFile = prt.PrtFile;
			var frameList = new List<ISpriteFrame>();
			const byte ShadowTileIndex = 1;

			for (var imgIndex = 0; imgIndex < prtFile.ImageCount; imgIndex++)
			{
				var img = prtFile.ImageHeader[imgIndex];
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
					const int BitsInAByte = 8;
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
							var reversedBitIndex = 7 - (i % BitsInAByte);
							var byteFloor = i - (i % BitsInAByte);

							var lookupIndex = byteFloor + reversedBitIndex;
							processedData[i] = (byte)(bits[lookupIndex] ? ShadowTileIndex : 0);
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
					Type = SpriteFrameType.Indexed8,
				};

				frameList.Add(img.SpriteFrame);
			}

			return frameList.ToArray();
		}
	}
}
