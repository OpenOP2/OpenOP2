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
using System.Collections.Generic;
using System.IO;
using OpenRA.FileSystem;
using FS = OpenRA.FileSystem.FileSystem;

namespace OpenRA.Mods.OpenOP2.FileSystem
{
	public class VolFileLoader : IPackageLoader
	{
		struct VolEntry
		{
			public string Filename { get; set; }
			public uint Offset { get; set; }
			public uint Size { get; set; }
			public int Compression { get; set; }
		}

		sealed class VolFile : IReadOnlyPackage
		{
			public string Name { get; private set; }
			public IEnumerable<string> Contents { get { return index.Keys; } }

			readonly Dictionary<string, VolEntry> index = new Dictionary<string, VolEntry>();
			readonly Stream stream;

			public VolFile(Stream stream, string filename)
			{
				Name = filename;
				this.stream = stream;

				var magicByte = stream.ReadASCII(4);
				if (magicByte != "VOL ")
					return;

				var unknown1 = stream.ReadUInt32();

				var header = stream.ReadASCII(4);
				var zero = stream.ReadUInt32();
				if (header != "volh" && zero != 0)
					return;

				var volumeSection = stream.ReadASCII(4);
				if (volumeSection != "vols")
					return;

				var unknown2 = stream.ReadUInt32();
				var directoryOffset = stream.ReadUInt32();

				var entryFilenames = new List<string>();
				while (true)
				{
					var entryFilename = stream.ReadASCIIZ();
					if (string.IsNullOrEmpty(entryFilename))
						break;

					entryFilenames.Add(entryFilename);
				}

				while (true)
				{
					if (stream.ReadByte() != 0)
						break;
				}

				stream.Seek(-1, SeekOrigin.Current);

				var volumeIndex = stream.ReadASCII(4);
				if (volumeIndex != "voli")
					return;

				var totalSize = stream.ReadUInt32();

				foreach (var entryFilename in entryFilenames)
				{
					var filenameIndex = stream.ReadUInt32();
					var offset = stream.ReadUInt32();
					var size = stream.ReadUInt32();
					var compression = stream.ReadByte();
					var valid = stream.ReadByte();
					if (valid != 0 && valid != 1)
						throw new InvalidDataException("Check byte contains invalid values");

					// Ignore duplicate files
					if (index.ContainsKey(entryFilename))
						continue;

					var info = new VolEntry()
					{
						Filename = entryFilename,
						Offset = offset + 8,
						Size = size,
						Compression = compression,
					};

					if (compression != 0)
						throw new NotImplementedException("'RLE', 'LZ', 'LZH' are not yet supported.");

					index.Add(entryFilename, info);
				}
			}

			public Stream GetStream(string filename)
			{
				VolEntry entry;
				if (!index.TryGetValue(filename, out entry))
					return null;

				stream.Seek(entry.Offset, SeekOrigin.Begin);
				var data = stream.ReadBytes((int)entry.Size);
				return new MemoryStream(data);
			}

			public bool Contains(string filename)
			{
				return index.ContainsKey(filename);
			}

			public IReadOnlyPackage OpenPackage(string filename, FS context)
			{
				IReadOnlyPackage package;
				var childStream = GetStream(filename);
				if (childStream == null)
					return null;

				if (context.TryParsePackage(childStream, filename, out package))
					return package;

				childStream.Dispose();
				return null;
			}

			public void Dispose()
			{
				stream.Dispose();
			}
		}

		bool IPackageLoader.TryParsePackage(Stream s, string filename, FS context, out IReadOnlyPackage package)
		{
			if (!filename.EndsWith(".vol", StringComparison.InvariantCultureIgnoreCase))
			{
				package = null;
				return false;
			}

			package = new VolFile(s, filename);
			return true;
		}
	}
}
