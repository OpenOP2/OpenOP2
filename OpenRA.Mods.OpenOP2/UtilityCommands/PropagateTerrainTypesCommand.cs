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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	sealed class PropagateTerrainTypesCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--propagate-terrain-types";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Copies the tile terrain types from the grouped sets into the raw tile definitions used by imported maps.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		public static bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 2;
		}

		static void Run(Utility utility, string[] args)
		{
			const string TilesetsPath = "..\\..\\mods\\openop2\\tilesets";

			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = utility.ModData;
			var filename = args[1];

			var fullFilepath = Path.Combine(TilesetsPath, filename);

			var sb = new StringBuilder();
			var dict = new Dictionary<string, Dictionary<int, string>>();
			using (var stream = File.OpenRead(fullFilepath))
			{
				var lines = stream.ReadAllLines().ToArray();

				const string FramesTemplate = "\t\tFrames: ";
				static int[] ExtractFrames(string ln)
				{
					var restOfLine = ln.Replace(FramesTemplate, string.Empty);
					var frameStrArray = restOfLine.Split(new char[] { ',' });
					return frameStrArray.Select(x => int.Parse(x.Trim(), NumberStyles.Integer, NumberFormatInfo.InvariantInfo)).ToArray();
				}

				const string IdTemplate = "\t\tId: ";
				const string ImagesTemplate = "\t\tImages: ";
				const string TilesTemplate = "\t\tTiles:";
				const string TileTerrainRegex = @"^\s\s\s(0?\d|\d\d):\s";
				var currentImage = string.Empty;
				var lineIndex = 0;
				var frames = Array.Empty<int>();
				foreach (var line in lines)
				{
					var newLine = line;
					if (line.StartsWith(IdTemplate, StringComparison.InvariantCulture))
					{
						var restOfLine = line.Replace(IdTemplate, string.Empty);
						if (int.TryParse(restOfLine, out var id))
						{
							// Break out when we reach the generated tiles
							if (id == 0)
							{
								break;
							}
						}
					}
					else if (line.StartsWith(ImagesTemplate, StringComparison.InvariantCulture))
					{
						currentImage = line.Replace(ImagesTemplate, string.Empty).Trim();
					}
					else if (line.StartsWith(FramesTemplate, StringComparison.InvariantCulture))
					{
						frames = ExtractFrames(line);

						if (!dict.ContainsKey(currentImage))
						{
							dict.Add(currentImage, new Dictionary<int, string>());
						}
					}
					else if (line.StartsWith(TilesTemplate, StringComparison.InvariantCulture))
					{
						// Read ahead the tile definitions
						var frameCount = frames.Length;
						var tileLines = lines
							.Skip(lineIndex + 1)
							.Take(frameCount)
							.ToArray();

						var tileNames = tileLines
							.Select(x => Regex.Replace(x, TileTerrainRegex, string.Empty).Trim())
							.ToArray();

						var currentDictionary = dict[currentImage];
						var frameIndex = 0;
						foreach (var frameId in frames)
						{
							if (!currentDictionary.ContainsKey(frameId))
							{
								currentDictionary.Add(frameId, tileNames[frameIndex]);
							}
							else
							{
								// Another tile is using this frame!
								throw new Exception($"A frame was re-used on line {lineIndex}.");
							}

							frameIndex++;
						}
					}

					sb.AppendLine(newLine);
					lineIndex++;
				}

				// Now continue through the rest of the lines and replace any we match from our dictionary
				var restOfLines = lines.Skip(lineIndex).ToArray();
				var tileDefs = new Dictionary<int, string>();
				foreach (var line in restOfLines)
				{
					var newLine = line;
					if (line.StartsWith(ImagesTemplate, StringComparison.InvariantCulture))
					{
						currentImage = line.Replace(ImagesTemplate, string.Empty).Trim();
						tileDefs.Clear();
					}
					else if (line.StartsWith(FramesTemplate, StringComparison.InvariantCulture))
					{
						frames = ExtractFrames(line);

						if (dict.TryGetValue(currentImage, out var value))
						{
							var currentDictionary = value;
							var frameIndex = 0;
							foreach (var frameId in frames)
							{
								if (currentDictionary.TryGetValue(frameId, out var value2))
								{
									// Found a replacement
									var terrainType = value2;
									tileDefs.Add(frameIndex, terrainType);
								}

								frameIndex++;
							}
						}
					}
					else if (Regex.IsMatch(line, TileTerrainRegex))
					{
						var extractedPrefix = Regex.Match(line, TileTerrainRegex).Value;
						var restOfLine = line.Replace(extractedPrefix, string.Empty);
						var digitNotOk = int.TryParse(restOfLine, out var tileIndex);
						if (digitNotOk)
						{
							throw new Exception($"Couldn't parse digit: {restOfLine}");
						}

						if (tileDefs.TryGetValue(tileIndex, out var value))
						{
							var tileDef = value;
							if (restOfLine != tileDef)
							{
								newLine = extractedPrefix + tileDef; // \t\t\t0: ClearMud
							}
						}
					}

					sb.AppendLine(newLine);
					lineIndex++;
				}
			}

			var filenameOnly = Path.GetFileNameWithoutExtension(filename);
			var destFile = Path.Combine(TilesetsPath, $"{filenameOnly}-propagated.yaml");
			try
			{
				using (var sw = new StreamWriter(destFile))
				{
					sw.Write(sb.ToString());
				}
			}
			catch (Exception)
			{
				// Console.WriteLine("Couldn't write destination filename.", ex);
				throw;
			}

			Console.WriteLine(destFile + " saved.");
		}
	}
}
