using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Mods.OpenOP2.UtilityCommands;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class Prt
	{
		private const string PrtFilename = "op2_art.prt";

		private static Prt instance;
		public static Prt Instance
		{
			get
			{
				if (instance != null)
				{
					return instance;
				}

				instance = new Prt();
				return instance;
			}
		}

		public readonly PrtFile PrtFile;
		public readonly List<uint[]> FramePalettes;
		public readonly Dictionary<int, uint[]> Palettes;
		public readonly Dictionary<string, List<SequenceDTO>> Sequences;

		public Prt()
		{
			using (var stream = Game.ModData.DefaultFileSystem.Open(PrtFilename))
			{
				FramePalettes = LoadPalettes(stream);
				PrtFile = LoadGroups(stream, out Palettes);
			}

			Sequences = BuildSequences(PrtFile);
		}

		private Dictionary<string, List<SequenceDTO>> BuildSequences(PrtFile prtFile)
		{
			var results = new Dictionary<string, List<SequenceDTO>>();

			Action<SequenceDTO> addSequence = (seq) =>
			{
				if (results.ContainsKey(seq.Image))
				{
					results[seq.Image].Add(seq);
				}
				else
				{
					results.Add(seq.Image, new List<SequenceDTO> { seq });
				}
			};

			var groups = new GroupsFile();
			foreach (var group in groups.Groups)
			{
				var literalGroupSequenceSets = new List<GroupSequenceSet>();

				foreach (var set in group.Sets)
				{
					if (!string.IsNullOrWhiteSpace(set.UseFile))
					{
						literalGroupSequenceSets.Add(set);
						continue;
					}

					var typeGroupedFrames = GroupsFile.GetTypedGroupFrames(prtFile, group, set, out var frameCount);
					Func<SequenceSet, int, string> getSequenceName = (inGroup, ind) =>
					{
						Func<int, string> getTypeString = (inFrameType) =>
						{
							switch (inFrameType)
							{
								case 1:
									return "sprite";
								case 4:
									return "shadow";
								case 5:
									return "shadow";
								default:
									return "unknown";
							}
						};

						//////////////////////
						var frameTypeString = getTypeString(inGroup.FrameType);
						var seqName = $"{set.Sequence}-{frameTypeString}" + (ind == 0 ? string.Empty : $"-id{ind}");
						if ((inGroup.FrameType == 0 || inGroup.FrameType == 1) && ind == 0)
						{
							seqName = set.Sequence;
						}

						return seqName;
					};

					var allSequenceNames = typeGroupedFrames.Select((x, index) => getSequenceName(x, index)).ToArray();

					var zIndex = 0;
					var typeIndex = 0;
					foreach (var typeGroupedFrame in typeGroupedFrames)
					{
						var sequenceName = allSequenceNames[typeIndex];
						var outputSequence = GroupsFile.GetOutputSequence(sequenceName, set, typeGroupedFrame);
						if (group.WithBlankIdle && set == group.Sets.First() && typeGroupedFrame == typeGroupedFrames.First())
						{
							var result = new SequenceDTO()
							{
								Image = group.Name,
								Name = "idle",
								IsBlank = true,
								Length = 1,
								Facings = 1,
								Start = GroupsFile.EmptySprite,
							};

							addSequence(result);
						}

						var isFacingOverride = set.FacingsOverride > 0;
						var seq = new SequenceDTO()
						{
							Image = group.Name,
							Name = sequenceName,
							Length = isFacingOverride ? 1 : frameCount,
							Facings = isFacingOverride ? set.FacingsOverride : set.Length,
							Offset = new float3(set.OffsetX, set.OffsetY, 0),
							ZOffset = set.OffsetZ != 0 ? set.OffsetZ : zIndex,
							Tick = set.Tick ?? 40,
						};

						foreach (var frameset in outputSequence.Framesets)
						{
							var dto = new CombineSequenceDTO()
							{
								IsBlank = frameset.IsBlank,
								Frames = frameset.Frames.ToArray(),
								Length = frameset.Frames.Count,
								Offset = new float3(frameset.OffsetX, frameset.OffsetY, 0),
								ZOffset = set.OffsetZ != 0 ? set.OffsetZ : zIndex
							};

							seq.Combine.Add(dto);
						}

						addSequence(seq);

						typeIndex++;
						zIndex -= 256;
					}
				}

				// Hack in our literal sequences
				// Used for icons only for now
				foreach (var seq in literalGroupSequenceSets)
				{
					var seqDto = new SequenceDTO()
					{
						Image = group.Name,
						UseFile = seq.UseFile,
						Name = seq.Sequence,
						Length = seq.Length,
						Start = seq.Start,
						Facings = 1,
						Offset = new float3(seq.OffsetX, seq.OffsetY, 0),
						ZOffset = seq.OffsetZ,
						Tick = seq.Tick ?? 40,
					};

					addSequence(seqDto);
				}
			}

			return results;
		}

		private List<uint[]> LoadPalettes(Stream s)
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
				palettes.Add(f, FramePalettes[palette]);

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
	}
}
