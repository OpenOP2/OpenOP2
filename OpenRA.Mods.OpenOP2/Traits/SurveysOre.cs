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

using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Turns an unsurveyed ore node into a mineable node.")]
	public class SurveysOreInfo : ConditionalTraitInfo
	{
		[Desc("How often to check for unsuveryed ore.")]
		public int UpdateEvery = 1;

		[Desc("Amount of ore to place when surveyed.")]
		public int Amount = 120000;

		[Desc("Notification sound to play when surveying completed.")]
		public string PlaysNotification = string.Empty;

		public override object Create(ActorInitializer init) { return new SurveysOre(init.Self, this); }

		[FieldLoader.LoadUsing(nameof(LoadReplacements))]
		public Dictionary<string, OreSurveyReplacementInfo> Replacements;

		static object LoadReplacements(MiniYaml yaml)
		{
			var retList = new Dictionary<string, OreSurveyReplacementInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "Replacements");
			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("Replacement", StringComparison.InvariantCulture)))
			{
				var ret = new OreSurveyReplacementInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}
	}

	public class OreSurveyReplacementInfo
	{
		[Desc("The type of resource to remove when surveying.")]
		public string RemoveResourceType = string.Empty;

		[Desc("The type of resource to place when surveying.")]
		public string PlaceResourceType = string.Empty;
	}

	public class SurveysOre : ConditionalTrait<SurveysOreInfo>, ITick
	{
		CPos lastPosition;
		readonly World world;
		Actor actor;
		readonly SurveysOreInfo info;
		readonly IResourceLayer resourceLayer;
		public SurveysOre(Actor self, SurveysOreInfo info)
			: base(info)
		{
			actor = self;
			world = self.World;
			this.info = info;
			resourceLayer = self.World.WorldActor.Trait<ResourceLayer>();
		}

		protected override void Created(Actor self)
		{
			base.Created(self);
			lastPosition = self.Location;
		}

		void UpdateCellsIfMoved(Actor self)
		{
			if (lastPosition != actor.Location)
			{
				lastPosition = actor.Location;

				var resource = resourceLayer.GetResource(lastPosition);
				foreach (var replacement in info.Replacements)
				{
					if (resource.Type == replacement.Value.RemoveResourceType)
					{
						resourceLayer.RemoveResource(replacement.Value.RemoveResourceType, lastPosition);
						resourceLayer.AddResource(replacement.Value.PlaceResourceType, lastPosition, Info.Amount);
						Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech", info.PlaysNotification, self.Owner.Faction.InternalName);
						break;
					}
				}
			}
		}

		void ITick.Tick(Actor self)
		{
			actor = self;

			if (IsTraitDisabled)
			{
				return;
			}

			if (world.WorldTick % info.UpdateEvery == 0)
			{
				UpdateCellsIfMoved(self);
			}
		}

		protected override void TraitEnabled(Actor self)
		{
			base.TraitEnabled(self);

			lastPosition = actor.Location;
			UpdateCellsIfMoved(self);
		}
	}
}
