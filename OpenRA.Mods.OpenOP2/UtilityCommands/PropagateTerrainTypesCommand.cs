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
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using OpenRA.FileSystem;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class PropagateTerrainTypesCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--propagate-terrain-types";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Copies the tile terrain types from the grouped sets into the raw tile definitions used by imported maps.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		private ModData modData;

		public bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 2;
		}

		private void Run(Utility utility, string[] args)
		{
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			const string tilesetsPath = "..\\..\\mods\\openop2\\tilesets";
			var filename = args[1];

			var fullFilepath = Path.Combine(tilesetsPath, filename);

			var sb = new StringBuilder();
			var dict = new Dictionary<string, Dictionary<int, string>>();
			using (var stream = File.OpenRead(fullFilepath))
			{
				var lines = stream.ReadAllLines().ToArray();

				const string framesTemplate = "\t\tFrames: ";
				Func<string, int[]> extractFrames = (ln) =>
				{
					var restOfLine = ln.Replace(framesTemplate, string.Empty);
					var frameStrArray = restOfLine.Split(new char[] { ',' });
					var frames = frameStrArray.Select(x => int.Parse(x.Trim())).ToArray();
					return frames;
				};

				const string idTemplate = "\t\tId: ";
				const string imagesTemplate = "\t\tImages: ";
				const string tileTerrainRegex = @"^\s\s\s(0?\d|\d\d):\s";
				var currentImage = string.Empty;
				var lineIndex = 0;
				foreach (var line in lines)
				{
					var newLine = line;
					if (line.StartsWith(idTemplate))
					{
						var restOfLine = line.Replace(idTemplate, string.Empty);
						if (int.TryParse(restOfLine, out var id))
						{
							// Break out when we reach the generated tiles
							if (id == 0)
							{
								break;
							}
						}
					}
					else if (line.StartsWith(imagesTemplate))
					{
						currentImage = line.Replace(imagesTemplate, string.Empty).Trim();
					}
					else if (line.StartsWith(framesTemplate))
					{
						var frames = extractFrames(line);

						if (!dict.ContainsKey(currentImage))
						{
							dict.Add(currentImage, new Dictionary<int, string>());
						}

						// Read ahead the tile definitions
						var frameCount = frames.Length;
						var tileLines = lines
							.Skip(lineIndex + 2)
							.Take(frameCount)
							.ToArray();

						var tileNames = tileLines
							.Select(x => Regex.Replace(x, tileTerrainRegex, string.Empty).Trim())
							.ToArray();

						var currentDictionary = dict[currentImage];
						var frameIndex = 0;
						foreach (var frame in frames)
						{
							if (!currentDictionary.ContainsKey(frame))
							{
								currentDictionary.Add(frame, tileNames[frameIndex]);
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
					if (line.StartsWith(imagesTemplate))
					{
						currentImage = line.Replace(imagesTemplate, string.Empty).Trim();
						tileDefs.Clear();
					}
					else if (line.StartsWith(framesTemplate))
					{
						var frames = extractFrames(line);

						if (dict.ContainsKey(currentImage))
						{
							var currentDictionary = dict[currentImage];
							var frameIndex = 0;
							foreach (var frame in frames)
							{
								if (currentDictionary.ContainsKey(frame))
								{
									// Found a replacement
									var terrainType = currentDictionary[frame];
									tileDefs.Add(frameIndex, terrainType);
								}

								frameIndex++;
							}
						}
					}
					else if (Regex.IsMatch(line, tileTerrainRegex))
					{
						var extractedPrefix = Regex.Match(line, tileTerrainRegex).Value;
						var restOfLine = line.Replace(extractedPrefix, string.Empty);
						var tileIndex = int.Parse(restOfLine);
						if (tileDefs.ContainsKey(tileIndex))
						{
							var tileDef = tileDefs[tileIndex];
							newLine = extractedPrefix + tileIndex + ": " + tileDef; // \t\t\t0: ClearMud
						}
					}

					sb.AppendLine(newLine);
					lineIndex++;
				}
			}

			var filenameOnly = Path.GetFileNameWithoutExtension(filename);
			var destFile = Path.Combine(tilesetsPath, $"{filenameOnly}-propagated.yaml");
			try
			{
				using var sw = new StreamWriter(destFile);
				sw.Write(sb.ToString());
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
