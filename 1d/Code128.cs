using System.Collections.Generic;
using System.Text.RegularExpressions;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Code128 : Base1D
	{
		private enum CodeSet { A, B, C }
		private enum TargetCodeSet { A, B, AorB, Any }

		private static class ControlCodes
		{
			internal const int DEL = 95;
			internal const int FNC_3 = 96;
			internal const int FNC_2 = 97;

			internal const int SHIFT = 98;
			internal const int CODE_C = 99;
			internal const int CODE_B = 100;
			internal const int CODE_A = 101;

			internal const int FNC_1 = 102;

			internal const int START_A = 103;
			internal const int START_B = 104;
			internal const int START_C = 105;

			internal const int STOP = 106;

			internal static readonly IDictionary<string, int> EscapeCharacters = new Dictionary<string, int>
		                                                           {
																	   {"><", 62},
																	   {">0", 30},
																	   {">=", 94},
																	   {">1", DEL},
																	   {">2", FNC_3},
																	   {">3", FNC_2},
																	   
																	   {">4", SHIFT},
																	   {">5", CODE_C},
																	   {">6", CODE_B},
																	   {">7", CODE_A},
																	   {">8", FNC_1},
																	   {">9", START_A},
																	   {">:", START_B},
																	   {">;", START_C}
		                                                           };

			public static int GetStartCode(CodeSet currentCodeSet)
			{
				return currentCodeSet == CodeSet.A ? START_A : currentCodeSet == CodeSet.B ? START_B : START_C;
			}

			public static int GetCodeSet(CodeSet currentCodeSet)
			{
				return currentCodeSet == CodeSet.A ? CODE_A : currentCodeSet == CodeSet.B ? CODE_B : CODE_C;
			}
		}

		private static readonly int[,] _patterns = 
                     {
                        {2,1,2,2,2,2,0,0},  // 0
                        {2,2,2,1,2,2,0,0},  // 1
                        {2,2,2,2,2,1,0,0},  // 2
                        {1,2,1,2,2,3,0,0},  // 3
                        {1,2,1,3,2,2,0,0},  // 4
                        {1,3,1,2,2,2,0,0},  // 5
                        {1,2,2,2,1,3,0,0},  // 6
                        {1,2,2,3,1,2,0,0},  // 7
                        {1,3,2,2,1,2,0,0},  // 8
                        {2,2,1,2,1,3,0,0},  // 9
                        {2,2,1,3,1,2,0,0},  // 10
                        {2,3,1,2,1,2,0,0},  // 11
                        {1,1,2,2,3,2,0,0},  // 12
                        {1,2,2,1,3,2,0,0},  // 13
                        {1,2,2,2,3,1,0,0},  // 14
                        {1,1,3,2,2,2,0,0},  // 15
                        {1,2,3,1,2,2,0,0},  // 16
                        {1,2,3,2,2,1,0,0},  // 17
                        {2,2,3,2,1,1,0,0},  // 18
                        {2,2,1,1,3,2,0,0},  // 19
                        {2,2,1,2,3,1,0,0},  // 20
                        {2,1,3,2,1,2,0,0},  // 21
                        {2,2,3,1,1,2,0,0},  // 22
                        {3,1,2,1,3,1,0,0},  // 23
                        {3,1,1,2,2,2,0,0},  // 24
                        {3,2,1,1,2,2,0,0},  // 25
                        {3,2,1,2,2,1,0,0},  // 26
                        {3,1,2,2,1,2,0,0},  // 27
                        {3,2,2,1,1,2,0,0},  // 28
                        {3,2,2,2,1,1,0,0},  // 29
                        {2,1,2,1,2,3,0,0},  // 30
                        {2,1,2,3,2,1,0,0},  // 31
                        {2,3,2,1,2,1,0,0},  // 32
                        {1,1,1,3,2,3,0,0},  // 33
                        {1,3,1,1,2,3,0,0},  // 34
                        {1,3,1,3,2,1,0,0},  // 35
                        {1,1,2,3,1,3,0,0},  // 36
                        {1,3,2,1,1,3,0,0},  // 37
                        {1,3,2,3,1,1,0,0},  // 38
                        {2,1,1,3,1,3,0,0},  // 39
                        {2,3,1,1,1,3,0,0},  // 40
                        {2,3,1,3,1,1,0,0},  // 41
                        {1,1,2,1,3,3,0,0},  // 42
                        {1,1,2,3,3,1,0,0},  // 43
                        {1,3,2,1,3,1,0,0},  // 44
                        {1,1,3,1,2,3,0,0},  // 45
                        {1,1,3,3,2,1,0,0},  // 46
                        {1,3,3,1,2,1,0,0},  // 47
                        {3,1,3,1,2,1,0,0},  // 48
                        {2,1,1,3,3,1,0,0},  // 49
                        {2,3,1,1,3,1,0,0},  // 50
                        {2,1,3,1,1,3,0,0},  // 51
                        {2,1,3,3,1,1,0,0},  // 52
                        {2,1,3,1,3,1,0,0},  // 53
                        {3,1,1,1,2,3,0,0},  // 54
                        {3,1,1,3,2,1,0,0},  // 55
                        {3,3,1,1,2,1,0,0},  // 56
                        {3,1,2,1,1,3,0,0},  // 57
                        {3,1,2,3,1,1,0,0},  // 58
                        {3,3,2,1,1,1,0,0},  // 59
                        {3,1,4,1,1,1,0,0},  // 60
                        {2,2,1,4,1,1,0,0},  // 61
                        {4,3,1,1,1,1,0,0},  // 62
                        {1,1,1,2,2,4,0,0},  // 63
                        {1,1,1,4,2,2,0,0},  // 64
                        {1,2,1,1,2,4,0,0},  // 65
                        {1,2,1,4,2,1,0,0},  // 66
                        {1,4,1,1,2,2,0,0},  // 67
                        {1,4,1,2,2,1,0,0},  // 68
                        {1,1,2,2,1,4,0,0},  // 69
                        {1,1,2,4,1,2,0,0},  // 70
                        {1,2,2,1,1,4,0,0},  // 71
                        {1,2,2,4,1,1,0,0},  // 72
                        {1,4,2,1,1,2,0,0},  // 73
                        {1,4,2,2,1,1,0,0},  // 74
                        {2,4,1,2,1,1,0,0},  // 75
                        {2,2,1,1,1,4,0,0},  // 76
                        {4,1,3,1,1,1,0,0},  // 77
                        {2,4,1,1,1,2,0,0},  // 78
                        {1,3,4,1,1,1,0,0},  // 79
                        {1,1,1,2,4,2,0,0},  // 80
                        {1,2,1,1,4,2,0,0},  // 81
                        {1,2,1,2,4,1,0,0},  // 82
                        {1,1,4,2,1,2,0,0},  // 83
                        {1,2,4,1,1,2,0,0},  // 84
                        {1,2,4,2,1,1,0,0},  // 85
                        {4,1,1,2,1,2,0,0},  // 86
                        {4,2,1,1,1,2,0,0},  // 87
                        {4,2,1,2,1,1,0,0},  // 88
                        {2,1,2,1,4,1,0,0},  // 89
                        {2,1,4,1,2,1,0,0},  // 90
                        {4,1,2,1,2,1,0,0},  // 91
                        {1,1,1,1,4,3,0,0},  // 92
                        {1,1,1,3,4,1,0,0},  // 93
                        {1,3,1,1,4,1,0,0},  // 94
                        {1,1,4,1,1,3,0,0},  // 95
                        {1,1,4,3,1,1,0,0},  // 96
                        {4,1,1,1,1,3,0,0},  // 97
                        {4,1,1,3,1,1,0,0},  // 98
                        {1,1,3,1,4,1,0,0},  // 99
                        {1,1,4,1,3,1,0,0},  // 100
                        {3,1,1,1,4,1,0,0},  // 101
                        {4,1,1,1,3,1,0,0},  // 102
                        {2,1,1,4,1,2,0,0},  // 103
                        {2,1,1,2,1,4,0,0},  // 104
                        {2,1,1,2,3,2,0,0},  // 105
                        {2,3,3,1,1,1,2,0}   // 106
                     };

		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 8; } }

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return ((dataLength - 3) * 11 + 35) * barWeight;
		}

		private static readonly Regex _hasControlChars = new Regex(@">[:;<=\d]");
		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrEmpty(inputData))
				return new int[0];

			if(_hasControlChars.IsMatch(inputData))
			{
				return GetValuesExplicit(inputData);
			}
			return GetValuesImplicit(inputData);
		}

		private int[] GetValuesExplicit(string inputData)
		{
			var codes = new List<int>(inputData.Length);

			var currentCodeSet = CodeSet.B;
			for(var c = 0; c < inputData.Length; c++)
			{
				if(inputData[c] == '>')
				{
					var controlString = inputData.Substring(c, 2);
					if(_hasControlChars.IsMatch(controlString))
					{
						switch(controlString)
						{
							case ">;":
							case ">5":
								currentCodeSet = CodeSet.C;
								break;

							case ">9":
							case ">7":
								currentCodeSet = CodeSet.B;
								break;
							case ">:":
							case ">6":
								currentCodeSet = CodeSet.A;
								break;
						}
						codes.Add(ControlCodes.EscapeCharacters[controlString]);
						c++;
						continue;
					}
				}
				codes.Add(GetValue(inputData[c], inputData.Length > c + 1 ? inputData[c + 1] : -1, currentCodeSet));
				if(currentCodeSet == CodeSet.C)
					c++;
			}

			codes.Add(GetChecksum(codes));

			codes.Add(ControlCodes.STOP);

			return codes.ToArray();
		}

		private static int GetChecksum(IList<int> codes)
		{
			var checksum = codes[0];
			for(var i = 1; i < codes.Count; i++)
			{
				checksum += i * codes[i];
			}
			return (checksum % 103);
		}

		private int GetValue(int currentChar, int nextChar, CodeSet currentCodeSet)
		{
			if(currentCodeSet == CodeSet.C)
			{
				return ((currentChar - 48) * 10) + (nextChar - 48);
			}
			return (currentChar >= 32) ? currentChar - 32 : currentChar + 64;
		}

		private int[] GetValuesImplicit(string inputData)
		{
			var codes = new List<int>(inputData.Length + 10);

			var preferredCodeSet = GetTargetCodeSet(inputData[0]);
			if(inputData.Length > 1)
			{
				var nextCodeSet = GetTargetCodeSet(inputData[1]);
				preferredCodeSet = MergeCodeSets(preferredCodeSet, nextCodeSet);
			}

			var currentCodeSet = GetCodeSet(preferredCodeSet);

			codes.Add(ControlCodes.GetStartCode(currentCodeSet));

			for(var c = 0; c < inputData.Length; c++)
			{
				preferredCodeSet = GetTargetCodeSet(inputData[c]);
				var nextCodeSet = TargetCodeSet.Any;
				if(c < inputData.Length - 1)
				{
					nextCodeSet = GetTargetCodeSet(inputData[c]);
					preferredCodeSet = MergeCodeSets(preferredCodeSet, nextCodeSet);

					if(preferredCodeSet == TargetCodeSet.Any && currentCodeSet != CodeSet.C)
					{
						currentCodeSet = CodeSet.C;
						codes.Add(ControlCodes.CODE_C);
					}
				}
				else if(currentCodeSet == CodeSet.C)
				{
					currentCodeSet = CodeSet.B;
					codes.Add(ControlCodes.CODE_B);
				}

				if(!IsCompatible(currentCodeSet, preferredCodeSet))
				{
					if(c < inputData.Length - 1 && currentCodeSet != CodeSet.C && preferredCodeSet != nextCodeSet)
					{
						codes.Add(ControlCodes.SHIFT);
					}
					else
					{
						currentCodeSet = GetCodeSet(preferredCodeSet);
						codes.Add(ControlCodes.GetCodeSet(currentCodeSet));
					}
				}

				codes.Add(GetValue(inputData[c], inputData.Length > c + 1 ? inputData[c + 1] : -1, currentCodeSet));

				if(currentCodeSet == CodeSet.C)
					c++;
			}

			codes.Add(GetChecksum(codes));

			codes.Add(ControlCodes.STOP);

			return codes.ToArray();
		}

		private bool IsCompatible(CodeSet currentCodeSet, TargetCodeSet preferredCodeSet)
		{
			switch(preferredCodeSet)
			{
				case TargetCodeSet.AorB:
					return currentCodeSet != CodeSet.C;
				case TargetCodeSet.A:
					return currentCodeSet == CodeSet.A;
				case TargetCodeSet.B:
					return currentCodeSet == CodeSet.B;
				case TargetCodeSet.Any:
				default:
					return true;
			}
		}

		private TargetCodeSet MergeCodeSets(TargetCodeSet preferredCodeSet, TargetCodeSet nextCodeSet)
		{
			if(preferredCodeSet == TargetCodeSet.A || preferredCodeSet == TargetCodeSet.B)
				return preferredCodeSet;
			if(nextCodeSet == TargetCodeSet.A || nextCodeSet == TargetCodeSet.B)
				return nextCodeSet;
			return preferredCodeSet;
		}

		private TargetCodeSet GetTargetCodeSet(char currentChar)
		{
			if(currentChar >= 48 && currentChar <= 57)
				return TargetCodeSet.Any;
			if(currentChar >= 32 && currentChar <= 95)
				return TargetCodeSet.AorB;
			if(currentChar < 32)
				return TargetCodeSet.A;
			return TargetCodeSet.B;
		}

		private static CodeSet GetCodeSet(TargetCodeSet preferredCodeSet)
		{
			return preferredCodeSet == TargetCodeSet.Any
				? CodeSet.C
				: preferredCodeSet == TargetCodeSet.A
					? CodeSet.A
					: CodeSet.B;
		}
	}
}