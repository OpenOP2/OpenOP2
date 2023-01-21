using System.IO;
using System.Text;
using OpenRA.Graphics;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenOP2.Traits
{
	public class DumpPalettesToJascFilesInfo : TraitInfo
	{
		public override object Create(ActorInitializer init) { return new DumpPalettesToJascFiles(); }
	}

	public class DumpPalettesToJascFiles : IWorldLoaded
	{
		public void WorldLoaded(World w, WorldRenderer wr)
		{
			var outDir = Path.Combine(Platform.EngineDir, "..");
			for (var i = 1; i < 9; i++)
			{
				var palette = wr.Palette(i.ToString());
				var sb = new StringBuilder();
				sb.AppendLine("JASC-PAL");
				sb.AppendLine("0100");
				sb.AppendLine("256");
				for (var c = 0; c < 256; c++)
				{
					var argb = palette.Palette[c];
					var color = Color.FromArgb(argb);
					sb.AppendLine($"{color.R} {color.G} {color.B}");
				}

				var palPath = Path.Combine(outDir, $"{i}.PAL");
				using (var stream = new StreamWriter(palPath))
				{
					stream.Write(sb.ToString());
				}
			}
		}
	}
}
