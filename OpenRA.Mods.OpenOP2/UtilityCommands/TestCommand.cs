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
		private const string OutputFilename = "..\\..\\mods\\openop2\\test.txt";

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

			const string mapDir = "..\\..\\mods\\openop2\\maps-import";
			foreach (var file in Directory.EnumerateFiles(mapDir, "*.map"))
			{
				sb.AppendLine(
					$"call OpenRA.Utility.exe %MOD_ID% --import-op2-map {Path.GetFileName(file)}");
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
