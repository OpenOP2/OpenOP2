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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class CreateSequencesCommand : IUtilityCommand
	{
		class AnimationFacing
		{
			public AnimationFrame[] Frames;
		}

		class AnimationFrame
		{
			public int Frame;
			public int OffsetX;
			public int OffsetY;
		}

		class OutputSequence
		{
			public string Name;
			public List<OutputSequenceFrameset> Framesets = new List<OutputSequenceFrameset>();
		}

		class OutputSequenceFrameset
		{
			public string Name;
			public bool IsBlank;
			public int OffsetX;
			public int OffsetY;
			public List<int> Frames = new List<int>();
		}

		class SequenceSet
		{
			public int FrameType;
			public int Palette;
			public int PicOrder;
			public int OffsetX;
			public int OffsetY;
			public AnimationFacing[] AnimationFacings;
			public string Name { get; set; }

			public AnimationFrame[] GetFacingArray(int facing)
			{
				var frames = AnimationFacings[facing].Frames;
				var isEvenNumberOfFrames = frames.Length % 2 == 0;
				if (isEvenNumberOfFrames)
				{
					var halfFrames = frames.Length / 2;
					var isFirstHalfPopulated = frames.Take(halfFrames).All(x => x != null);
					var isSecondHalfUnpopulated = frames.Skip(halfFrames).Take(halfFrames).All(x => x == null);
					if (isFirstHalfPopulated && isSecondHalfUnpopulated)
					{
						frames = frames.Take(halfFrames).Concat(frames.Take(halfFrames)).ToArray();
					}
				}

				return AnimationFacings[facing].Frames.ToArray();
			}
		}

		class ActorOverlay
		{
			public string SequenceName;
			public string Palette;
		}

		class ActorRule
		{
			public string Name;
			public string Palette;
			public bool CreateExampleActor = true;
			public bool CreateActor = true;
			public ActorType ActorType;
			public List<ActorOverlay> Overlays = new List<ActorOverlay>();
		}

		private const string OutputFilename = "..\\mods\\openop2\\sequences\\sequences-generated.yaml";
		private const string RulesOutputFilename = "..\\mods\\openop2\\rules\\rules-generated.yaml";
		private const string RulesExampleOutputFilename = "..\\mods\\openop2\\rules\\rules-example.yaml";

		string IUtilityCommand.Name => "--create-sequences";

		private bool onlyIdleSequence = false;

		private ActorRule[] actorRules;
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Convert an Outpost 2 map to the OpenRA format.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		private ModData modData;
		private PrtFile prtFile;
		private List<uint[]> framePalettes;

		public bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 1;
		}

		private void Run(Utility utility, string[] args)
		{
			const string prtFilename = "op2_art.prt";

			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			using (var stream = Game.ModData.DefaultFileSystem.Open(prtFilename))
			{
				framePalettes = LoadPalettes(stream);
				prtFile = LoadGroups(stream, out var palettes);
			}

			WriteSequences();
			Console.WriteLine($"Wrote sequences file: {OutputFilename}");

			WriteActors();
			Console.WriteLine($"Wrote rules file: {RulesOutputFilename}");
			Console.WriteLine($"Wrote rules example file: {RulesExampleOutputFilename}");
		}

		private List<GroupSequence> LoadGroupSetsFromFile()
		{
			var yamlDefs = MiniYaml.Load(Game.ModData.DefaultFileSystem, new[] { "op2-groups.yaml" }, null);
			var groupSequences = new List<GroupSequence>();
			foreach (var group in yamlDefs)
			{
				var fart = group.Value.Nodes.First(x => x.Key == "ActorType").Value;

				var groupNodes = group.Value.Nodes;
				var groupSequence = new GroupSequence
				{
					Name = group.Key,
					ActorType = (ActorType)Enum.Parse(typeof(ActorType),
						groupNodes.First(x => x.Key == "ActorType").Value.Value.ToString()),
				};

				var createBaseActor = groupNodes.FirstOrDefault(x => x.Key == "CreateBaseActor")?.Value?.Value
					?.ToString().ToLowerInvariant();
				if (!string.IsNullOrWhiteSpace(createBaseActor))
				{
					groupSequence.CreateBaseActor = createBaseActor == "true";
				}

				var createExampleActor = groupNodes.FirstOrDefault(x => x.Key == "CreateExampleActor")?.Value?.Value
					?.ToString().ToLowerInvariant();
				if (!string.IsNullOrWhiteSpace(createExampleActor))
				{
					groupSequence.CreateExampleActor = createExampleActor == "true";
				}

				var withBlankIdle = groupNodes.FirstOrDefault(x => x.Key == "WithBlankIdle")?.Value?.Value
					?.ToString().ToLowerInvariant();
				if (!string.IsNullOrWhiteSpace(withBlankIdle))
				{
					groupSequence.WithBlankIdle = withBlankIdle == "true";
				}

				var withSingleFrameIdle = groupNodes.FirstOrDefault(x => x.Key == "WithSingleFrameIdle")?.Value?.Value
					?.ToString().ToLowerInvariant();
				if (!string.IsNullOrWhiteSpace(withSingleFrameIdle))
				{
					groupSequence.WithSingleFrameIdle = withSingleFrameIdle == "true";
				}

				var groupIndexRemapping = groupNodes.FirstOrDefault(x => x.Key == "GroupIndexRemapping")?.Value?.Value
					?.ToString();
				if (!string.IsNullOrWhiteSpace(groupIndexRemapping))
				{
					var indexStrings = groupIndexRemapping.Split(',');
					var parsedIndices = indexStrings.Select(x => int.Parse(x)).ToArray();
					groupSequence.GroupIndexRemapping = parsedIndices;
				}

				var setsNode = group.Value.Nodes.First(x => x.Key == "Sets").Value.Nodes;
				var sets = new List<GroupSequenceSet>();
				foreach (var set in setsNode)
				{
					var setNodes = set.Value.Nodes;
					var groupSequenceSet = new GroupSequenceSet
					{
						Sequence = set.Value.Value,
						Start = int.Parse(setNodes.First(x => x.Key == "Start").Value.Value.ToString()),
					};

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "Length")?.Value?.Value?.ToString(), out var length))
					{
						groupSequenceSet.Length = length;
					}

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "OffsetX")?.Value?.Value?.ToString(), out var offsetX))
					{
						groupSequenceSet.OffsetX = offsetX;
					}

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "OffsetY")?.Value?.Value?.ToString(), out var offsetY))
					{
						groupSequenceSet.OffsetY = offsetY;
					}

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "OffsetZ")?.Value?.Value?.ToString(), out var offsetZ))
					{
						groupSequenceSet.OffsetZ = offsetZ;
					}

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "StartOffset")?.Value?.Value?.ToString(), out var startOffset))
					{
						groupSequenceSet.StartOffset = startOffset;
					}

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "Tick")?.Value?.Value?.ToString(), out var tick))
					{
						groupSequenceSet.Tick = tick;
					}

					var useFile = setNodes.FirstOrDefault(x => x.Key == "UseFile")?.Value?.Value?.ToString();
					if (!string.IsNullOrEmpty(useFile))
					{
						groupSequenceSet.UseFile = useFile;
					}

					sets.Add(groupSequenceSet);
				}

				groupSequence.Sets = sets.ToArray();
				groupSequences.Add(groupSequence);
			}

			return groupSequences;
		}

		private void WriteSequences()
		{
			// Load from yaml
			var groupSequences = LoadGroupSetsFromFile();

			var sb = new StringBuilder();
			sb.AppendLine("## This file was generated by CreateSequencesCommand.");
			sb.AppendLine("## Don't modify it yourself unless you know what you're doing.");
			sb.AppendLine(string.Empty);

			var newActorRules = new Dictionary<string, ActorRule>();

			foreach (var groupSequence in groupSequences)
			{
				sb.AppendLine($"{groupSequence.Name}:");

				var sets = groupSequence.Sets;
				if (groupSequence.WithSingleFrameIdle)
				{
					sets = AddSingleFrameIdle(sets);
				}

				var literalGroupSequenceSets = new List<GroupSequenceSet>();

				foreach (var groupSequenceSet in sets)
				{
					if (!string.IsNullOrWhiteSpace(groupSequenceSet.UseFile))
					{
						literalGroupSequenceSets.Add(groupSequenceSet);
						continue;
					}

					var typeGroupedFrames = GetTypedGroupFrames(groupSequence, groupSequenceSet, out var frameCount);

					Func<SequenceSet, int, string> getSequenceName = (inGroup, ind) =>
					{
						Func<int, string> getTypeString = (inFrameType) =>
						{
							return inFrameType switch
							{
								1 => "sprite",
								4 => "shadow",
								5 => "shadow",
								_ => "unknown"
							};
						};

						//////////////////////
						var seqName = $"{groupSequenceSet.Sequence}-{getTypeString(inGroup.FrameType)}" + (ind == 0 ? string.Empty : $"-id{ind}");
						if (inGroup.FrameType == 1 && ind == 0)
						{
							seqName = groupSequenceSet.Sequence;
						}

						return seqName;
					};

					var allSequenceNames = typeGroupedFrames.Select((x, index) => getSequenceName(x, index)).ToArray();

					sb.AppendLine($"\t# All sequences: {string.Join(", ", allSequenceNames)}");
					var zIndex = 0;
					var typeIndex = 0;
					foreach (var typeGroupedFrame in typeGroupedFrames)
					{
						var sequenceName = allSequenceNames[typeIndex];
						var outputSequence = GetOutputSequence(sequenceName, groupSequenceSet, typeGroupedFrame);

						if (groupSequence.WithBlankIdle && groupSequenceSet == sets.First())
						{
							sb.AppendLine("\tidle:");
							sb.AppendLine("\t\tLength: 1");
							sb.AppendLine("\t\tFacings: 1");
							sb.AppendLine("\t\tCombine:");
							sb.AppendLine("\t\t\tblank.png: idle-0");
							sb.AppendLine("\t\t\t\tFrames: 0");
							sb.AppendLine("\t\t\t\tLength: 1");
						}

						sb.AppendLine($"\t{sequenceName}:");
						sb.AppendLine($"\t\tLength: {frameCount}");
						sb.AppendLine($"\t\tFacings: {groupSequenceSet.Length}");
						sb.AppendLine($"\t\tOffset: {groupSequenceSet.OffsetX},{groupSequenceSet.OffsetY}");

						if (groupSequenceSet.OffsetZ != 0)
						{
							sb.AppendLine($"\t\tZOffset: {groupSequenceSet.OffsetZ}");
						}
						else
						{
							sb.AppendLine($"\t\tZOffset: {zIndex}");
						}

						if (groupSequenceSet.Tick.HasValue)
						{
							sb.AppendLine($"\t\tTick: {groupSequenceSet.Tick.Value}");
						}

						sb.AppendLine($"\t\tCombine:");

						foreach (var frameset in outputSequence.Framesets)
						{
							var framesString = string.Join(",", frameset.Frames);
							if (frameset.IsBlank)
							{
								sb.AppendLine($"\t\t\tblank.png: {frameset.Name}");
							}
							else
							{
								sb.AppendLine($"\t\t\top2_art.bmp: {frameset.Name}");
							}

							sb.AppendLine($"\t\t\t\tFrames: {framesString}");
							sb.AppendLine($"\t\t\t\tLength: {frameset.Frames.Count}");
							sb.AppendLine($"\t\t\t\tOffset: {frameset.OffsetX},{frameset.OffsetY}");
							sb.AppendLine($"\t\t\t\tZOffset: {zIndex}");
						}

						typeIndex++;
						zIndex -= 256;
					}

					// Also assemble actor rules
					var actorId = 0;
					foreach (var typeGroupedFrame in typeGroupedFrames)
					{
						var overlayPalette = typeGroupedFrame.FrameType == 4 || typeGroupedFrame.FrameType == 5 ? "shadow" : null;
						if (typeGroupedFrame.FrameType == 1)
						{
							// Palette possibilities:
							// 1: Eden building
							// 2: Plymouth building
							// 5: Unknown other
							// 8: Unknown other 2, UI item?
							overlayPalette = $"{typeGroupedFrame.Palette}";
						}

						ActorRule actorRule;
						if (newActorRules.ContainsKey(groupSequence.Name))
						{
							actorRule = newActorRules[groupSequence.Name];
						}
						else
						{
							actorRule = new ActorRule
							{
								Name = groupSequence.Name,
								Palette = overlayPalette,
								ActorType = groupSequence.ActorType,
								CreateActor = groupSequence.CreateBaseActor,
								CreateExampleActor = groupSequence.CreateExampleActor,
							};

							newActorRules.Add(groupSequence.Name, actorRule);
						}

						var sequenceName = getSequenceName(typeGroupedFrame, actorId);
						if (sequenceName != "idle" && sequenceName.StartsWith("idle-"))
						{
							actorRule.Overlays.Add(new ActorOverlay
							{
								SequenceName = sequenceName,
								Palette = overlayPalette
							});
						}

						actorId++;
					}

					if (onlyIdleSequence)
					{
						break;
					}
				}

				// Hack in our literal sequences
				// Used for icons only for now
				foreach (var seq in literalGroupSequenceSets)
				{
					sb.AppendLine($"\t{seq.Sequence}: {seq.UseFile}");
					sb.AppendLine($"\t\tStart: {seq.Start}");
					sb.AppendLine($"\t\tLength: {seq.Length}");
					sb.AppendLine($"\t\tOffset: {seq.OffsetX},{seq.OffsetY}");
				}

				sb.AppendLine(string.Empty);
			}

			actorRules = newActorRules.Values.ToArray();

			using var sw = new StreamWriter(OutputFilename);
			sw.Write(sb.ToString());
		}

		private GroupSequenceSet[] AddSingleFrameIdle(GroupSequenceSet[] groupSequenceSets)
		{
			const string idleString = "^idle[2-9]*$";
			var regex = new Regex(idleString);
			var originalIdle = groupSequenceSets.FirstOrDefault(t => t.Sequence == "idle");
			if (originalIdle == null)
			{
				return groupSequenceSets;
			}

			var newIdle = new GroupSequenceSet()
			{
				Length = originalIdle.Length,
				Sequence = originalIdle.Sequence,
				Start = originalIdle.Start,
				Tick = originalIdle.Tick,
				OffsetX = originalIdle.OffsetX,
				OffsetY = originalIdle.OffsetY,
				OffsetZ = originalIdle.OffsetZ,
				StartOffset = originalIdle.StartOffset,
			};

			var idleSets = groupSequenceSets.Where(t => regex.IsMatch(t.Sequence)).OrderBy(x => x.Sequence);
			var nonIdleSets = groupSequenceSets.Where(x => !idleSets.Contains(x));
			var renumberedIdleSets = idleSets.Select((x, ind) =>
			{
				x.Sequence = "idle" + (ind + 2).ToString();
				return x;
			}).ToList();

			var result = new List<GroupSequenceSet>() { newIdle };
			result.AddRange(renumberedIdleSets);
			result.AddRange(nonIdleSets);
			return result.ToArray();
		}

		/// <summary>
		/// Further specialize output sequences by shared offsets
		/// </summary>
		/// <param name="sequenceName"></param>
		/// <param name="groupSequenceSet"></param>
		/// <param name="typeGroupedFrame"></param>
		/// <returns></returns>
		private OutputSequence GetOutputSequence(string sequenceName, GroupSequenceSet groupSequenceSet, SequenceSet typeGroupedFrame)
		{
			var outputSequence = new OutputSequence()
			{
				Name = sequenceName,
			};

			for (var facingIndex = 0; facingIndex < groupSequenceSet.Length; facingIndex++)
			{
				var framesArray = typeGroupedFrame.GetFacingArray(facingIndex);
				var animationGroup = typeGroupedFrame.AnimationFacings[facingIndex];
				var facingFrameCount = animationGroup.Frames.Length;
				for (var frameIndex = 0; frameIndex < facingFrameCount; frameIndex++)
				{
					var frame = framesArray[frameIndex];

					var isBlank = frame == null;
					var innerSequenceName = $"{groupSequenceSet.Sequence}-facing{facingIndex}-{frameIndex}";

					Func<OutputSequenceFrameset> getNewFrameset = () =>
					{
						var newFrameset = new OutputSequenceFrameset()
						{
							Name = innerSequenceName,
							IsBlank = isBlank,
						};

						if (frame != null)
						{
							newFrameset.OffsetX = frame.OffsetX;
							newFrameset.OffsetY = frame.OffsetY;
							newFrameset.Frames.Add(frame.Frame);
						}
						else
						{
							newFrameset.Frames.Add(0);
						}

						return newFrameset;
					};

					var outputFrames = outputSequence.Framesets;
					var lastFrame = outputFrames.LastOrDefault();
					if (lastFrame == null)
					{
						outputSequence.Framesets.Add(getNewFrameset());
					}
					else
					{
						if ((isBlank && lastFrame.IsBlank) ||
						    (!isBlank && !lastFrame.IsBlank && lastFrame.OffsetX == frame.OffsetX && lastFrame.OffsetY == frame.OffsetY))
						{
							// It's a match, add our frame
							lastFrame.Frames.Add(isBlank ? 0 : frame.Frame);
						}
						else
						{
							// It's not a match; add a new one
							outputSequence.Framesets.Add(getNewFrameset());
						}
					}
				}
			}

			return outputSequence;
		}

		/// <summary>
		/// The frame types of each picture for each facing might not be in the same order for all facings, nor the same number of pictures for each frame type per facing.
		/// Create "buckets" for each frame type and deposit each picture into each bucket, expanding the number of buckets as necessary.
		/// </summary>
		/// <param name="groupSequence"></param>
		/// <param name="groupSequenceSet"></param>
		/// <param name="frameCount"></param>
		/// <returns></returns>
		private List<SequenceSet> GetTypedGroupFrames(GroupSequence groupSequence, GroupSequenceSet groupSequenceSet, out int frameCount)
		{
			var typeGroupedFrames = new List<SequenceSet>();

			var groups = prtFile.Groups
					.Skip(groupSequenceSet.Start)
					.Take(groupSequenceSet.Length)
					.ToList();

			if (groupSequence.GroupIndexRemapping != null)
			{
				var numGroups = groups.Count;
				var newArray = new ImageGroup[numGroups];

				for (var groupIndex = 0; groupIndex < numGroups; groupIndex++)
				{
					var remappedIndex = groupSequence.GroupIndexRemapping[groupIndex];
					var targetGroup = groups[remappedIndex];
					newArray[groupIndex] = targetGroup;
				}

				groups = newArray.ToList();
			}

			if (groupSequenceSet.StartOffset > 0)
			{
				var newGroups = groups
					.Skip(groupSequenceSet.StartOffset)
					.Take(groupSequenceSet.Length - groupSequenceSet.StartOffset)
					.ToList();

				newGroups.AddRange(groups
					.Take(groupSequenceSet.StartOffset));

				groups = newGroups;
			}

			frameCount = groups.Max(x => x.FrameCount);

			if (groupSequenceSet.Sequence == "idle" && groupSequence.WithSingleFrameIdle)
			{
				frameCount = 1;
			}

			for (var frameIndex = 0; frameIndex < frameCount; frameIndex++)
			{
				var groupIndex = 0;
				foreach (var group in groups)
				{
					Op2Frame frame;
					if (frameIndex >= group.Frames.Length)
					{
						// Hack it - just repeat the first one
						frame = group.Frames[0];
					}
					else
					{
						frame = group.Frames[frameIndex];
					}

					var pics = frame.Pictures.OrderBy(x => x.PicOrder);
					foreach (var pic in pics)
					{
						var rawFrame = prtFile.ImageHeader[pic.ImgNumber];
						var halfWidth = (int)Math.Ceiling(rawFrame.PaddedWidth / 2.0);
						var halfHeight = (int)Math.Ceiling(rawFrame.Height / 2.0);
						var relX = (halfWidth - group.CenterX) + pic.PosX;
						var relY = (halfHeight - group.CenterY) + pic.PosY;

						var matchingSequenceFrames = typeGroupedFrames
							.Where(x => x.FrameType == rawFrame.ImageType &&
							            x.Palette == rawFrame.Palette)
							.ToList();

						var setFrame = false;
						foreach (var seqFrame in matchingSequenceFrames)
						{
							if (seqFrame.AnimationFacings[groupIndex].Frames[frameIndex] == null)
							{
								var animation = new AnimationFrame()
								{
									Frame = pic.ImgNumber,
									OffsetX = relX,
									OffsetY = relY,
								};

								var ourGroup = seqFrame.AnimationFacings[groupIndex];
								ourGroup.Frames[frameIndex] = animation;
								setFrame = true;
								break;
							}
						}

						if (!setFrame)
						{
							// Need a new frameset for this image type and palette
							var seqFrame = new SequenceSet
							{
								Name = groupSequenceSet.Sequence,
								AnimationFacings = new AnimationFacing[groupSequenceSet.Length],
								FrameType = rawFrame.ImageType,
								Palette = rawFrame.Palette,
								PicOrder = pic.PicOrder,
								OffsetX = groupSequenceSet.OffsetX,
								OffsetY = groupSequenceSet.OffsetY,
							};

							for (var i = 0; i < groupSequenceSet.Length; i++)
							{
								seqFrame.AnimationFacings[i] = new AnimationFacing
								{
									Frames = new AnimationFrame[frameCount],
								};
							}

							// Populate it too
							var animation = new AnimationFrame()
							{
								Frame = pic.ImgNumber,
								OffsetX = relX,
								OffsetY = relY,
							};

							var ourGroup = seqFrame.AnimationFacings[groupIndex];
							ourGroup.Frames[frameIndex] = animation;

							typeGroupedFrames.Add(seqFrame);
						}
					}

					groupIndex++;
				}
			}

			typeGroupedFrames = typeGroupedFrames.OrderByDescending(x => x.PicOrder).ToList();

			// Put the non-shadow frames before the shadow frames
			var shadowFrames =
				typeGroupedFrames.Where(x => x.FrameType == 4 || x.FrameType == 5);
			var nonShadowFrames =
				typeGroupedFrames.Where(x => x.FrameType != 4 && x.FrameType != 5);

			typeGroupedFrames = nonShadowFrames.ToList();
			typeGroupedFrames.AddRange(shadowFrames);
			return typeGroupedFrames;
		}

		private void WriteActors()
		{
			var sb = new StringBuilder();
			sb.AppendLine("## This file was generated by CreateSequencesCommand.");
			sb.AppendLine("## Don't modify it yourself unless you know what you're doing.");
			sb.AppendLine(string.Empty);

			foreach (var actorRule in actorRules)
			{
				if (!actorRule.CreateActor)
				{
					continue;
				}

				sb.AppendLine($"^{actorRule.Name}-generated:");
				sb.AppendLine($"\tRenderSprites:");
				sb.AppendLine($"\t\tImage: {actorRule.Name}");
				var isPlayerPalette = int.TryParse(actorRule.Palette, out var actorPalette);
				var playerPalettes = new[] { 1, 2, 3, 5, 6, 7, 8 };
				if (playerPalettes.Contains(actorPalette))
				{
					// player color mapped palettes
					sb.AppendLine($"\t\tPlayerPalette: playerMapped{actorRule.Palette}");
				}
				else
				{
					// shadow palette
					sb.AppendLine($"\t\tPalette: {actorRule.Palette}");
				}

				var overlayIndex = 0;
				foreach (var overlay in actorRule.Overlays)
				{
					sb.AppendLine($"\tWithDirectionalIdleOverlay@{overlayIndex}:");
					sb.AppendLine($"\t\tSequence: {overlay.SequenceName}");
					if (!string.IsNullOrWhiteSpace(overlay.Palette))
					{
						var isOverlayPlayerPalette = int.TryParse(overlay.Palette, out var overlayPalette);
						if (playerPalettes.Contains(overlayPalette))
						{
							sb.AppendLine($"\t\tPalette: playerMapped{overlay.Palette}");
							sb.AppendLine($"\t\tIsPlayerPalette: true");
						}
						else
						{
							sb.AppendLine($"\t\tPalette: {overlay.Palette}");
						}
					}

					overlayIndex++;
				}

				sb.AppendLine(string.Empty);
			}

			using var sw = new StreamWriter(RulesOutputFilename);
			sw.Write(sb.ToString());

			const string placeholderHeader = @"## This file was generated by CreateSequencesCommand.
## If you want to modify one of these actors, take it out of this file and put it into the appropriate
## vehicles.yaml or structures.yaml file. Then make it inherit from your appropriate class like ^Op2Vehicle or ^Op2Building.
## instead of ^StubVehicle and ^StubBuilding, below.

^StubVehicle:
	Inherits: ^Vehicle
	Tooltip:
		Name: Example Vehicle
	Selectable:
		Class: E1
	Voiced:
		VoiceSet: GenericVoice
	Valued:
		Cost: 150
	Health:
		HP: 100
	RevealsShroud:
		Range: 8c0
	Mobile:
		Speed: 78

^StubBuilding:
	Inherits: ^Building
	MustBeDestroyed:
	Tooltip:
		Name: Example Building
	Selectable:
		Class: B1
	Valued:
		Cost: 150
	Health:
		HP: 100
	RevealsShroud:
		Range: 8c0
	-Capturable:
	-Sellable:
	-CaptureManager:
	-CapturableProgressBar:
	-CapturableProgressBlink:
	-Demolishable:

## Generated OP2 actors

";

			// Now write placeholder actors file
			sb = new StringBuilder();
			sb.Append(placeholderHeader);

			foreach (var actorRule in actorRules)
			{
				if (!actorRule.CreateExampleActor || !actorRule.CreateActor)
					continue;

				var inheritor = "^StubVehicle";
				if (actorRule.ActorType == ActorType.Building)
				{
					inheritor = "^StubBuilding";
				}

				sb.AppendLine($"{actorRule.Name}:");
				sb.AppendLine($"\tInherits@generated: ^{actorRule.Name}-generated");
				sb.AppendLine($"\tInherits: {inheritor}");
				sb.AppendLine(string.Empty);
			}

			using var sw2 = new StreamWriter(RulesExampleOutputFilename);
			sw2.Write(sb.ToString());
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
	}
}
