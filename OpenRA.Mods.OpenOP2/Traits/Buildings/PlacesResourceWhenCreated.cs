#region Copyright & License Information
/*
 * Copyright 2007-2020 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Places a resource tile to the west of the building when it is created.")]
	public class PlacesResourcesWhenCreatedInfo : ConditionalTraitInfo
	{
		public override object Create(ActorInitializer init) { return new PlacesResourcesWhenCreated(init.Self, this); }

		[FieldLoader.LoadUsing("LoadReplacements")]
		public Dictionary<string, MinerNodeResourceReplacementInfo> Replacements;

		static object LoadReplacements(MiniYaml yaml)
		{
			var retList = new Dictionary<string, MinerNodeResourceReplacementInfo>();
			var replacements = yaml.Nodes.First(x => x.Key == "Replacements");
			foreach (var node in replacements.Value.Nodes.Where(n => n.Key.StartsWith("Replacement")))
			{
				var ret = new MinerNodeResourceReplacementInfo();
				FieldLoader.Load(ret, node.Value);
				retList.Add(node.Key, ret);
			}

			return retList;
		}
	}

	public class MinerNodeResourceReplacementInfo
	{
		[Desc("The type of resource to remove when surveying.")]
		public string RemoveResourceType = string.Empty;

		[Desc("The type of resource to place when surveying.")]
		public string PlaceResourceType = string.Empty;

		[Desc("The resource density to set.")]
		public int Amount = 1;
	}

	public class PlacesResourcesWhenCreated : ConditionalTrait<PlacesResourcesWhenCreatedInfo>, INotifyRemovedFromWorld
	{
		readonly PlacesResourcesWhenCreatedInfo info;
		readonly IResourceLayer resourceLayer;
		CPos deployLocation;
		string removedResourceType;
		int removedResourceAmount;

		public PlacesResourcesWhenCreated(Actor self, PlacesResourcesWhenCreatedInfo info)
			: base(info)
		{
			this.info = info;
			resourceLayer = self.World.WorldActor.Trait<ResourceLayer>();
		}

		protected override void Created(Actor self)
		{
			var selfLocation = self.World.Map.CellContaining(self.CenterPosition);
			deployLocation = selfLocation + new CVec(-1, 0);
			var thisResource = resourceLayer.GetResource(selfLocation);
			foreach (var replacement in info.Replacements)
			{
				if (thisResource.Type == replacement.Value.RemoveResourceType)
				{
					removedResourceType = replacement.Value.RemoveResourceType;
					removedResourceAmount = thisResource.Density;

					resourceLayer.RemoveResource(removedResourceType, selfLocation);
					resourceLayer.AddResource(replacement.Value.PlaceResourceType, deployLocation, replacement.Value.Amount);
					break;
				}
			}
		}

		void INotifyRemovedFromWorld.RemovedFromWorld(Actor self)
		{
			resourceLayer.ClearResources(deployLocation);
			resourceLayer.AddResource(removedResourceType, self.World.Map.CellContaining(self.CenterPosition), removedResourceAmount);
		}
	}
}
