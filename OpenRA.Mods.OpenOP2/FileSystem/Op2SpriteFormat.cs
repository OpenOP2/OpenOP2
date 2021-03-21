namespace OpenRA.Mods.OpenOP2.FileSystem
{
	public class PrtFile
	{
		public string ID; // CPAL
		public int PalCount;
		public Ppal[] PalData;
		public int ImageCount;
		public Op2Image[] ImageHeader;
		public int AllGroupCount;
		public int AllFrameCount;
		public int AllPicCount;
		public int AllExtInfoCount;
		public ImageGroup[] Groups;
	}

	public class Ppal
	{
		public string ID; // PPAL
		public int Size;
		public string HeadID; // head
		public int BytesPerEntry;
		public int Unknown;
		public string DataID; // data
		public int PalSize; // 0x0400
		public RgbQuad[] PaletteData;
	}

	public class RgbQuad
	{
		public int Red;
		public int Green;
		public int Blue;
		public int Reserved;
	}

	public class Op2Image
	{
		public int SizeScanline;
		public byte[] ImgData;
		public int SizeX;
		public int SizeY;
		public short Unknown;
		public short Palette;
	}

	public class ImageGroup
	{
		public int Unknown1;
		public int SelLeft;
		public int SelTop;
		public int SelRight;
		public int SelBottom;
		public int CenterX;
		public int CenterY;
		public int Unknown8;
		public int FrameCount;
		public Op2Frame[] Frames;
		public int GroupExtCount;
		public GroupExt[] Extended;
	}

	public class GroupExt
	{
		public int Unknown1;
		public int Unknown2;
		public int Unknown3;
		public int Unknown4;
	}

	public class Op2Frame
	{
		public byte PicCount;
		public byte Unknown;
		public BytePair[] ExtUnknown1;
		public BytePair[] ExtUnknown2;
		public Op2Picture[] Pictures;
	}

	public class BytePair
	{
		public byte Byte1;
		public byte Byte2;
	}

	public class Op2Picture
	{
		public short ImgNumber;
		public byte Reserved; // standard 0xFF
		public byte PicOrder;
		public short PosX;
		public short PosY;
	}
}
