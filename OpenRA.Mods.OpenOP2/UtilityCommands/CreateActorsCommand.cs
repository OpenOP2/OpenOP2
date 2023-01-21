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
using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Mods.OpenOP2.SpriteLoaders;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class CreateActorsCommand : IUtilityCommand
	{
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

		private const string RulesOutputFilename = "..\\mods\\openop2\\rules\\rules-generated.yaml";
		private const string RulesExampleOutputFilename = "..\\mods\\openop2\\rules\\rules-example.yaml";

		string IUtilityCommand.Name => "--create-actors";

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
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			var prt = Prt.Instance;
			prtFile = prt.PrtFile;
			framePalettes = prt.FramePalettes;

			WriteSequences();
			WriteActors();
			Console.WriteLine($"Wrote rules file: {RulesOutputFilename}");
			Console.WriteLine($"Wrote rules example file: {RulesExampleOutputFilename}");
		}

		private void WriteSequences()
		{
			// Load from yaml
			var groupsFile = new GroupsFile();
			var groupSequences = groupsFile.Groups;

			var newActorRules = new Dictionary<string, ActorRule>();

			foreach (var groupSequence in groupSequences)
			{
				var sets = groupSequence.Sets;
				if (groupSequence.WithSingleFrameIdle)
				{
					sets = GroupsFile.AddSingleFrameIdle(sets);
				}

				foreach (var groupSequenceSet in sets)
				{
					var typeGroupedFrames = GroupsFile.GetTypedGroupFrames(prtFile, groupSequence, groupSequenceSet, out var frameCount);

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
						var seqName = $"{groupSequenceSet.Sequence}-{getTypeString(inGroup.FrameType)}" + (ind == 0 ? string.Empty : $"-id{ind}");
						if (inGroup.FrameType == 1 && ind == 0)
						{
							seqName = groupSequenceSet.Sequence;
						}

						return seqName;
					};

					// assemble actor rules
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
			}

			actorRules = newActorRules.Values.ToArray();
		}

		private void WriteActors()
		{
			var sb = new StringBuilder();
			sb.AppendLine("## This file was generated by CreateActorsCommand.");
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

			using (var sw = new StreamWriter(RulesOutputFilename))
			{
				sw.Write(sb.ToString());

				const string placeholderHeader = @"## This file was generated by CreateActorsCommand.
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
			}

			using (var sw2 = new StreamWriter(RulesExampleOutputFilename))
			{
				sw2.Write(sb.ToString());
			}
		}
	}
}
