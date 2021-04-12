namespace OpenRA.Mods.OpenOP2.UtilityCommands
{
	public class GroupSequenceSet
	{
		public int Offset;
		public int Length;
		public string Sequence { get; set; }
	}

	public enum ActorType
	{
		Vehicle,
		Building,
		Effect,
		Decoration,
		PaletteEight,
	}

	public class GroupSequence
	{
		public string Name;
		public ActorType ActorType;
		public GroupSequenceSet[] Sets;
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
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "cargo-truck",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 8,
						Length = 16,
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 24,
						Length = 16,
						Sequence = "idle-full"
					},
				}
			},
			new GroupSequence()
			{
				Name = "smokingcrater",
				ActorType = ActorType.Decoration,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 58,
						Length = 1,
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 59,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 60,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 69,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 61,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 62,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 70,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 803,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1846,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 63,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 64,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 71,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 65,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 66,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 72,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1857,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 67,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 68,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 74,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 82,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 83,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 138,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 84,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 85,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 140,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 86,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 87,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 139,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1861,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 88,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 89,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 143,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 90,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 91,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 148,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 613,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1849,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 92,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 93,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 150,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1852,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 94,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 95,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 149,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1859,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 96,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 97,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 144,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 98,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 99,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 154,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 653,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 351, // strange offset
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 100,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 101,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 151,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 102,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 103,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 354,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 623,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 106,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 107,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 152,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 108,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 109,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 142,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 111,
						Length = 1,
						Sequence = "damaged"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 601,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 112,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 113,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 153,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 670,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 114,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 115,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 141,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 649,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 116,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 117,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 155,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 118,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 119,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 145,
						Length = 1,
						Sequence = "make"
					},
				}
			},
			new GroupSequence()
			{
				Name = "eden-nursery",
				ActorType = ActorType.Building,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 596,
						Length = 1,
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 597,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 598,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 120,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 121,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 595,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 615,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 616,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 1843,
						Length = 1,
						Sequence = "idle4"
					},
					new GroupSequenceSet()
					{
						Offset = 122,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 123,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 146,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 611,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1864,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 124,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 125,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 77,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 650,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 651,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 1858,
						Length = 1,
						Sequence = "idle4"
					},
					new GroupSequenceSet()
					{
						Offset = 126,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 127,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 78,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1008,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1855,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 128,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 129,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 79,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 130,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 131,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 80,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 132,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 133,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 76,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 668,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 134,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 135,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 147,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 2064,
						Length = 1,
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 136,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 137,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 75,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 156,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 157,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 158,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 160,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 161,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 159,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 644,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 163,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 164,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 162,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1735,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1736,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 166,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 167,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 165,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 618,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 619,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 1850,
						Length = 1,
						Sequence = "idle4"
					},
					new GroupSequenceSet()
					{
						Offset = 169,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 170,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 168,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 641,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1777,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 172,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 173,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 171,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 2063,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1851,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 175,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 176,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 174,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 180,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 178,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 608,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 609,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 181,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 182,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 607,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 937,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 184,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 185,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 183,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 188,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 186,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 190,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 191,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 189,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 366,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 367,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 365,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 368,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 370,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 371,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 369,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 372,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1738,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 379,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 380,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 378,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 381,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 384,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 382,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 385,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1838,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 387,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 388,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 386,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 389,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1837,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 362,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 361,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 363,
						Length = 1,
						Sequence = "make"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 391,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 392,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 390,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 393,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 939,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 2062,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 395,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 396,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 394,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 397,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 655,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 656,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 399,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 400,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 398,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 401,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 964,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 1844,
						Length = 1,
						Sequence = "idle3"
					},
					new GroupSequenceSet()
					{
						Offset = 402,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 403,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 425,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 404,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1002,
						Length = 1,
						Sequence = "damaged"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 408,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 405,
						Length = 1,
						Sequence = "build2"
					},
					new GroupSequenceSet()
					{
						Offset = 406,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 409,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 411,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 412,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 1863,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 414,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 415,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 413,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 416,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 621,
						Length = 1,
						Sequence = "idle2"
					},
					new GroupSequenceSet()
					{
						Offset = 418,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 419,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 417,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 420,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 422,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 423,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 421,
						Length = 1,
						Sequence = "make"
					},
					new GroupSequenceSet()
					{
						Offset = 424,
						Length = 1,
						Sequence = "die"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 428,
						Length = 1,
						Sequence = "damaged"
					},
				}
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
						Sequence = "idle"
					},
					new GroupSequenceSet()
					{
						Offset = 437,
						Length = 1,
						Sequence = "damaged"
					},
					new GroupSequenceSet()
					{
						Offset = 438,
						Length = 1,
						Sequence = "damaged2"
					},
					new GroupSequenceSet()
					{
						Offset = 439,
						Length = 1,
						Sequence = "die"
					},
					new GroupSequenceSet()
					{
						Offset = 440,
						Length = 1,
						Sequence = "make"
					},
				}
			},

			// Collapsing buildings 192-229
			new GroupSequence()
			{
				Name = "explosion1",
				ActorType = ActorType.Effect,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 230,
						Length = 1,
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "explosion2",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 460,
						Length = 16,
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "eden-robo-surveyor",
				ActorType = ActorType.Vehicle,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 248,
						Length = 8,
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
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
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "rallypoint",
				ActorType = ActorType.Decoration,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1759,
						Length = 1,
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "waypoint",
				ActorType = ActorType.Decoration,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1856,
						Length = 1,
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "mpspawn",
				ActorType = ActorType.Decoration,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 1764,
						Length = 1,
						Sequence = "idle"
					},
				}
			},
			new GroupSequence()
			{
				Name = "clock",
				ActorType = ActorType.Decoration,
				Sets = new[]
				{
					new GroupSequenceSet()
					{
						Offset = 804,
						Length = 1,
						Sequence = "idle"
					},
				}
			},
		};
	}
}
