#region Copyright & License Information
/*
 * Copyright 2007-2021 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Changes tile types underneath to bulldozed tiles of the correct type upon creation.")]
	public class BulldozesTerrainWhenCreatedInfo : ConditionalTraitInfo
	{
		[Desc("Size of bulldozed area.")]
		public int2 Size = new(3, 3);

		[Desc("Offset from the top left of the object when bulldozing.")]
		public int2 Offset = new(0, 0);

		public override object Create(ActorInitializer init) { return new BulldozesTerrainWhenCreated(this); }

		[FieldLoader.LoadUsing(nameof(LoadReplacements))]
		public Dictionary<string, BulldozesTerrainReplacementInfo> Replacements;

		static object LoadReplacements(MiniYaml yaml)
		{
			var retList = new Dictionary<string, BulldozesTerrainReplacementInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "Replacements");
			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("Replacement")))
			{
				var ret = new BulldozesTerrainReplacementInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}
	}

	public class BulldozesTerrainReplacementInfo
	{
		[Desc("The min tile index type to replace.")]
		public int MinTileIndex = 0;

		[Desc("The max tile index type to replace.")]
		public int MaxTileIndex = 0;

		[Desc("The type of tile to place when bulldozing.")]
		public ushort PlaceTileType = 0;
	}

	public class BulldozesTerrainWhenCreated : ConditionalTrait<BulldozesTerrainWhenCreatedInfo>
	{
		readonly BulldozesTerrainWhenCreatedInfo info;
		readonly byte[] randomIndex = { 0, 1, 2, 3, 4, 5, 6, 7 };

		public BulldozesTerrainWhenCreated(BulldozesTerrainWhenCreatedInfo info)
			: base(info)
		{
			this.info = info;
		}

		protected override void Created(Actor self)
		{
			base.Created(self);

			for (var y = 0; y < info.Size.Y; y++)
			{
				for (var x = 0; x < info.Size.X; x++)
				{
					var bulldozeCell = self.Location + new CVec(x + info.Offset.X, y + info.Offset.Y);
					if (!self.World.Map.Contains(bulldozeCell))
						continue;

					var existingTile = self.World.Map.Tiles[bulldozeCell];
					var tileType = existingTile.Type;

					foreach (var replacement in info.Replacements)
					{
						if (tileType >= replacement.Value.MinTileIndex && tileType <= replacement.Value.MaxTileIndex)
						{
							self.World.Map.Tiles[bulldozeCell] = new TerrainTile(replacement.Value.PlaceTileType, randomIndex.Random(Game.CosmeticRandom));
							break;
						}
					}
				}
			}
		}
	}
}
