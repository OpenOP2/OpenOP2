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
using OpenRA.Mods.OpenOP2.FileSystem;
using OpenRA.Primitives;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class TestingCommand : IUtilityCommand
	{
		private const string OutputFilename = "..\\..\\mods\\openop2\\op2-groups.yaml";

		string IUtilityCommand.Name => "--testing";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Just for debug and test.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		private ModData modData;

		public bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 1;
		}

		private void Run(Utility utility, string[] args)
		{
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			var sb = new StringBuilder();

			foreach (var group in SequencesList.GroupSequenceSets)
			{
				sb.AppendLine($"{group.Name}:");
				sb.AppendLine($"\tActorType: {group.ActorType}");
				sb.AppendLine($"\tCreateActor: {group.CreateBaseActor}");
				sb.AppendLine($"\tCreateExampleActor: {group.CreateExampleActor}");
				sb.AppendLine("\tSets:");
				foreach (var set in group.Sets)
				{
					sb.AppendLine($"\t\tSequence: {set.Sequence}");
					sb.AppendLine($"\t\t\tStart: {set.Start}");
					sb.AppendLine($"\t\t\tStartOffset: {set.StartOffset}");
					sb.AppendLine($"\t\t\tLength: {set.Length}");
					sb.AppendLine($"\t\t\tOffsetX: {set.OffsetX}");
					sb.AppendLine($"\t\t\tOffsetY: {set.OffsetY}");
					sb.AppendLine($"\t\t\tOffsetZ: {set.OffsetZ}");
				}

				sb.AppendLine(string.Empty);
			}

			try
			{
				using var sw = new StreamWriter(OutputFilename);
				sw.Write(sb.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine("Couldn't write destination file.", ex);
				throw;
			}

			Console.WriteLine($"Wrote file: {OutputFilename}");
		}
	}
}
