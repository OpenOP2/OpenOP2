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

using OpenRA.Graphics;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[TraitLocation(SystemActors.World)]
	public class LoadOp2WidgetAtGameStartInfo : TraitInfo
	{
		[Desc("The OP2 widget tree to open when a regular map is loaded (i.e. the ingame UI).")]
		public readonly string IngameRoot = "OP2_WIDGETS";

		public override object Create(ActorInitializer init) { return new LoadOp2WidgetAtGameStart(this); }
	}

	public class LoadOp2WidgetAtGameStart : IWorldLoaded, INotifyGameLoading, INotifyGameLoaded
	{
		readonly LoadOp2WidgetAtGameStartInfo info;
		Widget root;

		public LoadOp2WidgetAtGameStart(LoadOp2WidgetAtGameStartInfo info)
		{
			this.info = info;
		}

		void INotifyGameLoading.GameLoading(World world)
		{
			// Clear any existing widget state
			// if (info.ClearRoot)
			// Ui.ResetAll();

			// Ui.OpenWindow(info.GameSaveLoadingRoot, new WidgetArgs()
			// {
			// 	{ "world", world }
			// });
		}

		void IWorldLoaded.WorldLoaded(World world, WorldRenderer wr)
		{
			// if (!world.IsLoadingGameSave && info.ClearRoot)
			// Ui.ResetAll();
			var widget = info.IngameRoot;

			root = Game.LoadWidget(world, widget, Ui.Root, new WidgetArgs());

			// The Lua API requires the UI to available, so hide it instead
			root.IsVisible = () => false;
		}

		void INotifyGameLoaded.GameLoaded(World world)
		{
			root.IsVisible = () => true;
		}
	}
}
