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

using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Removes blight from its radius and prevents it from spreading.")]
	public class SpawnsBlightInfo : TraitInfo
	{
		[Desc("After how many ticks.")]
		public int After = 0;

		public override object Create(ActorInitializer init) { return new SpawnsBlight(init.Self, this); }
	}

	public class SpawnsBlight : ITick
	{
		private World world;
		private Actor actor;
		private int count;
		private SpawnsBlightInfo info;

		public bool IsReady { get; private set; } = false;

		public SpawnsBlight(Actor self, SpawnsBlightInfo info)
		{
			actor = self;
			world = self.World;
			this.info = info;
			count = info.After;
		}

		void ITick.Tick(Actor self)
		{
			actor = self;

			if (IsReady)
			{
				return;
			}

			if (count <= 0)
			{
				IsReady = true;
			}

			count--;
		}
	}
}
