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
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using OpenRA.FileSystem;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class RenumberTilesetsCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--renumber-tileset";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "START", "Resequences a tileset yaml file from a certain number onward.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		private ModData modData;

		public bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 3;
		}

		private void Run(Utility utility, string[] args)
		{
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			const string tilesetsPath = "..\\..\\mods\\openop2\\tilesets";
			var filename = args[1];
			var startIndexStr = args[2];
			if (!int.TryParse(startIndexStr, out var startIndex))
				throw new IOException($"'{startIndexStr}' wasn't a valid number.");

			var fullFilepath = Path.Combine(tilesetsPath, filename);

			var sb = new StringBuilder();
			using (var stream = File.OpenRead(fullFilepath))
			{
				var lines = stream.ReadAllLines().ToArray();
				const string templateTemplate = "\tTemplate@";
				const string idTemplate = "\t\tId: ";
				var templateIndex = startIndex;
				foreach (var line in lines)
				{
					var newLine = line;
					if (line.StartsWith(templateTemplate))
					{
						var restOfLine = line.Replace(templateTemplate, string.Empty);
						restOfLine = restOfLine.Substring(0, restOfLine.Length - 1); // trim :

						if (int.TryParse(restOfLine, out var templateNumber))
						{
							newLine = templateTemplate + templateIndex + ":";
						}
					}
					else if (line.StartsWith(idTemplate))
					{
						newLine = idTemplate + templateIndex;
						templateIndex++;
					}

					sb.AppendLine(newLine);
				}
			}

			var filenameOnly = Path.GetFileNameWithoutExtension(filename);
			var destFile = Path.Combine(tilesetsPath, $"{filenameOnly}-renumbered.yaml");
			try
			{
				using (var sw = new StreamWriter(destFile))
				{
					sw.Write(sb.ToString());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Couldn't write destination filename.", ex);
				throw;
			}

			Console.WriteLine(destFile + " saved.");
		}
	}
}
