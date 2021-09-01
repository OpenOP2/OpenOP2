using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Mods.OpenOP2.UtilityCommands;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	public class GroupsFile
	{
		public List<GroupSequence> Groups { get; private set; }

		public GroupsFile()
		{
			var yamlDefs = MiniYaml.Load(Game.ModData.DefaultFileSystem, new[] { "op2-groups.yaml" }, null);
			var groupSequences = new List<GroupSequence>();
			foreach (var group in yamlDefs)
			{
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

					if (int.TryParse(setNodes.FirstOrDefault(x => x.Key == "FacingsOverride")?.Value?.Value?.ToString(), out var facingsOverride))
					{
						groupSequenceSet.FacingsOverride = facingsOverride;
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

			Groups = groupSequences;
		}

		/// <summary>
		/// The frame types of each picture for each facing might not be in the same order for all facings, nor the same number of pictures for each frame type per facing.
		/// Create "buckets" for each frame type and deposit each picture into each bucket, expanding the number of buckets as necessary.
		/// </summary>
		/// <param name="prtFile">The PRT file</param>
		/// <param name="groupSequence">The group sequence</param>
		/// <param name="groupSequenceSet">The group sequence set</param>
		/// <param name="frameCount">The output frame count</param>
		/// <returns>The list of sequence sets</returns>
		public List<SequenceSet> GetTypedGroupFrames(PrtFile prtFile, GroupSequence groupSequence, GroupSequenceSet groupSequenceSet, out int frameCount)
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
	}
}
