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
	/// Populate building picker widget with selected builder unit production queue upon selection
	/// </summary>
	class BuildingPickerFromSelectionInfo : TraitInfo
	{
		public string BuildSelectPalette = null;

		public override object Create(ActorInitializer init) { return new BuildingPickerFromSelection(init.World, this); }
	}

	class BuildingPickerFromSelection : INotifySelection
	{
		readonly World world;
		readonly Lazy<BuildSelectPaletteWidget> buildSelectWidget;

		public BuildingPickerFromSelection(World world, BuildingPickerFromSelectionInfo info)
		{
			this.world = world;
			buildSelectWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.BuildSelectPalette) as BuildSelectPaletteWidget);
		}

		void INotifySelection.SelectionChanged()
		{
			// Disable for spectators
			if (world.LocalPlayer == null)
				return;

			// Check for builder unit
			var builderQueue = world.Selection.Actors
				.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
				.SelectMany(a => a.TraitsImplementing<BuilderUnit>())
				.FirstOrDefault(q => q.Enabled);

			if (builderQueue == null)
			{
				var types = world.Selection.Actors.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
					.SelectMany(a => a.TraitsImplementing<Common.Traits.Production>())
					.SelectMany(t => t.Info.Produces);

				builderQueue = world.LocalPlayer.PlayerActor.TraitsImplementing<BuilderUnit>()
					.FirstOrDefault(q => q.Enabled && types.Contains(q.Info.Type));
			}

			if (builderQueue != null)
			{
				buildSelectWidget.Value.CurrentQueue = builderQueue;
			}
		}
	}
}
