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
		public int? Tick { get; set; }
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

		/// <summary>
		/// OP2 have a "busy" animation in the same group as the standard building idle frame.
		/// This will split out an "idle" sequence using just the first frame and do the rest of the animation in "idle2".
		/// </summary>
		public bool WithSingleFrameIdle { get; set; }
	}
}
