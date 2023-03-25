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
using OpenRA.Mods.Common.Orders;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Network;
using OpenRA.Primitives;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenOP2.Widgets
{
	// Copy of ProductionIcon, this will replace it
	public class BaseProductionIcon
	{
		public ActorInfo Actor;
		public string Name;
		public HotkeyReference Hotkey;
		public Sprite Sprite;
		public PaletteReference Palette;
		public float2 Pos;
	}

	// BaseProductionIcon with C&C/RA style default production queue info
	public class DefaultProductionIcon : BaseProductionIcon
	{
		public PaletteReference IconClockPalette;
		public PaletteReference IconDarkenPalette;
		public List<ProductionItem> Queued;
		public ProductionQueue ProductionQueue;
	}

	// DTO object
	public class GenericProductionPaletteIcon
	{
		public ActorInfo Actor;
		public string Name;
		public Sprite Sprite;
		public string Palette;
	}

	public interface IProductionPaletteProvider
	{
		IList<GenericProductionPaletteIcon> Icons { get; }

		void RefreshIcons();
	}

	public class ProductionPaletteProviderInfo : TraitInfo
	{

		public override object Create(ActorInitializer init)
		{
			return new DefaultProductionPaletteProvider(init, this);
		}
	}

	public class DefaultProductionPaletteProvider : IProductionPaletteProvider, INotifySelected
	{
		public IList<GenericProductionPaletteIcon> Icons { get; private set; } = new List<GenericProductionPaletteIcon>();

		readonly World world;
		readonly ProductionPaletteProviderInfo info;
		ProductionQueue currentQueue;
		public ProductionQueue CurrentQueue
		{
			get => currentQueue;
			set
			{
				currentQueue = value;

				RefreshIcons();
			}
		}

		public IEnumerable<ActorInfo> AllBuildables
		{
			get
			{
				if (CurrentQueue == null)
					return Enumerable.Empty<ActorInfo>();

				return CurrentQueue.AllItems().OrderBy(a => a.TraitInfo<BuildableInfo>().BuildPaletteOrder);
			}
		}

		public DefaultProductionPaletteProvider(ActorInitializer init, ProductionPaletteProviderInfo info)
		{
			world = init.World;
			this.info = info;
		}

		public void RefreshIcons()
		{
			Icons = new List<GenericProductionPaletteIcon>();
			var producer = CurrentQueue != null ? CurrentQueue.MostLikelyProducer() : default;
			if (CurrentQueue == null || producer.Trait == null)
			{
				return;
			}

			var faction = producer.Trait.Faction;

			foreach (var item in AllBuildables)
			{
				var rsi = item.TraitInfo<RenderSpritesInfo>();
				var icon = new Animation(world, rsi.GetImage(item, faction));
				var bi = item.TraitInfo<BuildableInfo>();
				icon.Play(bi.Icon);

				var palette = bi.IconPaletteIsPlayerPalette ? bi.IconPalette + producer.Actor.Owner.InternalName : bi.IconPalette;

				var pi = new GenericProductionPaletteIcon()
				{
					Actor = item,
					Name = item.Name,
					Sprite = icon.Image,
					Palette = palette,
				};

				Icons.Add(pi);
			}
		}

		void INotifySelected.Selected(Actor self)
		{
			CurrentQueue = self.TraitOrDefault<ProductionQueue>();
		}
	}

	public class DefaultProductionPaletteWidget : BaseProductionPaletteWidget<DefaultProductionIcon, DefaultProductionPaletteProvider>
	{
		public enum ReadyTextStyleOptions { Solid, AlternatingColor, Blinking }
		public readonly ReadyTextStyleOptions ReadyTextStyle = ReadyTextStyleOptions.AlternatingColor;
		public readonly Color ReadyTextAltColor = Color.Gold;

		public readonly string ClockAnimation = "clock";
		public readonly string ClockSequence = "idle";
		public readonly string ClockPalette = "chrome";

		public readonly string NotBuildableAnimation = "clock";
		public readonly string NotBuildableSequence = "idle";
		public readonly string NotBuildablePalette = "chrome";

		public readonly string OverlayFont = "TinyBold";
		public readonly string SymbolsFont = "Symbols";

		public readonly bool DrawTime = true;

		public readonly string ReadyText = "";

		public readonly string HoldText = "";

		public readonly string InfiniteSymbol = "\u221E";
		Animation cantBuild;
		Animation clock;
		SpriteFont overlayFont, symbolFont;
		float2 holdOffset, readyOffset, timeOffset, queuedOffset, infiniteOffset;
		IProductionIconOverlay[] pios;

		Player cachedQueueOwner;

		public override DefaultProductionPaletteProvider ProductionPaletteProvider
		{
			set
			{
				base.ProductionPaletteProvider = value;
				if (value.CurrentQueue != null)
					UpdateCachedProductionIconOverlays();
			}
		}

		public DefaultProductionPaletteWidget(ModData modData, OrderManager orderManager, World world, WorldRenderer worldRenderer)
			: base(modData, orderManager, world, worldRenderer)
		{

		}

		public override void Initialize(WidgetArgs args)
		{
			base.Initialize(args);

			clock = new Animation(World, ClockAnimation);
			cantBuild = new Animation(World, NotBuildableAnimation);
			cantBuild.PlayFetchIndex(NotBuildableSequence, () => 0);

			overlayFont = Game.Renderer.Fonts[OverlayFont];
			Game.Renderer.Fonts.TryGetValue(SymbolsFont, out symbolFont);

			queuedOffset = new float2(4, 2);
			holdOffset = iconOffset - overlayFont.Measure(HoldText) / 2;
			readyOffset = iconOffset - overlayFont.Measure(ReadyText) / 2;

			if (ChromeMetrics.TryGet("InfiniteOffset", out infiniteOffset))
				infiniteOffset += queuedOffset;
			else
				infiniteOffset = queuedOffset;
		}

		protected override bool HandleLeftClick(ProductionItem item, DefaultProductionIcon icon, int handleCount, Modifiers modifiers)
		{
			if (PickUpCompletedBuildingIcon(icon, item))
			{
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
				return true;
			}

			if (item != null && item.Paused)
			{
				// Resume a paused item
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Speech", ProductionPaletteProvider.CurrentQueue.Info.QueuedAudio, World.LocalPlayer.Faction.InternalName);
				TextNotificationsManager.AddTransientLine(ProductionPaletteProvider.CurrentQueue.Info.QueuedTextNotification, World.LocalPlayer);

				World.IssueOrder(Order.PauseProduction(ProductionPaletteProvider.CurrentQueue.Actor, icon.Name, false));
				return true;
			}

			var buildable = ProductionPaletteProvider.CurrentQueue.BuildableItems().FirstOrDefault(a => a.Name == icon.Name);

			if (buildable != null)
			{
				// Queue a new item
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
				var canQueue = ProductionPaletteProvider.CurrentQueue.CanQueue(buildable, out var notification, out var textNotification);

				if (!ProductionPaletteProvider.CurrentQueue.AllQueued().Any())
				{
					Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Speech", notification, World.LocalPlayer.Faction.InternalName);
					TextNotificationsManager.AddTransientLine(textNotification, World.LocalPlayer);
				}

				if (canQueue)
				{
					var queued = !modifiers.HasModifier(Modifiers.Ctrl);
					World.IssueOrder(Order.StartProduction(ProductionPaletteProvider.CurrentQueue.Actor, icon.Name, handleCount, queued));
					return true;
				}
			}

			return false;
		}

		protected bool PickUpCompletedBuildingIcon(DefaultProductionIcon icon, ProductionItem item)
		{
			var actor = World.Map.Rules.Actors[icon.Name];

			if (item != null && item.Done && actor.HasTraitInfo<BuildingInfo>())
			{
				World.OrderGenerator = new PlaceBuildingOrderGenerator(ProductionPaletteProvider.CurrentQueue, icon.Name, worldRenderer);
				return true;
			}

			return false;
		}

		public void PickUpCompletedBuilding()
		{
			foreach (var icon in icons.Values)
			{
				var item = icon.Queued.FirstOrDefault();
				if (PickUpCompletedBuildingIcon(icon, item))
					break;
			}
		}

		protected override bool HandleRightClick(ProductionItem item, DefaultProductionIcon icon, int handleCount)
		{
			if (item == null)
				return false;

			Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);

			if (ProductionPaletteProvider.CurrentQueue.Info.DisallowPaused || item.Paused || item.Done || item.TotalCost == item.RemainingCost)
			{
				// Instantly cancel items that haven't started, have finished, or if the queue doesn't support pausing
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Speech", ProductionPaletteProvider.CurrentQueue.Info.CancelledAudio, World.LocalPlayer.Faction.InternalName);
				TextNotificationsManager.AddTransientLine(ProductionPaletteProvider.CurrentQueue.Info.CancelledTextNotification, World.LocalPlayer);

				World.IssueOrder(Order.CancelProduction(ProductionPaletteProvider.CurrentQueue.Actor, icon.Name, handleCount));
			}
			else
			{
				// Pause an existing item
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Speech", ProductionPaletteProvider.CurrentQueue.Info.OnHoldAudio, World.LocalPlayer.Faction.InternalName);
				TextNotificationsManager.AddTransientLine(ProductionPaletteProvider.CurrentQueue.Info.OnHoldTextNotification, World.LocalPlayer);

				World.IssueOrder(Order.PauseProduction(ProductionPaletteProvider.CurrentQueue.Actor, icon.Name, true));
			}

			return true;
		}

		protected override bool HandleMiddleClick(ProductionItem item, DefaultProductionIcon icon, int handleCount)
		{
			if (item == null)
				return false;

			// Directly cancel, skipping "on-hold"
			Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickSound, null);
			Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Speech", ProductionPaletteProvider.CurrentQueue.Info.CancelledAudio, World.LocalPlayer.Faction.InternalName);
			TextNotificationsManager.AddTransientLine(ProductionPaletteProvider.CurrentQueue.Info.CancelledTextNotification, World.LocalPlayer);

			World.IssueOrder(Order.CancelProduction(ProductionPaletteProvider.CurrentQueue.Actor, icon.Name, handleCount));

			return true;
		}

		protected override bool HandleEvent(DefaultProductionIcon icon, MouseButton btn, Modifiers modifiers)
		{
			var startCount = modifiers.HasModifier(Modifiers.Shift) ? 5 : 1;

			// PERF: avoid an unnecessary enumeration by casting back to its known type
			var cancelCount = modifiers.HasModifier(Modifiers.Ctrl) ? ((List<ProductionItem>)ProductionPaletteProvider.CurrentQueue.AllQueued()).Count : startCount;
			var item = icon.Queued.FirstOrDefault();
			var handled = btn == MouseButton.Left ? HandleLeftClick(item, icon, startCount, modifiers)
				: btn == MouseButton.Right ? HandleRightClick(item, icon, cancelCount)
				: btn == MouseButton.Middle && HandleMiddleClick(item, icon, cancelCount);

			if (!handled)
				Game.Sound.PlayNotification(World.Map.Rules, World.LocalPlayer, "Sounds", ClickDisabledSound, null);

			return true;
		}

		public override bool HandleKeyPress(KeyInput e)
		{
			if (e.Event == KeyInputEvent.Up)
				return false;

			if (ProductionPaletteProvider == null || ProductionPaletteProvider.CurrentQueue == null)
				return false;

			if (SelectProductionBuildingHotkey.IsActivatedBy(e))
				return SelectProductionBuilding();

			return base.HandleKeyPress(e);
		}

		public override void Draw()
		{
			timeOffset = iconOffset - overlayFont.Measure(WidgetUtils.FormatTime(0, World.Timestep)) / 2;

			if (ProductionPaletteProvider == null || ProductionPaletteProvider.CurrentQueue == null)
				return;

			var buildableItems = ProductionPaletteProvider.CurrentQueue.BuildableItems();

			base.Draw();

			Game.Renderer.EnableAntialiasingFilter();
			foreach (var icon in icons.Values)
			{
				// Draw the ProductionIconOverlay's sprites
				foreach (var pio in pios.Where(p => p.IsOverlayActive(icon.Actor)))
					WidgetUtils.DrawSpriteCentered(pio.Sprite, worldRenderer.Palette(pio.Palette), icon.Pos + iconOffset + pio.Offset(IconSize));

				// Build progress
				if (icon.Queued.Count > 0)
				{
					var first = icon.Queued[0];
					clock.PlayFetchIndex(ClockSequence,
						() => (first.TotalTime - first.RemainingTime)
							* (clock.CurrentSequence.Length - 1) / first.TotalTime);
					clock.Tick();

					WidgetUtils.DrawSpriteCentered(clock.Image, icon.IconClockPalette, icon.Pos + iconOffset);
				}
				else if (!buildableItems.Any(a => a.Name == icon.Name))
					WidgetUtils.DrawSpriteCentered(cantBuild.Image, icon.IconDarkenPalette, icon.Pos + iconOffset);
			}

			Game.Renderer.DisableAntialiasingFilter();

			// Overlays
			foreach (var icon in icons.Values)
			{
				var total = icon.Queued.Count;
				if (total > 0)
				{
					var first = icon.Queued[0];
					var waiting = !ProductionPaletteProvider.CurrentQueue.IsProducing(first) && !first.Done;
					if (first.Done)
					{
						if (ReadyTextStyle == ReadyTextStyleOptions.Solid || orderManager.LocalFrameNumber * worldRenderer.World.Timestep / 360 % 2 == 0)
							overlayFont.DrawTextWithContrast(ReadyText, icon.Pos + readyOffset, Color.White, Color.Black, 1);
						else if (ReadyTextStyle == ReadyTextStyleOptions.AlternatingColor)
							overlayFont.DrawTextWithContrast(ReadyText, icon.Pos + readyOffset, ReadyTextAltColor, Color.Black, 1);
					}
					else if (first.Paused)
						overlayFont.DrawTextWithContrast(HoldText,
							icon.Pos + holdOffset,
							Color.White, Color.Black, 1);
					else if (!waiting && DrawTime)
						overlayFont.DrawTextWithContrast(WidgetUtils.FormatTime(first.Queue.RemainingTimeActual(first), World.Timestep),
							icon.Pos + timeOffset,
							Color.White, Color.Black, 1);

					if (first.Infinite && symbolFont != null)
						symbolFont.DrawTextWithContrast(InfiniteSymbol,
							icon.Pos + infiniteOffset,
							Color.White, Color.Black, 1);
					else if (total > 1 || waiting)
						overlayFont.DrawTextWithContrast(total.ToString(),
							icon.Pos + queuedOffset,
							Color.White, Color.Black, 1);
				}
			}
		}

		public override void RefreshIcons()
		{
			base.RefreshIcons();

			var rb = RenderBounds;
			foreach (var item in icons.Values.Skip(IconRowOffset * Columns).Take(MaxIconRowOffset * Columns))
			{
				var x = DisplayedIconCount % Columns;
				var y = DisplayedIconCount / Columns;
				var rect = new Rectangle(rb.X + x * (IconSize.X + IconMargin.X), rb.Y + y * (IconSize.Y + IconMargin.Y), IconSize.X, IconSize.Y);

				item.IconClockPalette = worldRenderer.Palette(ClockPalette);
				item.IconDarkenPalette = worldRenderer.Palette(NotBuildablePalette);
				item.Queued = ProductionPaletteProvider.CurrentQueue.AllQueued().Where(a => a.Item == item.Name).ToList();
				item.ProductionQueue = ProductionPaletteProvider.CurrentQueue;
				item.Pos = new float2(rect.Location);
			}
		}

		public override void Tick()
		{
			TotalIconCount = ProductionPaletteProvider.AllBuildables.Count();

			if (ProductionPaletteProvider != null && ProductionPaletteProvider.CurrentQueue != null && !ProductionPaletteProvider.CurrentQueue.Actor.IsInWorld)
				ProductionPaletteProvider.CurrentQueue = null;

			if (ProductionPaletteProvider.CurrentQueue != null)
			{
				if (ProductionPaletteProvider.CurrentQueue.Actor.Owner != cachedQueueOwner)
					UpdateCachedProductionIconOverlays();

				RefreshIcons();
			}
		}

		bool SelectProductionBuilding()
		{
			var viewport = worldRenderer.Viewport;
			var selection = World.Selection;

			if (ProductionPaletteProvider.CurrentQueue == null)
				return true;

			var facility = ProductionPaletteProvider.CurrentQueue.MostLikelyProducer().Actor;

			if (facility == null || facility.OccupiesSpace == null)
				return true;

			if (selection.Actors.Count() == 1 && selection.Contains(facility))
				viewport.Center(selection.Actors);
			else
				selection.Combine(World, new[] { facility }, false, true);

			Game.Sound.PlayNotification(World.Map.Rules, null, "Sounds", ClickSound, null);
			return true;
		}

		void UpdateCachedProductionIconOverlays()
		{
			cachedQueueOwner = ProductionPaletteProvider.CurrentQueue.Actor.Owner;
			pios = cachedQueueOwner.PlayerActor.TraitsImplementing<IProductionIconOverlay>().ToArray();
		}
	}

	// Copy of ProductionPaletteWidget with changes to be more generic and not depend directly on ProductionQueue
	public class BaseProductionPaletteWidget<TIconItem, TProductionPaletteProvider>
		: Widget
		where TIconItem : BaseProductionIcon, new()
		where TProductionPaletteProvider : IProductionPaletteProvider
	{
		public readonly int Columns = 3;
		public readonly int2 IconSize = new int2(64, 48);
		public readonly int2 IconSpriteOffset = int2.Zero;
		public readonly int2 IconMargin = int2.Zero;

		public readonly string ClickSound = ChromeMetrics.Get<string>("ClickSound");
		public readonly string ClickDisabledSound = ChromeMetrics.Get<string>("ClickDisabledSound");
		public readonly string TooltipContainer;
		public readonly string TooltipTemplate = "PRODUCTION_TOOLTIP";

		// Note: LinterHotkeyNames assumes that these are disabled by default
		public readonly string HotkeyPrefix = null;
		public readonly int HotkeyCount = 0;
		public readonly HotkeyReference SelectProductionBuildingHotkey = new HotkeyReference();

		public int DisplayedIconCount { get; private set; }
		public int TotalIconCount { get; protected set; }
		public event Action<int, int> OnIconCountChanged = (a, b) => { };

		public BaseProductionIcon TooltipIcon { get; private set; }
		public Func<BaseProductionIcon> GetTooltipIcon;
		public readonly World World;
		readonly ModData modData;
		protected readonly OrderManager orderManager;

		public int MinimumRows = 4;
		public int MaximumRows = int.MaxValue;

		public int IconRowOffset = 0;
		public int MaxIconRowOffset = int.MaxValue;

		readonly Lazy<TooltipContainerWidget> tooltipContainer;
		TProductionPaletteProvider currentProvider;
		HotkeyReference[] hotkeys;

		public virtual TProductionPaletteProvider ProductionPaletteProvider
		{
			get => currentProvider;
			set
			{
				currentProvider = value;
				if (currentProvider != null)
					currentProvider.RefreshIcons();

				RefreshIcons();
			}
		}

		public override Rectangle EventBounds => eventBounds;
		Rectangle eventBounds = Rectangle.Empty;
		protected Dictionary<Rectangle, TIconItem> icons = new Dictionary<Rectangle, TIconItem>();

		protected readonly WorldRenderer worldRenderer;

		protected float2 iconOffset;

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
		public BaseProductionPaletteWidget(ModData modData, OrderManager orderManager, World world, WorldRenderer worldRenderer)
		{
			this.modData = modData;
			this.orderManager = orderManager;
			World = world;
			this.worldRenderer = worldRenderer;
			GetTooltipIcon = () => TooltipIcon;
			tooltipContainer = Exts.Lazy(() =>
				Ui.Root.Get<TooltipContainerWidget>(TooltipContainer));
		}

		public override void Initialize(WidgetArgs args)
		{
			base.Initialize(args);

			hotkeys = Exts.MakeArray(HotkeyCount,
				i => modData.Hotkeys[HotkeyPrefix + (i + 1).ToString("D2")]);

			iconOffset = 0.5f * IconSize.ToFloat2() + IconSpriteOffset;
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

		public override void Tick()
		{
		}

		public override void MouseEntered()
		{
			if (TooltipContainer != null)
				tooltipContainer.Value.SetTooltip(TooltipTemplate,
					new WidgetArgs() { { "player", World.LocalPlayer }, { "getTooltipIcon", GetTooltipIcon }, { "world", World } });
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

			if (mi.Event == MouseInputEvent.Move)
				TooltipIcon = icon;

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

			return HandleEvent(icon, mi.Button, mi.Modifiers);
		}

		protected virtual bool HandleLeftClick(ProductionItem item, TIconItem icon, int handleCount, Modifiers modifiers)
		{
			return true;
		}

		protected virtual bool HandleRightClick(ProductionItem item, TIconItem icon, int handleCount)
		{
			return true;
		}

		protected virtual bool HandleMiddleClick(ProductionItem item, TIconItem icon, int handleCount)
		{
			return true;
		}

		protected virtual bool HandleEvent(TIconItem icon, MouseButton btn, Modifiers modifiers)
		{
			return true;
		}

		public override bool HandleKeyPress(KeyInput e)
		{
			if (e.Event == KeyInputEvent.Up)
				return false;

			var batchModifiers = e.Modifiers.HasModifier(Modifiers.Shift) ? Modifiers.Shift : Modifiers.None;

			// HACK: enable production if the shift key is pressed
			e.Modifiers &= ~Modifiers.Shift;
			var toBuild = icons.Values.FirstOrDefault(i => i.Hotkey != null && i.Hotkey.IsActivatedBy(e));
			return toBuild != null && HandleEvent(toBuild, MouseButton.Left, batchModifiers);
		}

		public virtual void RefreshIcons()
		{
			icons = new Dictionary<Rectangle, TIconItem>();
			if (ProductionPaletteProvider == null)
			{
				if (DisplayedIconCount != 0)
				{
					OnIconCountChanged(DisplayedIconCount, 0);
					DisplayedIconCount = 0;
				}

				return;
			}

			var oldIconCount = DisplayedIconCount;
			DisplayedIconCount = 0;

			var rb = RenderBounds;
			foreach (var item in ProductionPaletteProvider.Icons)
			{
				var x = DisplayedIconCount % Columns;
				var y = DisplayedIconCount / Columns;
				var rect = new Rectangle(rb.X + x * (IconSize.X + IconMargin.X), rb.Y + y * (IconSize.Y + IconMargin.Y), IconSize.X, IconSize.Y);

				var pi = new TIconItem()
				{
					Actor = item.Actor,
					Name = item.Name,
					Hotkey = DisplayedIconCount < HotkeyCount ? hotkeys[DisplayedIconCount] : null,
					Sprite = item.Sprite,
					Palette = worldRenderer.Palette(item.Palette),
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
			// Icons
			Game.Renderer.EnableAntialiasingFilter();
			foreach (var icon in icons.Values)
			{
				WidgetUtils.DrawSpriteCentered(icon.Sprite, icon.Palette, icon.Pos + iconOffset);
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
