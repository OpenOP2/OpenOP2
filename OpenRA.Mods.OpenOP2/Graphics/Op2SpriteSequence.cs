using System;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;
using Util = OpenRA.Mods.Common.Util;

namespace OpenRA.Mods.OpenOP2.Graphics
{
	public class Op2SpriteSequence : DefaultSpriteSequence
	{
		public Op2SpriteSequence(ModData modData, TileSet tileSet, SpriteCache cache, ISpriteSequenceLoader loader, string sequence, string animation, MiniYaml info)
			: base(modData, tileSet, cache, loader, sequence, animation, info)
		{
		}

		protected override Sprite GetSprite(int start, int frame, int facing)
		{
			return base.GetSprite(start, frame, facing);
		}
	}
}
