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

using System;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;
using OpenRA.Mods.OpenOP2.Interfaces;
using Util = OpenRA.Mods.Common.Util;

namespace OpenRA.Mods.OpenOP2.Graphics
{
	/// <inheritdoc/>
	[Desc("OP2 sprite sequence implementation with optional facings offset.")]
	public class Op2SpriteSequence : DefaultSpriteSequence, IOp2SpriteSequence
	{
		[Desc("Facing index to start from.")]
		static readonly SpriteSequenceField<int> FacingOffset = new SpriteSequenceField<int>(nameof(FacingOffset), 0);

		int IOp2SpriteSequence.FacingOffset => facingOffset;

		readonly int facingOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="Op2SpriteSequence"/> class.
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="loader"></param>
		/// <param name="image"></param>
		/// <param name="sequence"></param>
		/// <param name="data"></param>
		/// <param name="defaults"></param>
		/// <exception cref="FormatException"></exception>
		public Op2SpriteSequence(SpriteCache cache, ISpriteSequenceLoader loader, string image, string sequence, MiniYaml data, MiniYaml defaults)
			: base(cache, loader, image, sequence, data, defaults)
		{
			try
			{
				this.facingOffset = LoadField(FacingOffset, data, defaults);
			}
			catch (FormatException f)
			{
				throw new FormatException($"Failed to parse sequences for {image}.{sequence} at {data.Nodes[0].Location}:\n{f}");
			}
		}

		/// <inheritdoc/>
		protected override int GetFacingFrameOffset(WAngle facing)
		{
			return (Util.IndexFacing(facing, this.facings) + this.facingOffset) % this.facings;
		}
	}
}
