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
using System.Text;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	sealed class TestingCommand : IUtilityCommand
	{
		const string OutputFilename = "..\\..\\mods\\openop2\\op2-groups.yaml";

		string IUtilityCommand.Name => "--testing";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Just for debug and test.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility); }

		public static bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 1;
		}

		static void Run(Utility utility)
		{
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = utility.ModData;

			var sb = new StringBuilder();

			try
			{
				using (var sw = new StreamWriter(OutputFilename))
				{
					sw.Write(sb.ToString());
				}
			}
			catch (Exception)
			{
				// Console.WriteLine("Couldn't write destination file.", ex);
				throw;
			}

			Console.WriteLine($"Wrote file: {OutputFilename}");
		}
	}
}
