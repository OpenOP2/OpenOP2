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

using System;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	[Desc("Places a resource tile to the west of the building when it is created.")]
	public class PlacesResourcesWhenCreatedInfo : ConditionalTraitInfo
	{
		[Desc("The type of resource to place.")]
		public string ResourceType = string.Empty;

		[Desc("The resource density to set.")]
		public int Amount = 1;
		public override object Create(ActorInitializer init) { return new PlacesResourcesWhenCreated(init.Self, this); }
	}

	public class PlacesResourcesWhenCreated : ConditionalTrait<PlacesResourcesWhenCreatedInfo>, INotifyRemovedFromWorld
	{
		readonly PlacesResourcesWhenCreatedInfo info;
		readonly IResourceLayer resourceLayer;
		readonly IResourceLayer resourceType;
		CPos deployLocation;

		public PlacesResourcesWhenCreated(Actor self, PlacesResourcesWhenCreatedInfo info)
			: base(info)
		{
			this.info = info;
			resourceLayer = self.World.WorldActor.Trait<ResourceLayer>();
			var resourceTypes = self.World.WorldActor.TraitsImplementing<IResourceLayer>().ToArray();
			resourceType = resourceTypes.FirstOrDefault(a =>
				string.Equals(info.ResourceType, info.ResourceType, StringComparison.InvariantCultureIgnoreCase));

			if (resourceType == null)
				throw new ArgumentException($"Couldn't find resource type: {info.ResourceType}");
		}

		protected override void Created(Actor self)
		{
			deployLocation = self.World.Map.CellContaining(self.CenterPosition) + new CVec(-1, 0);
			resourceLayer.AddResource(info.ResourceType, deployLocation, Info.Amount);
		}

		void INotifyRemovedFromWorld.RemovedFromWorld(Actor self)
		{
			resourceLayer.ClearResources(deployLocation);
		}
	}
}
