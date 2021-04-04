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
using OpenRA.FileSystem;

namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	class ImportOp2MapCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--import-op2-map";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Convert an Outpost 2 map to the OpenRA format.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		private ModData modData;
		private Map map;
		private MapPlayers mapPlayers;

		public bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 2;
		}

		private void Run(Utility utility, string[] args)
		{
			// HACK: The engine code assumes that Game.modData is set.
			Game.ModData = modData = utility.ModData;

			var filename = args[1];
			if (!Game.ModData.DefaultFileSystem.Exists(filename))
			{
				throw new IOException($"Couldn't find map {filename} in maps.vol or filesystem.");
			}

			using (var stream = Game.ModData.DefaultFileSystem.Open(filename))
			{
				var tag = stream.ReadUInt32(); // always 4113
				var unknown = stream.ReadUInt32(); // always 0
				var lgTileWidth = stream.ReadInt32();
				var tileHeight = stream.ReadUInt32(); // always 64
				var numTileSets = stream.ReadUInt32(); // always 512

				// Update map header fields
				// Round height up to nearest power of 2
				var newHeight = tileHeight;
				newHeight -= 1;
				newHeight |= (newHeight >> 1);
				newHeight |= (newHeight >> 2);
				newHeight |= (newHeight >> 4);
				newHeight |= (newHeight >> 8);
				newHeight |= (newHeight >> 16);
				newHeight++;
				var width = 1 << lgTileWidth; // Calculate map width
				var height = newHeight;

				map = new Map(modData, modData.DefaultTileSets["default"], width + 2, (int)height + 2)
				{
					Title = Path.GetFileNameWithoutExtension(filename),
					Author = "OpenOP2",
					RequiresMod = modData.Manifest.Id,
				};

				SetBounds(map, (int)width + 2, (int)height + 2);
				for (var y = 0; y < height; y++)
				{
					for (var x = 0; x < width; x++)
					{
						// All these tile ID's will be completely different to ours - this importer will be broken until there's a mapping
						// between the OP2 tile IDs and OpenOP2 tile IDs.
						var tileType = stream.ReadUInt8();
						var byte2 = stream.ReadUInt8(); // Elevation or something
						var byte3 = stream.ReadUInt8();
						var byte4 = stream.ReadUInt8();
						map.Tiles[new CPos(x + 1, y + 1)] = new TerrainTile((ushort)tileType, 0);
					}
				}

				// TODO: Read all the rest of the actors and whatnot
			}

			mapPlayers = new MapPlayers(map.Rules, 0);
			map.PlayerDefinitions = mapPlayers.ToMiniYaml();
			map.FixOpenAreas();

			var dest = Path.GetFileNameWithoutExtension(args[1]) + ".oramap";
			var mapLocations = Game.ModData.Manifest.MapFolders;
			var userMapPath = mapLocations.First(mapLocation => mapLocation.Value == "User");
			var targetPath = Path.Combine(Platform.ResolvePath(userMapPath.Key.Substring(1)), dest);
			map.Save(ZipFileLoader.Create(targetPath));

			Console.WriteLine(targetPath + " saved.");
		}

		static void SetBounds(Map map, int width, int height)
		{
			var tl = new PPos(1, 1);
			var br = new PPos(0 + width - 2, 0 + height - 2);
			map.SetBounds(tl, br);
		}
	}
}
