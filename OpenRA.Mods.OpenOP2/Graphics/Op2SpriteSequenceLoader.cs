#region Copyright & License Information
/*
 * Copyright 2007-2024 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;

namespace OpenRA.Mods.OpenOP2.Graphics
{
	/// <inheritdoc/>
	public class Op2SpriteSequenceLoader : DefaultSpriteSequenceLoader
	{
		public Op2SpriteSequenceLoader(ModData modData)
			: base(modData) { }

		/// <inheritdoc/>
		public override ISpriteSequence CreateSequence(
			ModData modData,
			string tileSet,
			SpriteCache cache,
			string image,
			string sequence,
			MiniYaml data,
			MiniYaml defaults)
		{
			return new Op2SpriteSequence(cache, this, image, sequence, data, defaults);
		}
	}
}
