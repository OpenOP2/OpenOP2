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
using System.Text;
using OpenRA.FileSystem;
using FS = OpenRA.FileSystem.FileSystem;

namespace OpenRA.Mods.OpenOP2.FileSystem
{
	public class ClmFileLoader : IPackageLoader
	{
		struct ClmEntry
		{
			public string Filename { get; set; }
			public uint Offset { get; set; }
			public uint Length { get; set; }
		}

		struct WaveFormatEx
		{
			public ushort FormatTag { get; set; }
			public ushort Channels { get; set; }
			public uint SamplesPerSec { get; set; }
			public uint AvgBytesPerSec { get; set; }
			public ushort BlockAlign { get; set; }
			public ushort BitsPerSample { get; set; }
			public ushort Size { get; set; }
		}

		sealed class ClmFile : IReadOnlyPackage
		{
			// readonly byte[] clmHeaderUnknown = new byte[] { 0, 0, 0, 0, 1, 0 };
			const string ClmHeaderString = "OP2 Clump File Version 1.0\u001a\0\0\0\0\0";
			const int HeaderSize = 44;

			public string Name { get; }
			public IEnumerable<string> Contents { get { return index.Keys; } }

			readonly Dictionary<string, ClmEntry> index = new();
			readonly Stream stream;
			readonly WaveFormatEx waveFormatEx;

			public ClmFile(Stream stream, string filename)
			{
				Name = filename;
				this.stream = stream;

				var headerString = stream.ReadASCII(32);
				if (headerString != ClmHeaderString)
					return;

				waveFormatEx = new WaveFormatEx
				{
					FormatTag = stream.ReadUInt16(),
					Channels = stream.ReadUInt16(),
					SamplesPerSec = stream.ReadUInt32(),
					AvgBytesPerSec = stream.ReadUInt32(),
					BlockAlign = stream.ReadUInt16(),
					BitsPerSample = stream.ReadUInt16(),
					Size = stream.ReadUInt16()
				};
				_ = stream.ReadBytes(6);

				var filesCount = stream.ReadUInt32();

				for (var i = 0; i < filesCount; i++)
				{
					var clmIndex = new ClmEntry
					{
						Filename = stream.ReadASCII(8).Replace("\0", string.Empty),
						Offset = stream.ReadUInt32(),
						Length = stream.ReadUInt32()
					};

					index.Add(clmIndex.Filename, clmIndex);
				}

				_ = stream.ReadASCII(160);
			}

			public Stream GetStream(string filename)
			{
				if (!index.TryGetValue(filename.Replace(".wav", string.Empty), out var entry))
					return null;

				stream.Seek(entry.Offset, SeekOrigin.Begin);
				var dataHeader = GetWaveHeader(entry.Length);
				var data = stream.ReadBytes((int)entry.Length);

				var byteArray = new byte[HeaderSize + data.Length];
				Buffer.BlockCopy(dataHeader, 0, byteArray, 0, HeaderSize);
				Buffer.BlockCopy(data, 0, byteArray, HeaderSize, (int)entry.Length);

				return new MemoryStream(byteArray);
			}

			byte[] GetWaveHeader(uint length)
			{
				var bytes = new byte[HeaderSize];
				var stream = new MemoryStream(bytes);
				using (var streamWriter = new BinaryWriter(stream, Encoding.UTF8, false))
				{
					streamWriter.Write(Encoding.ASCII.GetBytes("RIFF"));
					streamWriter.Write(38 + length);
					streamWriter.Write(Encoding.ASCII.GetBytes("WAVE"));
					streamWriter.Write(Encoding.ASCII.GetBytes("fmt "));
					streamWriter.Write(16);
					streamWriter.Write(waveFormatEx.FormatTag);
					streamWriter.Write(waveFormatEx.Channels);
					streamWriter.Write(waveFormatEx.SamplesPerSec);
					streamWriter.Write(waveFormatEx.AvgBytesPerSec);
					streamWriter.Write(waveFormatEx.BlockAlign);
					streamWriter.Write(waveFormatEx.BitsPerSample);
					streamWriter.Write(Encoding.ASCII.GetBytes("data"));
					streamWriter.Write(length);
				}

				return bytes;
			}

			public bool Contains(string filename)
			{
				return index.ContainsKey(filename.Replace(".wav", string.Empty));
			}

			public IReadOnlyPackage OpenPackage(string filename, FS context)
			{
				var childStream = GetStream(filename);
				if (childStream == null)
					return null;

				if (context.TryParsePackage(childStream, filename, out var package))
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
			if (!filename.EndsWith(".clm", StringComparison.InvariantCultureIgnoreCase))
			{
				package = null;
				return false;
			}

			package = new ClmFile(s, filename);
			return true;
		}
	}
}
