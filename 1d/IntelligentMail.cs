using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class IntelligentMail : Base1D
	{
		private class BarValue
		{
			public int Character { get; set; }
			public int Bit { get; set; }
		}

		private static readonly Regex _validCode = new Regex(@"^\d*$");
		private static ushort[] _characters;
		private static ushort[] Characters
		{
			get
			{
				if(_characters == null)
				{
					var characters = new List<ushort>(1365);
					characters.AddRange(InitializeNof13Table(5, 1287));
					characters.AddRange(InitializeNof13Table(2, 78));
					_characters = characters.ToArray();
				}
				return _characters;
			}
		}

		private static readonly int[,] _patterns = { { 0 }, { 1 }, { 2 }, { 3 } };
		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 1; } }
		protected override decimal HeightRatio { get { return 0; } }
		protected override int StaticHeight { get { return 44; } }
		protected override int QuietWidth { get { return 25; } }

		private const int _barWidth = 40;
		protected override void DrawNonInterleaved(BarcodeOptions barcodeOptions, int[] codes, Graphics gr, int cursor, int height)
		{
			var top = barcodeOptions.AddQuietZone ? height * 1f / 8f : 0;
			var bottom = barcodeOptions.AddQuietZone ? height * 7f / 8f : height;
			var midTop = top + (bottom / 3f);
			var midBottom = bottom * 2f / 3f;

			foreach(var code in codes)
			{
				float barTop;
				float barHeight;

				switch(code)
				{
					case 1:
						barTop = midTop;
						barHeight = bottom - midTop;
						break;
					case 2:
						barTop = top;
						barHeight = midBottom - top;
						break;
					case 3:
						barTop = top;
						barHeight = bottom - top;
						break;
					default:
						barTop = midTop;
						barHeight = midBottom - midTop;
						break;
				}

				gr.FillRectangle(Brushes.Black, cursor, barTop, barcodeOptions.BarWeight, barHeight);

				cursor += 2 * barcodeOptions.BarWeight;
			}
		}

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return 65 * 2 * barWeight;
		}

		private static class CHARACTER
		{
			public const int A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, I = 8, J = 9;
		}

		private static readonly BarValue[,] _barValues =
		{
			{ new BarValue { Character = CHARACTER.H, Bit = 2 }, new BarValue { Character = CHARACTER.E, Bit = 3 } },
			{ new BarValue { Character = CHARACTER.B, Bit = 10 }, new BarValue { Character = CHARACTER.A, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 12 }, new BarValue { Character = CHARACTER.C, Bit = 8 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 5 }, new BarValue { Character = CHARACTER.G, Bit = 11 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 9 }, new BarValue { Character = CHARACTER.D, Bit = 1 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 1 }, new BarValue { Character = CHARACTER.F, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 5 }, new BarValue { Character = CHARACTER.B, Bit = 8 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 4 }, new BarValue { Character = CHARACTER.J, Bit = 11 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 3 }, new BarValue { Character = CHARACTER.I, Bit = 10 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 9 }, new BarValue { Character = CHARACTER.H, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 11 }, new BarValue { Character = CHARACTER.B, Bit = 4 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 5 }, new BarValue { Character = CHARACTER.C, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 10 }, new BarValue { Character = CHARACTER.A, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 1 }, new BarValue { Character = CHARACTER.G, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 6 }, new BarValue { Character = CHARACTER.E, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 3 }, new BarValue { Character = CHARACTER.I, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 4 }, new BarValue { Character = CHARACTER.C, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.B, Bit = 1 }, new BarValue { Character = CHARACTER.J, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 10 }, new BarValue { Character = CHARACTER.F, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 0 }, new BarValue { Character = CHARACTER.D, Bit = 8 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 2 }, new BarValue { Character = CHARACTER.A, Bit = 4 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 11 }, new BarValue { Character = CHARACTER.B, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 8 }, new BarValue { Character = CHARACTER.D, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 6 }, new BarValue { Character = CHARACTER.H, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 1 }, new BarValue { Character = CHARACTER.E, Bit = 10 } },
			{ new BarValue { Character = CHARACTER.B, Bit = 12 }, new BarValue { Character = CHARACTER.G, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 3 }, new BarValue { Character = CHARACTER.I, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 8 }, new BarValue { Character = CHARACTER.J, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 6 }, new BarValue { Character = CHARACTER.C, Bit = 10 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 4 }, new BarValue { Character = CHARACTER.A, Bit = 5 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 4 }, new BarValue { Character = CHARACTER.F, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 11 }, new BarValue { Character = CHARACTER.B, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 0 }, new BarValue { Character = CHARACTER.J, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 6 }, new BarValue { Character = CHARACTER.E, Bit = 8 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 1 }, new BarValue { Character = CHARACTER.D, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 9 }, new BarValue { Character = CHARACTER.I, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 11 }, new BarValue { Character = CHARACTER.G, Bit = 1 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 5 }, new BarValue { Character = CHARACTER.H, Bit = 4 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 3 }, new BarValue { Character = CHARACTER.B, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 7 }, new BarValue { Character = CHARACTER.C, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.B, Bit = 3 }, new BarValue { Character = CHARACTER.E, Bit = 1 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 10 }, new BarValue { Character = CHARACTER.D, Bit = 5 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 7 }, new BarValue { Character = CHARACTER.J, Bit = 4 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 11 }, new BarValue { Character = CHARACTER.F, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 8 }, new BarValue { Character = CHARACTER.H, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 2 }, new BarValue { Character = CHARACTER.I, Bit = 1 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 10 }, new BarValue { Character = CHARACTER.D, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 3 }, new BarValue { Character = CHARACTER.A, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 5 }, new BarValue { Character = CHARACTER.C, Bit = 4 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 8 }, new BarValue { Character = CHARACTER.B, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 0 }, new BarValue { Character = CHARACTER.E, Bit = 5 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 3 }, new BarValue { Character = CHARACTER.A, Bit = 10 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 12 }, new BarValue { Character = CHARACTER.J, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 11 }, new BarValue { Character = CHARACTER.B, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 8 }, new BarValue { Character = CHARACTER.H, Bit = 9 } },
			{ new BarValue { Character = CHARACTER.F, Bit = 4 }, new BarValue { Character = CHARACTER.A, Bit = 11 } },
			{ new BarValue { Character = CHARACTER.B, Bit = 5 }, new BarValue { Character = CHARACTER.C, Bit = 2 } },
			{ new BarValue { Character = CHARACTER.J, Bit = 1 }, new BarValue { Character = CHARACTER.E, Bit = 12 } },
			{ new BarValue { Character = CHARACTER.I, Bit = 3 }, new BarValue { Character = CHARACTER.G, Bit = 6 } },
			{ new BarValue { Character = CHARACTER.H, Bit = 0 }, new BarValue { Character = CHARACTER.D, Bit = 7 } },
			{ new BarValue { Character = CHARACTER.E, Bit = 7 }, new BarValue { Character = CHARACTER.H, Bit = 5 } },
			{ new BarValue { Character = CHARACTER.A, Bit = 12 }, new BarValue { Character = CHARACTER.B, Bit = 11 } },
			{ new BarValue { Character = CHARACTER.C, Bit = 9 }, new BarValue { Character = CHARACTER.J, Bit = 0 } },
			{ new BarValue { Character = CHARACTER.G, Bit = 8 }, new BarValue { Character = CHARACTER.F, Bit = 3 } },
			{ new BarValue { Character = CHARACTER.D, Bit = 10 }, new BarValue { Character = CHARACTER.I, Bit = 2 } }
		};
		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrWhiteSpace(inputData) || !_validCode.IsMatch(inputData)
				|| (inputData.Length != 20 && inputData.Length != 25 && inputData.Length != 29 && inputData.Length != 31))
			{
				return new int[0];
			}

			var trackingCode = inputData.Substring(0, 20);
			var routingCode = string.Empty;
			if(inputData.Length > 20)
			{
				routingCode = inputData.Substring(20);
			}

			var binary = new BigInteger();
			if(routingCode.Length >= 5)
			{
				ulong routingUlong;
				ulong.TryParse(routingCode, out routingUlong);
				binary = routingUlong;
				binary++;
				if(routingCode.Length >= 9)
				{
					binary += 100000;
				}
				if(routingCode.Length == 11)
				{
					binary += 1000000000;
				}
			}

			for(var c = 0; c < 20; c++)
			{
				var num = trackingCode[c] - 48;
				binary *= c == 1 ? 5ul : 10ul;
				binary += (ulong) num;
			}

			barcodeOptions.CheckDigit = GenerateCRC(binary);
			var codewords = GenerateCodewords(binary, barcodeOptions.CheckDigit);
			var characters = GetCharacters(codewords, barcodeOptions.CheckDigit);
			var content = new int[65];
			for(var c = 0; c < 65; c++)
			{
				var value = 0;
				for(var i = 0; i < 2; i++)
				{
					var bv = _barValues[c, i];
					var character = characters[bv.Character];
					if(((character >> bv.Bit) & 0x1) == 0x1)
					{
						value += (i + 1);
					}
				}
				content[c] = value;
			}
			return content;
		}

		private int GenerateCRC(BigInteger binary)
		{
			var binArray = binary.ToByteArray();
			var bytes = new byte[13];

			for(var c = 0; c < binArray.Length && c < 13; c++)
			{
				bytes[12 - c] = binArray[c];
			}

			var crc = USPS_MSB_Math_CRC11GenerateFrameCheckSequence(bytes);
			return crc;
		}

		private int[] GenerateCodewords(BigInteger binary, int crc)
		{
			var codewords = new int[10];
			for(var c = CHARACTER.J; c > CHARACTER.A; c--)
			{
				var dividend = c == 9 ? 636ul : 1365ul;
				codewords[c] = (int) (binary % dividend);
				binary /= dividend;
			}
			codewords[CHARACTER.A] = (int) (binary);
			codewords[CHARACTER.J] *= 2;

			if((crc & 0x400) == 0x400)
			{
				codewords[CHARACTER.A] += 659;
			}
			return codewords;
		}

		private ushort[] GetCharacters(int[] codewords, int crc)
		{
			var characters = codewords.Select(x => Characters[x]).ToArray();
			for(var c = CHARACTER.A; c <= CHARACTER.J; c++)
			{
				var bit = ((crc >> c) & 0x1) == 0x1;
				if(bit)
				{
					characters[c] = (ushort) ((~characters[c]) & 0x1FFF);
				}
			}
			return characters;
		}

		/***************************************************************************
		** USPS_MSB_Math_CRC11GenerateFrameCheckSequence
		**
		** Inputs:
		** ByteArrayPtr is the address of a 13 byte array holding 102 bits which
		** are right justified - ie: the leftmost 2 bits of the first byte do not
		** hold data and must be set to zero.
		**
		** Outputs:
		** return unsigned short - 11 bit Frame Check Sequence (right justified)
		***************************************************************************/

		private int USPS_MSB_Math_CRC11GenerateFrameCheckSequence(byte[] byteArray)
		{
			const short generatorPolynomial = 0x0F35;
			var frameCheckSequence = 0x07FF;

			var c = 0;
			/* Do most significant byte skipping the 2 most significant bits */
			var data = byteArray[c] << 5;
			c++;
			for(var bit = 2; bit < 8; bit++)
			{
				if(((frameCheckSequence ^ data) & 0x400) == 0x400)
				{
					frameCheckSequence = (frameCheckSequence << 1) ^ generatorPolynomial;
				}
				else
				{
					frameCheckSequence = (frameCheckSequence << 1);
				}
				frameCheckSequence &= 0x7FF;
				data <<= 1;
			}
			/* Do rest of the bytes */
			for(var byteIndex = 1; byteIndex < 13; byteIndex++)
			{
				data = byteArray[c] << 3;
				c++;
				for(var bit = 0; bit < 8; bit++)
				{
					if(((frameCheckSequence ^ data) & 0x0400) == 0x400)
					{
						frameCheckSequence = (frameCheckSequence << 1) ^ generatorPolynomial;
					}
					else
					{
						frameCheckSequence = (frameCheckSequence << 1);
					}
					frameCheckSequence &= 0x7FF;
					data <<= 1;
				}
			}
			return frameCheckSequence;
		}

		private static ushort ReverseUnsignedShort(ushort input)
		{
			ushort reverse = 0;
			for(var i = 0; i < 16; i++)
			{
				reverse <<= 1;
				reverse |= (ushort) (input & 0x1);
				input >>= 1;
			}
			return reverse;
		}


		/******************************************************************************
		** InitializeNof13Table
		**
		** Inputs:
		** N is the type of table (i.e. 5 for 5of13 table, 2 for 2of13 table
		** TableLength is the length of the table requested (i.e. 78 for 2of13 table)
		** Output:
		** TableNof13 is a pointer to the resulting table
		******************************************************************************/

		private static ushort[] InitializeNof13Table(int n, int tableLength)
		{
			var tableNof13 = new ushort[tableLength];
			/* Count up to 2^13 - 1 and find all those values that have N bits on */
			var lutLowerIndex = 0;
			var lutUpperIndex = tableLength - 1;
			for(ushort Count = 0; Count < 8192; Count++)
			{
				var bitCount = 0;
				for(var bitIndex = 0; bitIndex < 13; bitIndex++)
					bitCount += ((Count & (1 << bitIndex)) != 0) ? 1 : 0;
				/* If we don't have the right number of bits on, go on to the next value */
				if(bitCount != n)
					continue;
				/* If the reverse is less than count, we have already visited this pair before */
				var reverse = (ushort) (ReverseUnsignedShort(Count) >> 3);
				if(reverse < Count)
					continue;
				/* If Count is symmetric, place it at the first free slot from the end of the */
				/* list. Otherwise, place it at the first free slot from the beginning of the */
				/* list AND place Reverse at the next free slot from the beginning of the list.*/
				if(Count == reverse)
				{
					tableNof13[lutUpperIndex] = Count;
					lutUpperIndex -= 1;
				}
				else
				{
					tableNof13[lutLowerIndex] = Count;
					lutLowerIndex += 1;
					tableNof13[lutLowerIndex] = reverse;
					lutLowerIndex += 1;
				}
			}
			/* Make sure the lower and upper parts of the table meet properly */
			if(lutLowerIndex != (lutUpperIndex + 1))
				return new ushort[0];

			return tableNof13;
		}
	}
}