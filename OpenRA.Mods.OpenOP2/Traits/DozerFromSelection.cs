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
using System.Linq;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.OpenOP2.Widgets;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Traits
{
	/// <summary>
	/// Populate dozer widget with selected dozer's UseCustomPaletteIcons upon selection
	/// </summary>
	class DozerFromSelectionInfo : TraitInfo
	{
		public string DozerPalette = null;

		public override object Create(ActorInitializer init) { return new DozerFromSelection(init.World, this); }
	}

	class DozerFromSelection : INotifySelection
	{
		readonly World world;
		readonly Lazy<DozerPaletteWidget> buildSelectWidget;

		public DozerFromSelection(World world, DozerFromSelectionInfo info)
		{
			this.world = world;
			buildSelectWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.DozerPalette) as DozerPaletteWidget);
		}

		void INotifySelection.SelectionChanged()
		{
			// Disable for spectators
			if (world.LocalPlayer == null)
				return;

			// Check for custom palette icons
			var customPaletteIcons = world.Selection.Actors
				.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
				.Where(a => a.TraitOrDefault<CustomProductionPalette>() != null)
				.FirstOrDefault();

			if (customPaletteIcons != null)
			{
				buildSelectWidget.Value.CurrentActor = customPaletteIcons;
			}
		}
	}
}
