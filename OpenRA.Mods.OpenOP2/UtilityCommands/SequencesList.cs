namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	public class GroupSequenceSet
	{
		public int Offset;
		public int Length;
		public int LoopOffset = 0;

		public string Sequence { get; set; }
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
		public bool CreateActor = true;
		public bool CreateExampleActor = true;
	}

	public static class SequencesList
	{
		public static GroupSequence[] GroupSequenceSets => new[]
		{
			new GroupSequence()
			{
				Name = "eden-robo-dozer",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 0,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1665,
						Length = 8,
						Sequence = "doze",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robo-dozer",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 343,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1673,
						Length = 8,
						Sequence = "doze",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-cargo-truck",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 8,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cargo-truck",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 24,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 287,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
					new GroupSequenceSet()
					{
						Offset = 1695,
						Length = 1,
						Sequence = "build",
					},
					new GroupSequenceSet()
					{
						Offset = 1719,
						Length = 8,
						Sequence = "repair",
						LoopOffset = 2,
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
						Offset = 319,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
					new GroupSequenceSet()
					{
						Offset = 1696,
						Length = 1,
						Sequence = "build",
					},
					new GroupSequenceSet()
					{
						Offset = 1727,
						Length = 8,
						Sequence = "repair",
						LoopOffset = 2,
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
						Offset = 303,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 263,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 1811, // TODO: directions are all out of wack
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1146,
						Length = 8,
						Sequence = "idle2",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1703,
						Length = 8,
						Sequence = "build",
						LoopOffset = 2,
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
						Offset = 1819, // TODO: directions are all out of wack
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1827,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1711,
						Length = 8,
						Sequence = "build",
						LoopOffset = 2,
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
						Offset = 442,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 460,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 247,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 1122,
						Length = 8,
						Sequence = "survey",
						LoopOffset = 2,
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
						Offset = 255,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
					new GroupSequenceSet()
					{
						Offset = 2025,
						Length = 8,
						Sequence = "survey",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-robo-miner",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1170,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
					new GroupSequenceSet()
					{
						Offset = 1335,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 1336,
						Length = 1,
						Sequence = "make-dildo",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-robo-miner",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1186,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-combat-chassis-heavy",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 231,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-combat-chassis-heavy",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 279,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-combat-chassis-medium",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 239,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-combat-chassis-medium",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 335,
						Length = 8,
						Sequence = "idle",
						LoopOffset = 2,
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-chassis-unknown",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1202,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-chassis-unknown",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1227,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 1779,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 1795,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 1130,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
					new GroupSequenceSet()
					{
						Offset = 1625,
						Length = 16,
						Sequence = "repair",
						LoopOffset = 4,
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
						Offset = 1154,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
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
						Offset = 1633,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
					new GroupSequenceSet()
					{
						Offset = 1681,
						Length = 8,
						Sequence = "attack",
						LoopOffset = 2,
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
						Offset = 1649,
						Length = 16,
						Sequence = "idle",
						LoopOffset = 4,
					},
				},
			},
			new GroupSequence()
			{
				Name = "smokingcrater",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 58,
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
						Offset = 1845,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 61,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 62,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 70,
						Length = 1,
						Sequence = "make",
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
						Offset = 1854,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 84,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 85,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 140,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-tokamak",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 2035,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 132,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 133,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 76,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-tokamak",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 652,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 653,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 351, // strange offset
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 100,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 101,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 151,
						Length = 1,
						Sequence = "make",
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
						Offset = 610,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 611,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1864,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 124,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 125,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 77,
						Length = 1,
						Sequence = "make",
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
						Offset = 612,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 613,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1849,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 92,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 93,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 150,
						Length = 1,
						Sequence = "make",
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
						Offset = 73,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 65,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 66,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 72,
						Length = 1,
						Sequence = "make",
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
						Offset = 1860,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1861,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 88,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 89,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 143,
						Length = 1,
						Sequence = "make",
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
						Offset = 801,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 108,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 109,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 142,
						Length = 1,
						Sequence = "make",
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
						Offset = 646,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 82,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 83,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 138,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-factory-vehicle",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 807,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 2064,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 136,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 137,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 75,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-factory-vehicle",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1853,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 102,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 103,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 354,
						Length = 1,
						Sequence = "make",
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
						Offset = 802,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 803,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1846,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 63,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 64,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 71,
						Length = 1,
						Sequence = "make",
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
						Offset = 1862,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 86,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 87,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 139,
						Length = 1,
						Sequence = "make",
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
						Offset = 642,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1857,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 67,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 68,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 74,
						Length = 1,
						Sequence = "make",
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
						Offset = 2034,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 90,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 91,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 148,
						Length = 1,
						Sequence = "make",
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
						Offset = 647,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 650,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 651,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 1858,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Offset = 126,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 127,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 78,
						Length = 1,
						Sequence = "make",
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
						Offset = 645,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1852,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 94,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 95,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 149,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-smelter-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1007,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1008,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1855,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 128,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 129,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 79,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-smelter-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1009,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1859,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 96,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 97,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 144,
						Length = 1,
						Sequence = "make",
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
						Offset = 2059,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 59,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 60,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 69,
						Length = 1,
						Sequence = "make",
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
						Offset = 2061,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 98,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 99,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 154,
						Length = 1,
						Sequence = "make",
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
						Offset = 2060,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 391,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 392,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 390,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 393,
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
						Offset = 938,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 939,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 2062,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 395,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 396,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 394,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 397,
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
						Offset = 669,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 670,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 114,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 115,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 141,
						Length = 1,
						Sequence = "make",
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
						Offset = 671,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1735,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1736,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 166,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 167,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 165,
						Length = 1,
						Sequence = "make",
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
						Offset = 600,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 601,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 112,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 113,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 153,
						Length = 1,
						Sequence = "make",
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
						Offset = 643,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 644,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 163,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 164,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 162,
						Length = 1,
						Sequence = "make",
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
						Offset = 622,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 623,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 106,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 107,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 152,
						Length = 1,
						Sequence = "make",
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
						Offset = 640,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 641,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1777,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 172,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 173,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 171,
						Length = 1,
						Sequence = "make",
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
						Offset = 602,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 118,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 119,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 145,
						Length = 1,
						Sequence = "make",
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
						Offset = 620,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 621,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 418,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 419,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 417,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 420,
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
						Offset = 598,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 596,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 597,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 120,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 121,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 595,
						Length = 1,
						Sequence = "make",
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
						Offset = 606,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 608,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 609,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 181,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 182,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 607,
						Length = 1,
						Sequence = "make",
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
						Offset = 614,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 615,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 616,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 1843,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Offset = 122,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 123,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 146,
						Length = 1,
						Sequence = "make",
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
						Offset = 617,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 618,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 619,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 1850,
						Length = 1,
						Sequence = "idle4",
					},
					new GroupSequenceSet()
					{
						Offset = 169,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 170,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 168,
						Length = 1,
						Sequence = "make",
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
						Offset = 941,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 130,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 131,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 80,
						Length = 1,
						Sequence = "make",
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
						Offset = 942,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 2063,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1851,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 175,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 176,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 174,
						Length = 1,
						Sequence = "make",
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
						Offset = 667,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 668,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 134,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 135,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 147,
						Length = 1,
						Sequence = "make",
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
						Offset = 936,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 937,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 184,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 185,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 183,
						Length = 1,
						Sequence = "make",
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
						Offset = 1841,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 370,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 371,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 369,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 372,
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
						Offset = 1010,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 190,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 191,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 189,
						Length = 1,
						Sequence = "make",
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
						Offset = 383,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 384,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 382,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 385,
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
						Offset = 1737,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1738,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 379,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 380,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 378,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 381,
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
						Offset = 800,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1838,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 387,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 388,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 386,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 389,
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
						Offset = 946,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1837,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 362,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 361,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 363,
						Length = 1,
						Sequence = "make",
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
						Offset = 963,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 964,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 1844,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 402,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 403,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 425,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 404,
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
						Offset = 965,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 437,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 438,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 439,
						Length = 1,
						Sequence = "die",
					},
					new GroupSequenceSet()
					{
						Offset = 440,
						Length = 1,
						Sequence = "make",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-mine-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1001,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1002,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 1003,
						Length = 1,
						Sequence = "die",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-mine-common",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1848,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 998,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 999,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 421,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 1000,
						Length = 1,
						Sequence = "die",
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
						Offset = 407,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 408,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 405,
						Length = 1,
						Sequence = "build2",
					},
					new GroupSequenceSet()
					{
						Offset = 406,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 409,
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
						Offset = 594,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 422,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 423,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 421,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 424,
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
						Offset = 410,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1337,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 411,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 412,
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
						Offset = 940,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 1863,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 414,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 415,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 413,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 416,
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
						Offset = 427,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 428,
						Length = 1,
						Sequence = "damaged",
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
						Offset = 1847,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 156,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 157,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 158,
						Length = 1,
						Sequence = "make",
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
						Offset = 2033,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 160,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 161,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 159,
						Length = 1,
						Sequence = "make",
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
						Offset = 1839,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 366,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 367,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 365,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 368,
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
						Offset = 654,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 655,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 656,
						Length = 1,
						Sequence = "idle3",
					},
					new GroupSequenceSet()
					{
						Offset = 399,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 400,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 398,
						Length = 1,
						Sequence = "make",
					},
					new GroupSequenceSet()
					{
						Offset = 401,
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
						Offset = 648,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 649,
						Length = 1,
						Sequence = "idle2",
					},
					new GroupSequenceSet()
					{
						Offset = 116,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 117,
						Length = 1,
						Sequence = "damaged2",
					},
					new GroupSequenceSet()
					{
						Offset = 155,
						Length = 1,
						Sequence = "make",
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
						Offset = 179,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 180,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 178,
						Length = 1,
						Sequence = "make",
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
						Offset = 187,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 188,
						Length = 1,
						Sequence = "damaged",
					},
					new GroupSequenceSet()
					{
						Offset = 186,
						Length = 1,
						Sequence = "make",
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
						Offset = 110,
						Length = 1,
						Sequence = "idle",
					},
					new GroupSequenceSet()
					{
						Offset = 111,
						Length = 1,
						Sequence = "damaged",
					},
				},
			},

			// Collapsing buildings 192-229
			new GroupSequence()
			{
				Name = "explosion1",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 230,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff1",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 603,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 604,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "puff3",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 605,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 657,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 659,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring-hover",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 660,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "pulsing-ring-dissipate",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 661,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion3",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 662,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion4",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 663,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion5",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 2071,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion6",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 2072,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "airburst",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 2067,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 664,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 665,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 943,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large-hover",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 944,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large-dissipate",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 945,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-large2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1360,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning-crackle",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 666,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-billowing",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1004,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1005,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1006,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1027,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-ignite",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1028,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large-ignite",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1029,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor2-ignite",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1030,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-splash",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1358,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "meteor-large-splash",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1359,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1047,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-small",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1334,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large1",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1699,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1700,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-large3",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1701,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mushroom-cloud-huge",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1702,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "explosion-goo3",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1048,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-forming",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1113,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud-dissipating",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1114,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado1",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1115,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1116,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-left",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1117,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-right",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1118,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground-form",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1119,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1120,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tornado-ground-dissipate",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1121,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-form",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1244,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1245,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1246,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-expand-left",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1247,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow-left",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1248,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate-left",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1249,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-expand-right",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1250,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-billow-right",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1251,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "cloud2-dissipate-right",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1252,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning1",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1253,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning2",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1361,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lightning3",
				ActorType = ActorType.Effect,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1362,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "rallypoint",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1759,
						Length = 1,
						Sequence = "idle",
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
						Offset = 1856,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "mpspawn",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					// Alt mpspawn: 1764, 1111, 1112, 1107
					new GroupSequenceSet()
					{
						Offset = 1107,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "clock",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					// Alt clocks: 658, 599
					new GroupSequenceSet()
					{
						Offset = 804,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-1",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 624,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-elec",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 947,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-laser",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 966,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-machinegun",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 982,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-2",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1011,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-3",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1031,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-4",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1068,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-5",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1084,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-missile",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1254,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1270,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-turret-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1286,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1302,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1318,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-machinegun-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1363,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-machinegun-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1379,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1395,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1411,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-medium",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1427,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-cannon-medium-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1443,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-missile-medium",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1461,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-missile-medium-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1477,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-unknown",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1493,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-unknown-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1509,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-laser-turret-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1525,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-laser-turret-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1541,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-elec-turret-small",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1557,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-elec-turret-small-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1573,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-cannon",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1589,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-medium-cannon-double",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1605,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-turret-5",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 2043,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lava-ripple",
				ActorType = ActorType.Turret,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1460,
						Length = 16,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "tokamak-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1100,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "medical-center-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1101,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "seismology-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1102,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "rocket-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1103,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "nuke-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1104,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "agridome-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1105,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "vehicle-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1106,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "residence-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1107,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "structure-factory-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1108,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "smelter-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1109,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "lab-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1110,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "eden-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1111,
						Length = 1,
						Sequence = "idle",
					},
				},
			},
			new GroupSequence()
			{
				Name = "plymouth-icon",
				ActorType = ActorType.Decoration,
				CreateActor = false,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1112,
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
