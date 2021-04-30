namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	public class GroupSequenceSet
	{
		/// <summary>
		/// Start ID of the group index.
		/// </summary>
		public int Start { get; set; }

		/// <summary>
		/// The number of groups to include in the sequence.
		/// Each group is typically a different orientation.
		/// </summary>
		public int Length { get; set; } = 1;

		/// <summary>
		/// Start from this group offset.
		/// Groups will be read from the Start again after the last group.
		/// </summary>
		public int StartOffset { get; set; } = 0;

		/// <summary>
		/// The name of the sequence to write to file.
		/// </summary>
		public string Sequence { get; set; }

		public int OffsetX { get; set; } = 0;
		public int OffsetY { get; set; } = 0;
		public int OffsetZ { get; set; } = 0;
	}

	public enum ActorType
	{
		Vehicle,
		Building,
		Effect,
		Decoration,
		Turret,
	}

	public class GroupSequence
	{
		public string Name;
		public ActorType ActorType;
		public GroupSequenceSet[] Sets;
		public bool CreateBaseActor = true;
		public bool CreateExampleActor = true;
		public bool WithBlankIdle = false;
	}

	public static class SequencesList
	{
		public static GroupSequence[] GroupSequenceSets => new[]
		{
			new GroupSequence()
			{
				Name = "eden-robo-dozer",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 0,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1665,
						Length = 8,
						Sequence = "doze",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 0,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robo-dozer",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 343,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1673,
						Length = 8,
						Sequence = "doze",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 343,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-cargo-truck",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 8,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 8,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cargo-truck",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 24,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 24,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-convec",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 287,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1695,
						Length = 1,
						Sequence = "build",
					},
					new GroupSequenceSet()
					{
						Start = 1719,
						Length = 8,
						Sequence = "repair",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-convec",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 319,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1696,
						Length = 1,
						Sequence = "build",
					},
					new GroupSequenceSet()
					{
						Start = 1727,
						Length = 8,
						Sequence = "repair",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-evacuation-transport",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 303,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-evacuation-transport",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 263,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-earthworker",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1811, // TODO: directions are all out of wack
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1146,
						Length = 8,
						Sequence = "idle2",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1703,
						Length = 8,
						Sequence = "build",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-earthworker",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1819, // TODO: directions are all out of wack
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1827,
						Length = 8,
						Sequence = "idle2",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1711,
						Length = 8,
						Sequence = "build",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-scout",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 442,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-scout",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 460,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-robo-surveyor",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 247,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 1122,
						Length = 8,
						Sequence = "survey",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robo-surveyor",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 255,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
					new GroupSequenceSet()
					{
						Start = 2025,
						Length = 8,
						Sequence = "survey",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-robo-miner",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1170,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1336,
						Length = 1,
						Sequence = "make-dildo",
					},
					new GroupSequenceSet()
					{
						Start = 1170,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robo-miner",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1186,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1186,
						Length = 1,
						Sequence = "icon",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-tiger",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 231,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-tiger",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 279,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-panther",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 239,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-panther",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 335,
						Length = 8,
						Sequence = "idle",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-lynx",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1202,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1202,
						Length = 1,
						Sequence = "icon",
					},
					new GroupSequenceSet()
					{
						Start = 1270,
						Length = 16,
						Sequence = "turret2",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-lynx",
				ActorType = ActorType.Vehicle,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1227,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1227,
						Length = 1,
						Sequence = "icon",
					},
					new GroupSequenceSet()
					{
						Start = 1302,
						Length = 16,
						Sequence = "turret2",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-flyer",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1779,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-flyer",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1795,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-robo-repair",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1130,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1625,
						Length = 16,
						Sequence = "repair",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-only",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1154,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-arachnid1",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1633,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
					new GroupSequenceSet()
					{
						Start = 1681,
						Length = 8,
						Sequence = "attack",
						StartOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-arachnid2",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1649,
						Length = 16,
						Sequence = "idle",
						StartOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "smokingcrater",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 58,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-command-center",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1845,
						Length = 1,
						Sequence = "idle",
						OffsetX = 1,
					},
					new GroupSequenceSet()
					{
						Start = 61,
						Length = 1,
						Sequence = "damaged",
						OffsetX = 1,
					},
					new GroupSequenceSet()
					{
						Start = 62,
						Length = 1,
						Sequence = "damaged2",
						OffsetX = 1,
					},
					new GroupSequenceSet()
					{
						Start = 70,
						Length = 1,
						Sequence = "make",
						OffsetX = 1,
					},
					new GroupSequenceSet()
					{
						Start = 194,
						Length = 1,
						Sequence = "die",
						OffsetX = 1,
					},
					new GroupSequenceSet()
					{
						Start = 1108,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-command-center",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1854,
						Length = 1,
						Sequence = "idle",
						OffsetY = 1,
					},
					new GroupSequenceSet()
					{
						Start = 84,
						Length = 1,
						Sequence = "damaged",
						OffsetY = 1,
					},
					new GroupSequenceSet()
					{
						Start = 85,
						Length = 1,
						Sequence = "damaged2",
						OffsetY = 1,
					},
					new GroupSequenceSet()
					{
						Start = 140,
						Length = 1,
						Sequence = "make",
						OffsetY = 1,
					},
					new GroupSequenceSet()
					{
						Start = 211,
						Length = 1,
						Sequence = "die",
						OffsetY = 1,
					},
					new GroupSequenceSet()
					{
						Start = 1108,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-tokamak",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2035,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 132,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 133,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 76,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 373,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1100,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-tokamak",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 652,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 653,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 351, // strange offset
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 100,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 101,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 151,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 359,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1100,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-residence",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 610,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 611,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1864,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 124,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 125,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 77,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 201,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-residence",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 612,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 613,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1849,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 92,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 93,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 150,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 214,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-factory-structure",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 73,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 65,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 66,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 72,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 196,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-factory-structure",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1860,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1861,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 88,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 89,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 143,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 212,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-agridome",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 801,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 108,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 109,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 142,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 199,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-agridome",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 646,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 82,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 83,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 138,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 210,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-factory-vehicle",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 807,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 2064,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 136,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 137,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 75,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 200,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1106,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-factory-vehicle",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1853,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 102,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 103,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 354,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 213,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1106,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-dirt",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 802,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 803,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1846,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 63,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 64,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 71,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 195,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-dirt",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1862,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 86,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 87,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 139,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 218,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-lab-basic",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 642,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1857,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 67,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 68,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 74,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 197,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-lab-basic",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2034,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 90,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 91,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 148,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 216,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-robot-command-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 647,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 650,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 651,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 1858,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Start = 126,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 127,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 78,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 202,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robot-command-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 645,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1852,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 94,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 95,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 149,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 215,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-smelter-common",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1007,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1008,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1855,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 128,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 129,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 79,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 203,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1109,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-smelter-common",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1009,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1859,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 96,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 97,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 144,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 219,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 1109,
						Length = 1,
						Sequence = "icon",
						OffsetX = -23,
						OffsetY = -23,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-storage-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2059,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 59,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 60,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 69,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 193,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-storage-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2061,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 98,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 99,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 154,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 223,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-storage-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2060,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 391,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 392,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 390,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 393,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-storage-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2062,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 395,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 396,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 394,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 397,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-lab-standard",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 669,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 670,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 114,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 115,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 141,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 192,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-lab-standard",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 671,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1735,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1736,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 166,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 167,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 165,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 220,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-lab-advanced",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 600,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 601,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 112,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 113,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 153,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 209,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-lab-advanced",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 643,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 644,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 163,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 164,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 162,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 227,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-residence-advanced",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 622,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 623,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 106,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 107,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 152,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 198,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-residence-reinforced",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 640,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 641,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1777,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 172,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 173,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 171,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 225,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-medical-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 602,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 118,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 119,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 145,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 207,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-medical-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 620,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 621,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 418,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 419,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 417,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 420,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-nursery",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 598,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 596,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 597,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 120,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 121,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 595,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 204,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-nursery",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 606,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 608,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 609,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 181,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 182,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 607,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 364,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-recreation-facility",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 614,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 615,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 616,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 1843,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Start = 122,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 123,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 146,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 208,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-recreation-facility",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 617,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 618,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 619,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 1850,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Start = 169,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 170,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 168,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 226,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-solar-power-array",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 941,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 130,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 131,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 80,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 374,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-solar-power-array",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 942,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 2063,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1851,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 175,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Start = 176,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 177,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 174,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 360,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-university",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 667,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 668,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 134,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 135,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 147,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 205,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-university",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 936,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 937,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 184,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 185,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 183,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 222,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-smelter-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1841,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 370,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 371,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 369,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 372,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-smelter-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1010,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 190,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 191,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 189,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 229,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-gorf",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 383,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 384,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 382,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 385,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-gorf",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1737,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1738,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 379,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 380,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 378,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 381,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-trade-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 800,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1838,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 387,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 388,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 386,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 389,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-trade-center",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 946,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1837,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 362,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 361,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 363,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 221,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-spaceport",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 963,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 964,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 1844,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 402,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 403,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 425,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 404,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-spaceport",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 965,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 437,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 438,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 439,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Start = 440,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-mine-common",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1001,
						Length = 1,
						Sequence = "idle",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 1002,
						Length = 1,
						Sequence = "damaged",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 1335,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 1003,
						Length = 1,
						Sequence = "die",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 1762,
						Length = 1,
						Sequence = "deck",
						OffsetZ = -1024,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-mine-common",
				ActorType = ActorType.Building,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1848,
						Length = 1,
						Sequence = "idle",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 998,
						Length = 1,
						Sequence = "damaged",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 999,
						Length = 1,
						Sequence = "damaged2",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 375,
						Length = 1,
						Sequence = "make",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 1000,
						Length = 1,
						Sequence = "die",
						OffsetX = -16,
						OffsetY = 6,
					},
					new GroupSequenceSet()
					{
						Start = 1762,
						Length = 1,
						Sequence = "deck",
						OffsetZ = -1024,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-mine-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 407,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 408,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 405,
						Length = 1,
						Sequence = "build2",
					},
					new GroupSequenceSet()
					{
						Start = 406,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 409,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-mine-rare",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 594,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 422,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 423,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 421,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 424,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-mine-magma",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 410,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1337,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 411,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 412,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-mhd-generator",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 940,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 1863,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 414,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 415,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 413,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 416,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-meteor-defense",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 564,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 565,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 566,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 568,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 569,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 567,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 570,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-geothermal-plant",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 427,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 428,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 426,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 429,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-factory-arachnid",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1847,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 156,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 157,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 158,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 217,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-forum",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2033,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 160,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 161,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 159,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 228,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-factory-consumer",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1839,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 366,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 367,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 365,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 368,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-observatory",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 654,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 655,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 656,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Start = 399,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 400,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 398,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 401,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-light-tower",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 648,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 649,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Start = 116,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 117,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Start = 155,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 206,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-light-tower",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 179,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 180,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 178,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 224,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-light-tower2",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 187,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 188,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Start = 186,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Start = 224,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-unknown-turret",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 110,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 111,
						Length = 1,
						Sequence = "damaged",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-unknown-turret2",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1623,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-unknown-turret2-double",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1624,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-unknown-turret2-double",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1621,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-unknown-turret3",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1342,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-unknown-turret3-double",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1622,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-unknown-turret4",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 938,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Start = 939,
						Length = 1,
						Sequence = "fire",
					},
				},
			},

			// Collapsing buildings 192-229
			new GroupSequence()
			{
				Name = "die1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 571,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "die2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 572,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 230,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 603,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 604,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff3",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 605,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 657,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 659,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring-hover",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 660,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring-dissipate",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 661,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion3",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 662,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion4",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 663,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion5",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2071,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion6",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2072,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "airburst",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2067,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 664,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 665,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 943,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large-hover",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 944,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large-dissipate",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 945,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1360,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning-crackle",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 666,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-billowing",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1004,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1005,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1006,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1027,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-ignite",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1028,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large-ignite",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1029,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor2-ignite",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1030,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-splash",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1358,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large-splash",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1359,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1047,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-small",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1334,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1699,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1700,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large3",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1701,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-huge",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1702,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo3",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1048,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-forming",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1113,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-dissipating",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1114,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1115,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1116,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-left",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1117,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-right",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1118,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground-form",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1119,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1120,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground-dissipate",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1121,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-form",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1244,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1245,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1246,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-expand-left",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1247,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow-left",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1248,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate-left",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1249,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-expand-right",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1250,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow-right",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1251,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate-right",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1252,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning1",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1253,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning2",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1361,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning3",
				ActorType = ActorType.Effect,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1362,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "rallypoint",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1764,
						Length = 1,
						Sequence = "flag",
					},
					new GroupSequenceSet()
					{
						Start = 1764,
						Length = 1,
						Sequence = "circles",
					},
				},
			},
			new GroupSequence()
			{
				Name = "waypoint",
				ActorType = ActorType.Decoration,
				CreateExampleActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1856,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mpspawn",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					// Alt mpspawn: 1764, 1111, 1112, 1107
					new GroupSequenceSet()
					{
						Start = 1107,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "clock",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					// Alt clocks: 658, 599
					new GroupSequenceSet()
					{
						Start = 804,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-1",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 624,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-elec",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 947,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-laser",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 966,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-machinegun",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 982,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-2",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1011,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-3",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1031,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-4",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1068,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-5",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1084,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-missile",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1254,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1286,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-small",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1302,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1318,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-machinegun-small",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1363,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-machinegun-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1379,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-small",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1395,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1411,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-medium",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1427,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-medium-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1443,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-missile-medium",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1461,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-missile-medium-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1477,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-unknown",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1493,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-unknown-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1509,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-laser-turret-small",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1525,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-laser-turret-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1541,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-elec-turret-small",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1557,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-elec-turret-small-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1573,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-cannon",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1589,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-cannon-double",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1605,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-5",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 2043,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lava-ripple",
				ActorType = ActorType.Turret,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1460,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tokamak-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1100,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "medical-center-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1101,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "seismology-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1102,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "rocket-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1103,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "nuke-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1104,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "agridome-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1105,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "vehicle-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1106,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "residence-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1107,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "structure-factory-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1108,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "smelter-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1109,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lab-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1110,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1111,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-icon",
				ActorType = ActorType.Decoration,
				CreateBaseActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Start = 1112,
						Length = 1,
						Sequence = "idle",
					},
				},
			},

			// new GroupSequence()
			// {
			// 	Name = "ore",
			// 	ActorType = ActorType.Decoration,
			// 	CreateExampleActor = false,
			// 	Sets = new[]
			// 	{
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1755,
			// 			Length = 1,
			// 			Sequence = "ore"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1754,
			// 			Length = 1,
			// 			Sequence = "ore-1-high"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1753,
			// 			Length = 1,
			// 			Sequence = "ore-1-medium"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1752,
			// 			Length = 1,
			// 			Sequence = "ore-1-low"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1751,
			// 			Length = 1,
			// 			Sequence = "ore-2-high"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1750,
			// 			Length = 1,
			// 			Sequence = "ore-2-medium"
			// 		},
			// 		new GroupSequenceSet()
			// 		{
			// 			Offset = 1749,
			// 			Length = 1,
			// 			Sequence = "ore-2-low"
			// 		},
			// 	}
			// },
		};
	}
}
