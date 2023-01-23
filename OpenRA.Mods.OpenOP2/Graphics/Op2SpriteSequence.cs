using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;

namespace OpenRA.Mods.OpenOP2.Graphics
{
	public class Op2SpriteSequence : DefaultSpriteSequence
	{
		public Op2SpriteSequence(ModData modData, string tileSet, SpriteCache cache, ISpriteSequenceLoader loader, string sequence, string animation, MiniYaml info)
			: base(modData, tileSet, cache, loader, sequence, animation, info)
		{
		}
	}
}
