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
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Widgets;
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
		public readonly string StructureFactoryPalette = null;
		public readonly string ProductionPaletteWidget = null;
		public readonly string StructureQueueName = "Buildings";

		public override object Create(ActorInitializer init) { return new StructureFactoryFromSelection(init.World, this); }
	}

	class StructureFactoryFromSelection : INotifySelection
	{
		readonly World world;
		readonly StructureFactoryFromSelectionInfo info;
		readonly Lazy<ProductionPaletteWidget> paletteWidget;
		readonly Lazy<StructureFactoryPaletteWidget> structureFactoryWidget;

		public StructureFactoryFromSelection(World world, StructureFactoryFromSelectionInfo info)
		{
			this.world = world;
			this.info = info;
			paletteWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.ProductionPaletteWidget) as ProductionPaletteWidget);
			structureFactoryWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.StructureFactoryPalette) as StructureFactoryPaletteWidget);
		}

		void INotifySelection.SelectionChanged()
		{
			// Disable for spectators
			if (world.LocalPlayer == null)
				return;

			// Check for custom palette icons
			var productionQueue = world.Selection.Actors
				.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
				.SelectMany(a => a.TraitsImplementing<ProductionQueue>())
				.Where(a => a.Info.Group == info.StructureQueueName)
				.FirstOrDefault(q => q.Enabled);

			if (productionQueue != null)
			{
				structureFactoryWidget.Value.CurrentQueue = productionQueue;

				paletteWidget.Value.CurrentQueue = null;
			}
		}
	}
}
