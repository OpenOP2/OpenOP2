#region Copyright & License Information
/*
 * Copyright (c) The OpenRA Developers and Contributors
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Widgets.Logic
{
	public class LoadIngameOp2UILogic : ChromeLogic
	{
		[ObjectCreator.UseCtor]
		public LoadIngameOp2UILogic(Widget widget, World world)
		{
			var playerWidgets = widget.Get("PLAYER_WIDGETS");

			if (world.LocalPlayer != null)
			{
				var op2Widgets = Game.LoadWidget(world, "OP2_WIDGETS", playerWidgets, new WidgetArgs());
			}

			world.GameOver += () =>
			{
			};
		}
	}
}
