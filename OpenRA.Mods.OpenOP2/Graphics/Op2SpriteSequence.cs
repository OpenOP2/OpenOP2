#region Copyright & License Information
/*
 * Copyright 2007-2022 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;
using Util = OpenRA.Mods.Common.Util;

namespace OpenRA.Mods.OpenOP2.Graphics
{
	public interface IOp2SpriteSequence : ISpriteSequence
	{
		int FacingOffset { get; }
	}

	public class Op2SpriteSequenceLoader : DefaultSpriteSequenceLoader
	{
		public Op2SpriteSequenceLoader(ModData modData)
			: base(modData) { }

		public override ISpriteSequence CreateSequence(ModData modData, string tileSet, SpriteCache cache, string sequence, string animation, MiniYaml info)
		{
			return new Op2SpriteSequence(modData, tileSet, cache, this, sequence, animation, info);
		}
	}

	[Desc("OP2 sprite sequence implementation with optional facings offset.")]
	public class Op2SpriteSequence : DefaultSpriteSequence, IOp2SpriteSequence
	{
		[Desc("Facing index to start from.")]
		static readonly SpriteSequenceField<int> FacingOffset = new SpriteSequenceField<int>(nameof(FacingOffset), 0);
		int IOp2SpriteSequence.FacingOffset => facingOffset;
		readonly int facingOffset;

		public Op2SpriteSequence(ModData modData, string tileSet, SpriteCache cache, ISpriteSequenceLoader loader, string sequence, string animation, MiniYaml info)
			: base(modData, tileSet, cache, loader, sequence, animation, info)
		{
			var d = info.ToDictionary();

			try
			{
				facingOffset = LoadField(d, FacingOffset);
			}
			catch (FormatException f)
			{
				throw new FormatException($"Failed to parse sequences for {sequence}.{animation} at {info.Nodes[0].Location}:\n{f}");
			}
		}

		protected override int GetFacingFrameOffset(WAngle facing)
		{
			return (Util.IndexFacing(facing, facings) + facingOffset) % facings;
		}
	}
}
