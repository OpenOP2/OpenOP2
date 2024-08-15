using OpenRA.Graphics;

namespace OpenRA.Mods.OpenOP2.Interfaces
{
	/// <inheritdoc/>
	public interface IOp2SpriteSequence : ISpriteSequence
	{
		/// <summary>
		/// Gets Initial facing offset.
		/// </summary>
		int FacingOffset { get; }
	}
}
