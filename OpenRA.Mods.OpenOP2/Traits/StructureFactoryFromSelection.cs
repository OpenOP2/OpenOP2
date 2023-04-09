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
using OpenRA.Mods.OpenOP2.Widgets;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Traits
{
	/// <summary>
	/// Populate dozer widget with selected dozer's UseCustomPaletteIcons upon selection
	/// </summary>
	class StructureFactoryFromSelectionInfo : TraitInfo
	{
		public string StructureFactoryPalette = null;

		public override object Create(ActorInitializer init) { return new StructureFactoryFromSelection(init.World, this); }
	}

	class StructureFactoryFromSelection : INotifySelection
	{
		readonly World world;
		readonly Lazy<CargoBaysPaletteWidget> cargoBaysWidget;

		public StructureFactoryFromSelection(World world, StructureFactoryFromSelectionInfo info)
		{
			this.world = world;
			cargoBaysWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.StructureFactoryPalette) as CargoBaysPaletteWidget);
		}

		void INotifySelection.SelectionChanged()
		{
			// Disable for spectators
			if (world.LocalPlayer == null)
				return;

			// Check for custom palette icons
			var cargoBay = world.Selection.Actors
				.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
				.Select(a => a.TraitOrDefault<CargoBay>())
				.FirstOrDefault();

			if (cargoBay != null)
			{
				cargoBaysWidget.Value.CargoBay = cargoBay;
			}
		}
	}
}
