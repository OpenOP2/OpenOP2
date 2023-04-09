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
using System.Collections.Generic;
using System.Linq;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Used to control visibility of additional sidebar palette widgets besides the existing production palette.")]
	public class UseCustomProductionPalettesInfo : TraitInfo
	{
		[Desc("Name of the existing sidebar production widget we want to show or hide when we change selection.")]
		public readonly string MainProductionWidgetName = "SIDEBAR_PRODUCTION";

		[FieldLoader.LoadUsing("LoadCustomProductionPalettes")]
		public Dictionary<string, CustomProductionPalettesPaletteInfo> CustomProductionPalettes;

		static object LoadCustomProductionPalettes(MiniYaml yaml)
		{
			var retList = new Dictionary<string, CustomProductionPalettesPaletteInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "CustomProductionPalettes");

			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("Palette")))
			{
				var ret = new CustomProductionPalettesPaletteInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}

		public override object Create(ActorInitializer init) { return new UseCustomProductionPalettes(init.World, this); }
	}

	public class CustomProductionPalettesPaletteInfo
	{
		[Desc("The name of the production palette type.")]
		public string Name;

		[Desc("The name of the production palette widget.")]
		public string WidgetName;
	}

	public class UseCustomProductionPalettes : INotifySelection
	{
		readonly World world;
		readonly Lazy<ContainerWidget> mainProductionWidget;
		readonly Dictionary<string, Lazy<Widget>> paletteWidgets = new Dictionary<string, Lazy<Widget>>();

		public UseCustomProductionPalettes(World world, UseCustomProductionPalettesInfo info)
		{
			this.world = world;

			mainProductionWidget = Exts.Lazy(() => Ui.Root.GetOrNull(info.MainProductionWidgetName) as ContainerWidget);

			foreach (var widgetInfo in info.CustomProductionPalettes.Values)
			{
				var widget = Exts.Lazy(() => Ui.Root.GetOrNull(widgetInfo.WidgetName));
				paletteWidgets.Add(widgetInfo.Name, widget);
			}
		}

		void INotifySelection.SelectionChanged()
		{
			// Disable for spectators
			if (world.LocalPlayer == null)
				return;

			var customProductionPalette = world.Selection.Actors
				.Where(a => a.IsInWorld && a.World.LocalPlayer == a.Owner)
				.Select(a => a.TraitOrDefault<CustomProductionPalette>())
				.FirstOrDefault();

			if (customProductionPalette != null)
			{
				mainProductionWidget.Value.Visible = false;
				var paletteWidget = paletteWidgets[customProductionPalette.Name];
				if (paletteWidget != null)
				{
					foreach (var existingWidget in paletteWidgets.Values)
					{
						existingWidget.Value.Visible = false;
					}

					paletteWidget.Value.Visible = true;
				}
			}
			else
			{
				foreach (var existingWidget in paletteWidgets.Values)
				{
					existingWidget.Value.Visible = false;
				}

				mainProductionWidget.Value.Visible = true;
			}
		}
	}
}
