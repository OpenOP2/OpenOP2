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
using OpenRA.Primitives;

namespace OpenRA.Mods.Dr.SpriteLoaders
{
	public class PrtLoader : ISpriteLoader
	{
		PrtFile file;

		class PrtFile
		{
			public string ID; // CPAL
			public int PalCount;
			public Ppal[] PalData;
			public int ImageCount;
			public Op2Image[] ImageHeader;
			public int AllGroupCount;
			public int AllFrameCount;
			public int AllPicCount;
			public int AllExtInfoCount;
			public ImageGroup[] Groups;
		}

		class Ppal
		{
			public string ID; // PPAL
			public int Size;
			public string HeadID; // head
			public int BytesPerEntry;
			public int Unknown;
			public string DataID; // data
			public int PalSize; // 0x0400
			public RgbQuad[] PaletteData;
		}

		class RgbQuad
		{
			public int Red;
			public int Green;
			public int Blue;
			public int Reserved;
		}

		class Op2Image
		{
			public int SizeScanline;
			public byte[] ImgData;
			public int SizeX;
			public int SizeY;
			public short Unknown;
			public short Palette;
		}

		class ImageGroup
		{
			public int Unknown1;
			public int SelLeft;
			public int SelTop;
			public int SelRight;
			public int SelBottom;
			public int CenterX;
			public int CenterY;
			public int Unknown8;
			public int FrameCount;
			public Op2Frame[] Frames;
			public int GroupExtCount;
			public GroupExt[] Extended;
		}

		class GroupExt
		{
			public int Unknown1;
			public int Unknown2;
			public int Unknown3;
			public int Unknown4;
		}

		class Op2Frame
		{
			public byte PicCount;
			public byte Unknown;
			public BytePair[] ExtUnknown1;
			public BytePair[] ExtUnknown2;
			public Op2Picture[] Pictures;
		}

		class BytePair
		{
			public byte Byte1;
			public byte Byte2;
		}

		class Op2Picture
		{
			public short ImgNumber;
			public byte Reserved; // standard 0xFF
			public byte PicOrder;
			public short PosX;
			public short PosY;
		}

		class Op2SpriteFrameInfo
		{
		}

		class Op2SpriteFrame : ISpriteFrame
		{
			public SpriteFrameType Type { get; private set; }
			public Size Size { get; private set; }
			public Size FrameSize { get; private set; }
			public float2 Offset { get; private set; }
			public byte[] Data { get; set; }
			public bool DisableExportPadding { get { return false; } }

			public Op2SpriteFrame(Stream s, PrtFile head, Op2SpriteFrameInfo info)
			{
				/*
				Type = SpriteFrameType.Indexed;
				var picindex = info.A * sph.Nrots + info.R;
				var readInt = new Func<int, int>((off) =>
				{
					s.Position = off;
					return s.ReadInt32();
				});

				var picnr = readInt(HeaderSize + picindex * 4);
				if (picnr >= sph.Npics)
					throw new Exception("Pic number was greater or equal to number of pics.");

				var picoff = readInt(sph.OffPicoffs + 8 * picnr);
				var nextpicoff = readInt(sph.OffPicoffs + 8 * (picnr + 1));
				var start = s.Position;
				s.Position = sph.OffBits + picoff;
				var tempData = s.ReadBytes(nextpicoff - picoff);
				Data = new byte[(sph.Szx + 2) * sph.Szy];

				var pixindex = new Func<int, int, int>((x, y) =>
				{
					int vr = (y * sph.Szx) + x;
					return vr;
				});

				var curr = 0;
				for (var l = 0; l < sph.Szy; ++l)
				{
					int step = 0, currx = 0, cnt, i;
					while (currx < sph.Szx)
					{
						cnt = tempData[curr++];
						if ((step & 1) != 0)
							cnt &= 0x7f;
						if ((step & 1) != 0)
						{
							if (!sph.IsShadow)
							{
								for (i = 0; i < cnt; ++i, ++curr)
								{
									int newIndex = pixindex(currx + i, l);
									Data[newIndex] = tempData[curr];
								}
							}
							else
							{
								for (i = 0; i < cnt; ++i)
								{
									int newIndex = pixindex(currx + i, l);
									Data[newIndex] = 47;
								}
							}
						}

						currx += cnt;
						++step;
					}

					if (currx != sph.Szx)
						throw new Exception("Current x was not equal to the line size.");
				}

				Offset = new float2(0, 0);
				FrameSize = new Size(sph.Szx, sph.Szy);
				Size = FrameSize;

				s.Position = start;
				*/
			}
		}

		private bool IsPrt(Stream s)
		{
			var start = s.Position;
			var h = new PrtFile()
			{
				ID = s.ReadASCII(4),
				PalCount = s.ReadInt32(),
			};

			if (h.ID != "CPAL")
			{
				s.Position = start;
				return false;
			}

			h.PalData = new Ppal[h.PalCount];

			// Parse palettes
			for (var i = 0; i < h.PalCount; i++)
			{
				var ppal = new Ppal
				{
					ID = s.ReadASCII(4),
					Size = s.ReadInt32(),
					HeadID = s.ReadASCII(4),
					BytesPerEntry = s.ReadInt32(),
					Unknown = s.ReadInt32(),
					DataID = s.ReadASCII(4),
					PalSize = s.ReadInt32()
				};

				if (ppal.ID != "PPAL" || ppal.HeadID != "head" || ppal.DataID != "data")
				{
					s.Position = start;
					return false;
				}

				var numPaletteEntries = ppal.PalSize / ppal.BytesPerEntry;

				ppal.PaletteData = new RgbQuad[numPaletteEntries];

				// Populate palette
				for (var j = 0; j < numPaletteEntries; j++)
				{
					var rgba = new RgbQuad
					{
						Red = s.ReadByte(),
						Green = s.ReadByte(),
						Blue = s.ReadByte(),
						Reserved = s.ReadByte(),
					};

					ppal.PaletteData[j] = rgba;
				}

				h.PalData[i] = ppal;
			}

			h.ImageCount = s.ReadInt32();
			h.ImageHeader = new Op2Image[h.ImageCount];

			for (var i = 0; i < h.ImageCount; i++)
			{
				var img = new Op2Image
				{
					SizeScanline = s.ReadInt32(),
				};

				img.ImgData = s.ReadBytes(img.SizeScanline);
				img.SizeX = s.ReadInt32();
				img.SizeY = s.ReadInt32();
				img.Unknown = s.ReadInt16();
				img.Palette = s.ReadInt16();
				h.ImageHeader[i] = img;
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

			file = h;

			return true;
		}

		Op2SpriteFrame[] ParseFrames(Stream s, out TypeDictionary metadata)
		{
			var start = s.Position;

			metadata = new TypeDictionary();
			var frames = new List<Op2SpriteFrame>();

			// for (int sect = 0; sect < header.Nsects; ++sect)
			// {
			// 	s.Position = header.OffSections + 16 * sect;
			// 	var firstanim = s.ReadInt32();
			// 	var lastanim = s.ReadInt32();
			// 	s.ReadInt32(); // Framerate
			// 	var numhotspots = s.ReadInt32();
			//
			// 	var bmp_szx = header.Szx * header.Nrots;
			// 	var bmp_szy = header.Szy * (lastanim - firstanim + 1);
			//
			// 	var rotOffset = 0;
			// 	if (header.Nrots >= 4)
			// 		rotOffset = header.Nrots / 4;
			//
			// 	for (var r = 0; r < header.Nrots; ++r)
			// 	{
			// 		var newR = r + rotOffset;
			// 		if (newR >= header.Nrots)
			// 			newR -= header.Nrots;
			//
			// 		for (var a = firstanim; a <= lastanim; ++a)
			// 		{
			// 			var sfi = new SprFrameInfo()
			// 			{
			// 				A = a,
			// 				R = newR,
			// 				S = sect,
			// 				BmpSzx = bmp_szx,
			// 				BmpSzy = bmp_szy,
			// 				Lastanim = lastanim
			// 			};
			// 			var frame = new DrSprFrame(s, header, sfi);
			// 			frames.Add(frame);
			// 		}
			// 	}
			//
			// 	if (numhotspots > 0)
			// 	{
			// 		int off_hotspots, h;
			// 		s.Seek(header.OffPicoffs + 8 * header.Npics, SeekOrigin.Begin);
			// 		off_hotspots = header.OffBits;
			// 		for (h = 0; h < numhotspots; ++h)
			// 		{
			// 			int frameindex = 0;
			// 			for (int r = 0; r < header.Nrots; ++r)
			// 			{
			// 				for (int a = firstanim; a <= lastanim; ++a)
			// 				{
			// 					int picindex = a * header.Nrots + r;
			// 					var read_int = new Func<int, int>((off) =>
			// 					{
			// 						s.Position = off;
			// 						return s.ReadInt32();
			// 					});
			//
			// 					int headersize = 32;
			// 					int picnr = read_int(headersize + picindex * 4);
			// 					int hotoff = read_int(header.OffPicoffs + 8 * picnr + 4);
			// 					s.Position = off_hotspots + 4 + 3 * (hotoff + h);
			// 					byte hx = s.ReadUInt8();
			// 					byte hy = s.ReadUInt8();
			//
			// 					metadata.Add(new DrFrameMetadata()
			// 					{
			// 						Hotspot = new int2(hx, hy),
			// 						FrameIndex = frameindex
			// 					});
			//
			// 					frameindex++;
			// 				}
			// 			}
			// 		}
			// 	}
			// }

			s.Position = start;
			return frames.ToArray();
		}

		public bool TryParseSprite(Stream s, out ISpriteFrame[] frames, out TypeDictionary metadata)
		{
			metadata = null;

			if (!IsPrt(s))
			{
				frames = null;
				return false;
			}

			frames = ParseFrames(s, out metadata);
			return true;
		}
	}
}
