using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;
using OpenRA.Primitives;
using Util = OpenRA.Mods.Common.Util;

namespace OpenRA.Mods.OpenOP2.SpriteLoaders
{
	internal class Op2SpriteUtil
	{
		internal static T LoadField<T>(Dictionary<string, MiniYaml> d, string key, T fallback)
		{
			if (d.TryGetValue(key, out var value))
				return FieldLoader.GetValue<T>(key, value.Value);

			return fallback;
		}
	}

	public class CombineSequenceDTO
	{
		public bool IsBlank { get; set; }
		public int Start { get; set; }
		public float3 Offset { get; set; }
		public int ZOffset { get; set; }
		public int Length { get; set; }
		public int[] Frames { get; set; }
	}

	public class SequenceDTO : CombineSequenceDTO
	{
		public string UseFile { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public int Stride { get; set; }
		public int Facings { get; set; }
		public int Tick { get; set; }
		public int ShadowStart { get; set; }
		public int ShadowZOffset { get; set; }
		public Rectangle Bounds { get; set; }
		public bool IgnoreWorldTint { get; set; }
		public List<CombineSequenceDTO> Combine { get; set; } = new List<CombineSequenceDTO>();
	}

	public class Op2SpriteSequenceLoader : ISpriteSequenceLoader
	{
		public Op2SpriteSequenceLoader(ModData modData) { }

		public IReadOnlyDictionary<string, ISpriteSequence> ParseSequences(ModData modData, string tileSet, SpriteCache cache, MiniYamlNode node)
		{
			var sequences = new Dictionary<string, ISpriteSequence>();
			var nodes = node.Value.ToDictionary();
			var isOp2Sequence = false;

			try
			{
				if (nodes.TryGetValue("Sets", out var defaults))
				{
					isOp2Sequence = true;
				}
			}
			catch (Exception e)
			{
				throw new InvalidDataException("Error occurred while parsing {0}".F(node.Key), e);
			}

			if (!isOp2Sequence)
			{
				try
				{
					if (nodes.TryGetValue("Defaults", out var defaults))
					{
						nodes.Remove("Defaults");
						foreach (var n in nodes)
						{
							n.Value.Nodes = MiniYaml.Merge(new[] { defaults.Nodes, n.Value.Nodes });
							n.Value.Value = n.Value.Value ?? defaults.Value;
						}
					}
				}
				catch (Exception e)
				{
					throw new InvalidDataException("Error occurred while parsing {0}".F(node.Key), e);
				}

				foreach (var kvp in nodes)
				{
					using (new Support.PerfTimer("new Sequence(\"{0}\")".F(node.Key), 20))
					{
						try
						{
							sequences.Add(kvp.Key, new DefaultSpriteSequence(modData, tileSet, cache, this, node.Key, kvp.Key, kvp.Value));
						}
						catch (FileNotFoundException ex)
						{
							// Defer exception until something tries to access the sequence
							// This allows the asset installer and OpenRA.Utility to load the game without having the actor assets
							sequences.Add(kvp.Key, new FileNotFoundSequence(ex));
						}
					}
				}
			}
			else
			{
				using (new Support.PerfTimer("new Op2Sequence(\"{0}\")".F(node.Key), 20))
				{
					var prt = Prt.Instance;
					var sequenceDtos = prt.Sequences[node.Key];
					foreach (var sequenceDto in sequenceDtos)
					{
						sequences.Add(sequenceDto.Name, new Op2SpriteSequence(cache, this, node.Key, sequenceDto));
					}
				}
			}

			return new ReadOnlyDictionary<string, ISpriteSequence>(sequences);
		}
	}

	public class Op2SpriteSequence : ISpriteSequence
	{
		private const string ImageName = "op2_art.bmp";
		private static readonly WDist DefaultShadowSpriteZOffset = new WDist(-5);
		private readonly Sprite[] sprites;
		private readonly bool reverseFacings, transpose;
		private readonly string sequence;

		private readonly ISpriteSequenceLoader loader;

		public string Name { get; private set; }
		public int Start { get; private set; }
		public int Length { get; private set; }
		public int Stride { get; private set; }
		public int Facings { get; private set; }
		public int Tick { get; private set; }
		public int ZOffset { get; private set; }
		public float ZRamp { get; private set; }
		public int ShadowStart { get; private set; }
		public int ShadowZOffset { get; private set; }
		public int[] Frames { get; private set; }
		public Rectangle Bounds { get; private set; }
		public bool IgnoreWorldTint { get; private set; }

		public int InterpolatedFacings => -1;

		public float Scale => 1f;

		protected string GetSpriteSrc(string useFile)
		{
			return useFile ?? ImageName;
		}

		protected static Rectangle FlipRectangle(Rectangle rect, bool flipX, bool flipY)
		{
			var left = flipX ? rect.Right : rect.Left;
			var top = flipY ? rect.Bottom : rect.Top;
			var right = flipX ? rect.Left : rect.Right;
			var bottom = flipY ? rect.Top : rect.Bottom;

			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		public Op2SpriteSequence(
			SpriteCache cache,
			ISpriteSequenceLoader loader,
			string sequence,
			SequenceDTO dto)
		{
			this.sequence = sequence;
			Name = dto.Name;
			this.loader = loader;
			Start = dto.Start;
			ShadowStart = -1;
			ShadowZOffset = DefaultShadowSpriteZOffset.Length;
			ZOffset = dto.ZOffset;
			ZRamp = 0;
			Tick = 40;
			transpose = false;
			Frames = dto.Frames;
			IgnoreWorldTint = false;
			Facings = dto.Facings;
			reverseFacings = false;
			var offset = dto.Offset;
			const BlendMode blendMode = BlendMode.Alpha;

			Func<int, IEnumerable<int>> getUsedFrames = frameCount =>
			{
				Length = dto.Length;
				Stride = Length;

				// if (Length > Stride)
				// 	throw new YamlException("Sequence {0}.{1}: Length must be <= stride"
				// 		.F(sequence, animation));
				//
				// if (Frames != null && Length > Frames.Length)
				// 	throw new YamlException("Sequence {0}.{1}: Length must be <= Frames.Length"
				// 		.F(sequence, animation));
				//
				// var end = Start + (Facings - 1) * Stride + Length - 1;
				// if (Frames != null)
				// {
				// 	foreach (var f in Frames)
				// 		if (f < 0 || f >= frameCount)
				// 			throw new YamlException("Sequence {0}.{1} defines a Frames override that references frame {2}, but only [{3}..{4}] actually exist"
				// 				.F(sequence, animation, f, Start, end));
				//
				// 	if (Start < 0 || end >= Frames.Length)
				// 		throw new YamlException("Sequence {0}.{1} uses indices [{2}..{3}] of the Frames list, but only {4} frames are defined"
				// 				.F(sequence, animation, Start, end, Frames.Length));
				// }
				// else if (Start < 0 || end >= frameCount)
				// 	throw new YamlException("Sequence {0}.{1} uses frames [{2}..{3}], but only [0..{4}] actually exist"
				// 			.F(sequence, animation, Start, end, frameCount - 1));
				//
				// if (ShadowStart >= 0 && ShadowStart + (Facings - 1) * Stride + Length > frameCount)
				// 	throw new YamlException("Sequence {0}.{1}'s shadow frames use frames [{2}..{3}], but only [0..{4}] actually exist"
				// 		.F(sequence, animation, ShadowStart, ShadowStart + (Facings - 1) * Stride + Length - 1, frameCount - 1));
				var usedFrames = new List<int>();
				for (var facing = 0; facing < Facings; facing++)
				{
					for (var frame = 0; frame < Length; frame++)
					{
						var i = transpose ? (frame % Length) * Facings + facing :
							(facing * Stride) + (frame % Length);

						usedFrames.Add(Frames != null ? Frames[i] : Start + i);
					}
				}

				if (ShadowStart >= 0)
					return usedFrames.Concat(usedFrames.Select(i => i + ShadowStart - Start));

				return usedFrames;
			};

			if (dto.Combine.Count > 0)
			{
				var combine = dto.Combine;
				var combined = Enumerable.Empty<Sprite>();
				foreach (var sub in combine)
				{
					// Allow per-sprite offset, flipping, start, and length
					var subStart = sub.Start;
					var subOffset = sub.Offset;
					var subFrames = sub.Frames;
					var subLength = 0;
					Func<int, IEnumerable<int>> subGetUsedFrames = subFrameCount =>
					{
						subLength = subFrames != null ? subFrames.Length : subFrameCount - subStart;

						return subFrames != null ? subFrames.Skip(subStart).Take(subLength) : Enumerable.Range(subStart, subLength);
					};

					using (new Support.PerfTimer("(\"{0}\")".F(sub.Length), 20))
					{
						var subSprites = cache[ImageName, subGetUsedFrames].Select(
							s => s != null
								? new Sprite(s.Sheet,
									s.Bounds, ZRamp, // FlipRectangle(s.Bounds, subFlipX, subFlipY)
									s.Offset + subOffset + offset,
									s.Channel, blendMode)
								: null).ToList();

						var frames = subFrames != null ? subFrames.Skip(subStart).Take(subLength).ToArray() : Exts.MakeArray(subLength, i => subStart + i);
						combined = combined.Concat(frames.Select(i => subSprites[i]));
					}
				}

				sprites = combined.ToArray();
				getUsedFrames(sprites.Length);
			}
			else
			{
				// Apply offset to each sprite in the sequence
				// Different sequences may apply different offsets to the same frame
				sprites = cache[GetSpriteSrc(dto.UseFile), getUsedFrames].Select(
					s => s != null ? new Sprite(s.Sheet,
						s.Bounds, ZRamp, // FlipRectangle(s.Bounds, subFlipX, subFlipY)
						s.Offset + offset,
						s.Channel, blendMode) : null).ToArray();
			}

			var boundSprites = SpriteBounds(sprites, Frames, Start, Facings, Length, Stride, transpose);
			if (ShadowStart > 0)
				boundSprites = boundSprites.Concat(SpriteBounds(sprites, Frames, ShadowStart, Facings, Length, Stride, transpose));

			Bounds = boundSprites.Union();
		}

		/// <summary>Returns the bounds of all of the sprites that can appear in this animation</summary>
		static IEnumerable<Rectangle> SpriteBounds(Sprite[] sprites, int[] frames, int start, int facings, int length, int stride, bool transpose)
		{
			for (var facing = 0; facing < facings; facing++)
			{
				for (var frame = 0; frame < length; frame++)
				{
					var i = transpose ? (frame % length) * facings + facing :
								(facing * stride) + (frame % length);
					var s = frames != null ? sprites[frames[i]] : sprites[start + i];
					if (!s.Bounds.IsEmpty)
						yield return new Rectangle(
							(int)(s.Offset.X - s.Size.X / 2),
							(int)(s.Offset.Y - s.Size.Y / 2),
							s.Bounds.Width, s.Bounds.Height);
				}
			}
		}

		public Sprite GetSprite(int frame)
		{
			return GetSprite(Start, frame, WAngle.Zero);
		}

		public Sprite GetSprite(int frame, WAngle facing)
		{
			return GetSprite(Start, frame, facing);
		}

		public Sprite GetShadow(int frame, WAngle facing)
		{
			return ShadowStart >= 0 ? GetSprite(ShadowStart, frame, facing) : null;
		}

		protected virtual Sprite GetSprite(int start, int frame, WAngle facing)
		{
			var f = GetFacingFrameOffset(facing);
			if (reverseFacings)
				f = (Facings - f) % Facings;

			var i = transpose ? (frame % Length) * Facings + f :
				(f * Stride) + (frame % Length);

			var j = Frames != null ? Frames[i] : start + i;
			if (sprites[j] == null)
				throw new InvalidOperationException("Attempted to query unloaded sprite from {0}.{1}".F(Name, sequence) +
					" start={0} frame={1} facing={2}".F(start, frame, facing));

			return sprites[j];
		}

		protected virtual int GetFacingFrameOffset(WAngle facing)
		{
			return Util.IndexFacing(facing, Facings);
		}

		public (Sprite, WAngle) GetSpriteWithRotation(int frame, WAngle facing)
		{
			var rotation = WAngle.Zero;

			// Note: Error checking is not done here as it is done on load
			if (InterpolatedFacings != -1)
				rotation = Util.GetInterpolatedFacing(facing, Math.Abs(Facings), InterpolatedFacings);

			return (GetSprite(Start, frame, facing), rotation);
		}

		public float GetAlpha(int frame)
		{
			return 1f;
		}
	}
}
