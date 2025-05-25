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
	sealed class ImportOp2MapCommand : IUtilityCommand
	{
		sealed class ClipRect
		{
			public int X1;
			public int X2;
			public int Y1;
			public int Y2;
		}

		sealed class TileSetInfo
		{
			public int NumTiles;                // Number of tiles in this tile set
			public string TileSetName;          // File name of the tile set, as Outpost2 sees it
		}

		sealed class TileSetMapping
		{
			public short TileSetIndex;
			public short TileIndex;
			public short NumTileReplacements;
			public short CycleDelay;
		}

		sealed class TerrainType
		{
			// public short FirstTile;		// First tile index in this terrain type class
			// public short LastTile;			// Last tile index in this terrain type class
			// public short Bulldozed;		// Index of the bulldozed tile
			// public short CommonRubble;		// Index of the common rubble tile (4 common rubble tiles, followed by 4 rare rubble tiles)
			// public short[] TubeUnknown;	// (6) 12
			//
			// // public TerrainTypeItemTable Wall[5];	// Wall groups 16 shorts (160)
			// public short Lava;
			// public short Flat1;
			// public short Flat2;
			// public short Flat3;
			//
			// // public TerrainTypeItemTable Tube;		// Tube group 32
			// public short Scorched;			// Scorched tile index (from vehicle explosion) 214 total
			// public short[] Unknown;	// 21 (266 total)
		}

		string IUtilityCommand.Name => "--import-op2-map";
		bool IUtilityCommand.ValidateArguments(string[] args) { return ValidateArguments(args); }

		[Desc("FILENAME", "Convert an Outpost 2 map to the OpenRA format.")]
		void IUtilityCommand.Run(Utility utility, string[] args) { Run(utility, args); }

		ModData modData;
		Map map;
		MapPlayers mapPlayers;

		public static bool ValidateArguments(IReadOnlyCollection<string> args)
		{
			return args.Count >= 2;
		}

		void Run(Utility utility, string[] args)
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
				var tileHeight = stream.ReadInt32(); // always 64
				var numTileSets = stream.ReadInt32(); // always 512

				// Update map header fields
				// Round height up to nearest power of 2
				var newHeight = tileHeight - 1;
				newHeight |= newHeight >> 1;
				newHeight |= newHeight >> 2;
				newHeight |= newHeight >> 4;
				newHeight |= newHeight >> 8;
				newHeight |= newHeight >> 16;
				newHeight++;
				var width = 1 << lgTileWidth; // Calculate map width
				var height = newHeight;

				map = new Map(modData, modData.DefaultTerrainInfo["default"], width + 2, height + 2)
				{
					Title = Path.GetFileNameWithoutExtension(filename),
					Author = "OpenOP2",
					RequiresMod = modData.Manifest.Id,
				};

				var tiles = new List<int>();
				for (var i = 0; i < width * height; i++)
				{
					var tile = stream.ReadInt32();
					tiles.Add(tile);
				}

				var clipRect = new ClipRect()
				{
					X1 = stream.ReadInt32(),
					Y1 = stream.ReadInt32(),
					X2 = stream.ReadInt32(),
					Y2 = stream.ReadInt32(),
				};

				SetBounds(map, width, height);

				// map.SetBounds(new PPos(clipRect.X1, clipRect.Y1), new PPos(width - clipRect.X2, (int)height - clipRect.Y2));
				// map.SetBounds(new PPos(clipRect.X1, clipRect.Y1), new PPos(clipRect.X1 + clipRect.X2, clipRect.Y1 - clipRect.Y2));
				// Read in tileset mappings
				var tileIds = new List<TileSetInfo>();
				var tilesetStartIndices = new List<uint>();
				var tilesetTileIndex = 0;
				for (var i = 0; i < numTileSets; i++)
				{
					var stringLength = stream.ReadInt32();
					if (stringLength <= 0) continue;

					var tilesetMapping = new TileSetInfo()
					{
						TileSetName = stream.ReadASCII(stringLength),
						NumTiles = stream.ReadInt32(),
					};

					tilesetStartIndices.Add((uint)tilesetTileIndex);
					tilesetTileIndex += tilesetMapping.NumTiles;

					tileIds.Add(tilesetMapping);
				}

				var testString = stream.ReadASCII(10);
				if (!testString.StartsWith("TILE SET", StringComparison.InvariantCulture))
				{
					throw new IOException("Couldn't find TILE SET tag.");
				}

				var numMappings = stream.ReadInt32();
				var mappings = new TileSetMapping[numMappings];
				for (var i = 0; i < numMappings; i++)
				{
					mappings[i] = new TileSetMapping
					{
						TileSetIndex = stream.ReadInt16(),
						TileIndex = stream.ReadInt16(),
						NumTileReplacements = stream.ReadInt16(),
						CycleDelay = stream.ReadInt16(),
					};
				}

				var numTerrainTypes = stream.ReadInt32();
				var terrains = new TerrainType[numTerrainTypes];

				stream.Seek(numTerrainTypes * 264, SeekOrigin.Current);

				var checkTag = stream.ReadUInt32();
				if (checkTag != tag)
				{
					throw new IOException("Format error: Tag did not match header tag.");
				}

				var checkTag2 = stream.ReadInt32(); // 4113 - the same all the time?
				var numActors = stream.ReadInt32(); // 218

				// TODO: The rest of the tiles
				// Actually place the tiles
				for (var y = 0; y < height; y++)
				{
					for (var x = 0; x < width; x++)
					{
						var tileXUpper = x >> 5;
						var tileXLower = x & 0x1F;
						var tileOffset = ((tileXUpper * height + y) << 5) + tileXLower;
						var tile = tiles[tileOffset];

						// Get the tile mapping index
						var cellType = tile & 0x1F;
						var tileMappingIndex = (tile & 0xFFE0) >> 5;
						var actorMappingIndex = (tile & 0x7FF00000) >> 11;
						var lava = (tile & 0x00000001) >> 27;
						var lavaPossible = (tile & 0x00000001) >> 28;
						var expand = (tile & 0x00000001) >> 29;
						var microbe = (tile & 0x00000001) >> 30;
						var wallOrBuilding = (tile & 0x00000001) >> 31;
						if (actorMappingIndex != 0 || lavaPossible != 0 || wallOrBuilding != 0)
						{
							throw new Exception("Actor mapping was " + actorMappingIndex);
						}

						var thisMapping = mappings[tileMappingIndex];
						var startIndex = tilesetStartIndices[thisMapping.TileSetIndex];

						map.Tiles[new CPos(x + 1, y + 1)] = new TerrainTile((ushort)(startIndex + thisMapping.TileIndex), 0);
					}
				}

				var numSomething = stream.ReadInt32(); // always 217
				var aftertiles1 = stream.ReadInt32(); // always 1
				var aftertiles2 = stream.ReadInt32(); // always 1
				var aftertiles3 = stream.ReadInt32(); // always 0
				var aftertiles4 = stream.ReadInt32(); // always 4
				var checkString = stream.ReadASCII(4); // always BLUE
				var aftertiles5 = stream.ReadInt32(); // always 4
				var aftertiles6 = stream.ReadInt32(); // always 4

				// always digits 1 - 16
				for (var i = 0; i < 16; i++)
				{
					var digit = stream.ReadInt32();
				}

				for (var y = 0; y < numSomething - 1; y++)
				{
					var actorNameLength = stream.ReadInt32();
					var actorName = stream.ReadASCII(actorNameLength);
					var sizeX = stream.ReadInt32();
					var sizeY = stream.ReadInt32();

					for (var x = 0; x < sizeX * sizeY; x++)
					{
						var tileDigit = stream.ReadInt32();
					}
				}

				var checkString2 = stream.ReadASCII(11);
				Console.WriteLine($"{checkString2}");
			}

			mapPlayers = new MapPlayers(map.Rules, 0);
			map.PlayerDefinitions = mapPlayers.ToMiniYaml();

			var dest = Path.GetFileNameWithoutExtension(args[1]) + ".oramap";
			var mapLocations = Game.ModData.Manifest.MapFolders;
			var userMapPath = mapLocations.First(mapLocation => mapLocation.Value == "User");
			var targetPath = Path.Combine(Platform.ResolvePath(userMapPath.Key[1..]), dest);
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
