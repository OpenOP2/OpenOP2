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
using OpenRA.Graphics;
using OpenRA.Mods.Common.Lint;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.OpenOP2.Traits;
using OpenRA.Network;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Widgets
{
	public class BayIcon
	{
		public string Name;
		public HotkeyReference Hotkey;
		public Sprite Sprite;
		public PaletteReference Palette;
		public float2 Pos;
	}

	public class CargoBaysPaletteWidget : Widget
	{
		public readonly int Columns = 3;
		public readonly int2 IconSize = new int2(64, 48);
		public readonly int2 IconMargin = int2.Zero;
		public readonly int2 IconSpriteOffset = int2.Zero;

		public readonly string ClickSound = ChromeMetrics.Get<string>("ClickSound");
		public readonly string ClickDisabledSound = ChromeMetrics.Get<string>("ClickDisabledSound");
		public readonly string TooltipContainer;
		public readonly string TooltipTemplate = "PRODUCTION_TOOLTIP";

		// Note: LinterHotkeyNames assumes that these are disabled by default
		public readonly string HotkeyPrefix = null;
		public readonly int HotkeyCount = 0;
		public readonly HotkeyReference SelectProductionBuildingHotkey = new HotkeyReference();

		public readonly string OverlayFont = "TinyBold";
		public readonly string SymbolsFont = "Symbols";

		public readonly bool DrawTime = true;

		public readonly string ReadyText = "";

		public readonly string HoldText = "";

		public readonly string InfiniteSymbol = "\u221E";

		public int DisplayedIconCount { get; private set; }
		public int TotalIconCount { get; private set; }
		public event Action<int, int> OnIconCountChanged = (a, b) => { };

		//public BayIcon TooltipIcon { get; private set; }
		//public Func<BayIcon> GetTooltipIcon;
		public readonly World World;
		readonly ModData modData;
		readonly OrderManager orderManager;

		public int MinimumRows = 4;
		public int MaximumRows = int.MaxValue;

		public int IconRowOffset = 0;
		public int MaxIconRowOffset = int.MaxValue;

		readonly Lazy<TooltipContainerWidget> tooltipContainer;
		CargoBay cargoBay;
		HotkeyReference[] hotkeys;

		public CargoBay CargoBay
		{
			get => cargoBay;
			set
			{
				cargoBay = value;
				//if (cargoBay != null)
				//	UpdateCachedProductionIconOverlays();

				RefreshIcons();
			}
		}

		public override Rectangle EventBounds => eventBounds;
		Dictionary<Rectangle, BayIcon> icons = new Dictionary<Rectangle, BayIcon>();
		//Animation cantBuild;
		//Animation clock;
		Rectangle eventBounds = Rectangle.Empty;

		readonly WorldRenderer worldRenderer;

		SpriteFont overlayFont, symbolFont;
		float2 iconOffset, holdOffset, readyOffset, timeOffset, queuedOffset, infiniteOffset;

		Player cachedQueueOwner;
		IProductionIconOverlay[] pios;

		[CustomLintableHotkeyNames]
		public static IEnumerable<string> LinterHotkeyNames(MiniYamlNode widgetNode, Action<string> emitError)
		{
			var prefix = "";
			var prefixNode = widgetNode.Value.Nodes.FirstOrDefault(n => n.Key == "HotkeyPrefix");
			if (prefixNode != null)
				prefix = prefixNode.Value.Value;

			var count = 0;
			var countNode = widgetNode.Value.Nodes.FirstOrDefault(n => n.Key == "HotkeyCount");
			if (countNode != null)
				count = FieldLoader.GetValue<int>("HotkeyCount", countNode.Value.Value);

			if (count == 0)
				return Array.Empty<string>();

			if (string.IsNullOrEmpty(prefix))
				emitError($"{widgetNode.Location} must define HotkeyPrefix if HotkeyCount > 0.");

			return Exts.MakeArray(count, i => prefix + (i + 1).ToString("D2"));
		}

		[ObjectCreator.UseCtor]
		public CargoBaysPaletteWidget(ModData modData, OrderManager orderManager, World world, WorldRenderer worldRenderer)
		{
			this.modData = modData;
			this.orderManager = orderManager;
			World = world;
			this.worldRenderer = worldRenderer;
			//GetTooltipIcon = () => TooltipIcon;
			tooltipContainer = Exts.Lazy(() =>
				Ui.Root.Get<TooltipContainerWidget>(TooltipContainer));
		}

		public override void Initialize(WidgetArgs args)
		{
			base.Initialize(args);

			hotkeys = Exts.MakeArray(HotkeyCount,
				i => modData.Hotkeys[HotkeyPrefix + (i + 1).ToString("D2")]);

			overlayFont = Game.Renderer.Fonts[OverlayFont];
			Game.Renderer.Fonts.TryGetValue(SymbolsFont, out symbolFont);

			iconOffset = 0.5f * IconSize.ToFloat2() + IconSpriteOffset;
			//queuedOffset = new float2(4, 2);
			//holdOffset = iconOffset - overlayFont.Measure(HoldText) / 2;
			//readyOffset = iconOffset - overlayFont.Measure(ReadyText) / 2;

			if (ChromeMetrics.TryGet("InfiniteOffset", out infiniteOffset))
				infiniteOffset += queuedOffset;
			else
				infiniteOffset = queuedOffset;
		}

		public void ScrollDown()
		{
			if (CanScrollDown)
				IconRowOffset++;
		}

		public bool CanScrollDown
		{
			get
			{
				var totalRows = (TotalIconCount + Columns - 1) / Columns;

				return IconRowOffset < totalRows - MaxIconRowOffset;
			}
		}

		public void ScrollUp()
		{
			if (CanScrollUp)
				IconRowOffset--;
		}

		public bool CanScrollUp => IconRowOffset > 0;

		public void ScrollToTop()
		{
			IconRowOffset = 0;
		}

		//public IEnumerable<ActorInfo> AllBuildables
		//{
		//	get
		//	{
		//		if (CurrentQueue == null)
		//			return Enumerable.Empty<ActorInfo>();

		//		return CurrentQueue.AllItems().OrderBy(a => a.TraitInfo<BuildableInfo>().BuildPaletteOrder);
		//	}
		//}

		public override void Tick()
		{
			TotalIconCount = CargoBay != null ? CargoBay.Info.Capacity : 0;

			//if (CargoBay != null && !CargoBay.IsInWorld)
			//	CargoBay = null;

			if (CargoBay != null)
			{
				//if (CargoBay.Owner != cachedQueueOwner)
				//	UpdateCachedProductionIconOverlays();

				RefreshIcons();
			}
		}

		public override void MouseEntered()
		{
			//if (TooltipContainer != null)
			//	tooltipContainer.Value.SetTooltip(TooltipTemplate,
			//		new WidgetArgs() { { "player", World.LocalPlayer }, { "getTooltipIcon", GetTooltipIcon }, { "world", World } });
		}

		public override void MouseExited()
		{
			if (TooltipContainer != null)
				tooltipContainer.Value.RemoveTooltip();
		}

		public override bool HandleMouseInput(MouseInput mi)
		{
			var icon = icons.Where(i => i.Key.Contains(mi.Location))
				.Select(i => i.Value).FirstOrDefault();

			//if (mi.Event == MouseInputEvent.Move)
			//	TooltipIcon = icon;

			if (mi.Event == MouseInputEvent.Scroll)
			{
				if (mi.Delta.Y < 0 && CanScrollDown)
				{
					ScrollDown();
					Ui.ResetTooltips();
					Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
				}
				else if (mi.Delta.Y > 0 && CanScrollUp)
				{
					ScrollUp();
					Ui.ResetTooltips();
					Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
				}
			}

			if (icon == null)
				return false;

			// Eat mouse-up events
			if (mi.Event != MouseInputEvent.Down)
				return true;

			return HandleEvent(icon, mi.Button);
		}

		bool HandleLeftClick(CargoBay cargoBay, BayIcon icon)
		{
			Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);

			if (cargoBay != null)
			{
				var carrier = cargoBay.GetCargoCarrier();
				if (carrier != null)
				{
					// TODO: Handle disburse of cargo using an order instead
					var cargoName = icon.Name;
					if (cargoBay.GetCargo(cargoName))
					{
						carrier.AddCargo(cargoName);
						return true;
					}

					return false;
				}
			}

			return false;
		}

		bool HandleEvent(BayIcon icon, MouseButton btn)
		{
			var cargoBay = CargoBay;
			var handled = btn == MouseButton.Left ? HandleLeftClick(cargoBay, icon) : false;

			if (!handled)
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickDisabledSound, null);

			return true;
		}

		public override bool HandleKeyPress(KeyInput e)
		{
			if (e.Event == KeyInputEvent.Up || CargoBay == null)
				return false;

			//if (SelectProductionBuildingHotkey.IsActivatedBy(e))
			//	return SelectProductionBuilding();

			// HACK: enable production if the shift key is pressed
			e.Modifiers &= ~Modifiers.Shift;
			var toBuild = icons.Values.FirstOrDefault(i => i.Hotkey != null && i.Hotkey.IsActivatedBy(e));
			return toBuild != null && HandleEvent(toBuild, MouseButton.Left);
		}

		//bool SelectProductionBuilding()
		//{
		//	var viewport = worldRenderer.Viewport;
		//	var selection = World.Selection;

		//	if (CargoBay == null)
		//		return true;

		//	var facility = CargoBay;

		//	if (facility == null || facility.OccupiesSpace == null)
		//		return true;

		//	if (selection.Actors.Count() == 1 && selection.Contains(facility))
		//		viewport.Center(selection.Actors);
		//	else
		//		selection.Combine(World, new[] { facility }, false, true);

		//	Game.Sound.PlayNotification(World.Map.Rules, null, "Sounds", ClickSound, null);
		//	return true;
		//}

		//void UpdateCachedProductionIconOverlays()
		//{
		//	cachedQueueOwner = CurrentActor.Owner;
		//	pios = cachedQueueOwner.PlayerActor.TraitsImplementing<IProductionIconOverlay>().ToArray();
		//}

		public void RefreshIcons()
		{
			icons = new Dictionary<Rectangle, BayIcon>();

			var oldIconCount = DisplayedIconCount;
			DisplayedIconCount = 0;

			var rb = RenderBounds;
			//var faction = CargoBay.Owner.Faction;

			foreach (var item in CargoBay.Bays)
			{
				var image = CargoBay.Info.BayIconImage;
				var sequence = CargoBay.Info.BayIconSequence;

				if (item != null)
				{
					var actorRenderSprites = World.Map.Rules.Actors[item]?.TraitInfoOrDefault<RenderSpritesInfo>();
					image = actorRenderSprites != null ? actorRenderSprites.Image : image;
					sequence = "icon";
				}

				var x = DisplayedIconCount % Columns;
				var y = DisplayedIconCount / Columns;
				var rect = new Rectangle(rb.X + x * (IconSize.X + IconMargin.X), rb.Y + y * (IconSize.Y + IconMargin.Y), IconSize.X, IconSize.Y);

				var icon = new Animation(World, image);
				icon.Play(sequence);

				var palette = "player";

				var pi = new BayIcon()
				{
					Name = item ?? image,
					Hotkey = DisplayedIconCount < HotkeyCount ? hotkeys[DisplayedIconCount] : null,
					Sprite = icon.Image,
					Palette = worldRenderer.Palette(palette),
					Pos = new float2(rect.Location)
				};

				icons.Add(rect, pi);
				DisplayedIconCount++;
			}

			eventBounds = icons.Keys.Union();

			if (oldIconCount != DisplayedIconCount)
				OnIconCountChanged(oldIconCount, DisplayedIconCount);
		}

		public override void Draw()
		{
			timeOffset = iconOffset - overlayFont.Measure(WidgetUtils.FormatTime(0, World.Timestep)) / 2;

			if (CargoBay == null)
				return;

			// Icons
			Game.Renderer.EnableAntialiasingFilter();
			foreach (var icon in icons.Values)
			{
				WidgetUtils.DrawSpriteCentered(icon.Sprite, icon.Palette, icon.Pos + iconOffset);

				// Draw the ProductionIconOverlay's sprites
				//foreach (var pio in pios.Where(p => p.IsOverlayActive(icon.Actor)))
				//	WidgetUtils.DrawSpriteCentered(pio.Sprite, worldRenderer.Palette(pio.Palette), icon.Pos + iconOffset + pio.Offset(IconSize));
			}

			Game.Renderer.DisableAntialiasingFilter();
		}

		public override string GetCursor(int2 pos)
		{
			var icon = icons.Where(i => i.Key.Contains(pos))
				.Select(i => i.Value).FirstOrDefault();

			return icon != null ? base.GetCursor(pos) : null;
		}
	}
}
