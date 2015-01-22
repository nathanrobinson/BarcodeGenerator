using System;
using System.Collections.Generic;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Code39 : Base1D
	{
		private static readonly int[,] _patterns =
		{
		//       B  S  B  S   B  S  B  S  B
			{1,1,1,3,3,1,3,1,1, 1}, //0
			{3,1,1,3,1,1,1,1,3, 1}, //1
			{1,1,3,3,1,1,1,1,3, 1}, //2
			{3,1,3,3,1,1,1,1,1, 1}, //3
			{1,1,1,3,3,1,1,1,3, 1}, //4
			{3,1,1,3,3,1,1,1,1, 1}, //5
			{1,1,3,3,3,1,1,1,1, 1}, //6
			{1,1,1,3,1,1,3,1,3, 1}, //7
			{3,1,1,3,1,1,3,1,1, 1}, //8
			{1,1,3,3,1,1,3,1,1, 1}, //9
		//       B  S  B  S   B  S  B  S  B
			{3,1,1,1,1,3,1,1,3, 1}, //10
			{1,1,3,1,1,3,1,1,3, 1}, //11
			{3,1,3,1,1,3,1,1,1, 1}, //12
			{1,1,1,1,3,3,1,1,3, 1}, //13
			{3,1,1,1,3,3,1,1,1, 1}, //14
			{1,1,3,1,3,3,1,1,1, 1}, //15
			{1,1,1,1,1,3,3,1,3, 1}, //16
			{3,1,1,1,1,3,3,1,1, 1}, //17
			{1,1,3,1,1,3,3,1,1, 1}, //18
			{1,1,1,1,3,3,3,1,1, 1}, //19
		//       B  S  B  S   B  S  B  S  B
			{3,1,1,1,1,1,1,3,3, 1}, //20
			{1,1,3,1,1,1,1,3,3, 1}, //21
			{3,1,3,1,1,1,1,3,1, 1}, //22
			{1,1,1,1,3,1,1,3,3, 1}, //23
			{3,1,1,1,3,1,1,3,1, 1}, //24
			{1,1,3,1,3,1,1,3,1, 1}, //25
			{1,1,1,1,1,1,3,3,3, 1}, //26
			{3,1,1,1,1,1,3,3,1, 1}, //27
			{1,1,3,1,1,1,3,3,1, 1}, //28
			{1,1,1,1,3,1,3,3,1, 1}, //29
		//       B  S  B  S   B  S  B  S  B
			{3,3,1,1,1,1,1,1,3, 1}, //30
			{1,3,3,1,1,1,1,1,3, 1}, //31
			{3,3,3,1,1,1,1,1,1, 1}, //32
			{1,3,1,1,3,1,1,1,3, 1}, //33
			{3,3,1,1,3,1,1,1,1, 1}, //34
			{1,3,3,1,3,1,1,1,1, 1}, //35
			{1,3,1,1,1,1,3,1,3, 1}, //36
			{3,3,1,1,1,1,3,1,1, 1}, //37
			{1,3,3,1,1,1,3,1,1, 1}, //38
		//       B  S  B  S   B  S  B  S  B
			{1,3,1,3,1,3,1,1,1, 1}, //39
			{1,3,1,3,1,1,1,3,1, 1}, //40
			{1,3,1,1,1,3,1,3,1, 1}, //41
			{1,1,1,3,1,3,1,3,1, 1}, //42
			{1,3,1,1,3,1,3,1,1, 1 } // START_STOP
		};

		private static readonly Tuple<char, string>[] _asciiReplacements =
		{
			new Tuple<char, string>((char)0, "%U"),
			new Tuple<char, string>((char)1, "$A"),
			new Tuple<char, string>((char)2, "$B"),
			new Tuple<char, string>((char)3, "$C"),
			new Tuple<char, string>((char)4, "$D"),
			new Tuple<char, string>((char)5, "$E"),
			new Tuple<char, string>((char)6, "$F"),
			new Tuple<char, string>((char)7, "$G"),
			new Tuple<char, string>((char)8, "$H"),
			new Tuple<char, string>((char)9, "$I"),
			new Tuple<char, string>((char)10, "$J"),
			new Tuple<char, string>((char)11, "$K"),
			new Tuple<char, string>((char)12, "$L"),
			new Tuple<char, string>((char)13, "$M"),
			new Tuple<char, string>((char)14, "$N"),
			new Tuple<char, string>((char)15, "$O"),
			new Tuple<char, string>((char)16, "$P"),
			new Tuple<char, string>((char)17, "$Q"),
			new Tuple<char, string>((char)18, "$R"),
			new Tuple<char, string>((char)19, "$S"),
			new Tuple<char, string>((char)20, "$T"),
			new Tuple<char, string>((char)21, "$U"),
			new Tuple<char, string>((char)22, "$V"),
			new Tuple<char, string>((char)23, "$W"),
			new Tuple<char, string>((char)24, "$X"),
			new Tuple<char, string>((char)25, "$Y"),
			new Tuple<char, string>((char)26, "$Z"),

			new Tuple<char, string>('!', "/A"),
			new Tuple<char, string>('"', "/B"),
			new Tuple<char, string>('#', "/C"),
			new Tuple<char, string>('$', "/D"),
			new Tuple<char, string>('%', "/E"),
			new Tuple<char, string>('&', "/F"),
			new Tuple<char, string>('\'', "/G"),
			new Tuple<char, string>('(', "/H"),
			new Tuple<char, string>(')', "/I"),
			new Tuple<char, string>('*', "/J"),
			new Tuple<char, string>('+', "/K"),
			new Tuple<char, string>(',', "/L"),
			new Tuple<char, string>('!', "/O"),
			new Tuple<char, string>(':', "/Z"),
			
			new Tuple<char, string>((char)27, "%A"),
			new Tuple<char, string>((char)28, "%B"),
			new Tuple<char, string>((char)29, "%C"),
			new Tuple<char, string>((char)30, "%D"),
			new Tuple<char, string>((char)31, "%E"),
			new Tuple<char, string>(';', "%F"),
			new Tuple<char, string>('<', "%G"),
			new Tuple<char, string>('=', "%H"),
			new Tuple<char, string>('>', "%I"),
			new Tuple<char, string>('?', "%J"),
			new Tuple<char, string>('[', "%K"),
			new Tuple<char, string>('\\', "%L"),
			new Tuple<char, string>(']', "%M"),
			new Tuple<char, string>('^', "%N"),
			new Tuple<char, string>('_', "%O"),
			new Tuple<char, string>('{', "%P"),
			new Tuple<char, string>('|', "%Q"),
			new Tuple<char, string>('}', "%R"),
			new Tuple<char, string>('~', "%S"),
			new Tuple<char, string>((char)127, "%T"),
			
			new Tuple<char, string>('a', "+A"),
			new Tuple<char, string>('b', "+B"),
			new Tuple<char, string>('c', "+C"),
			new Tuple<char, string>('d', "+D"),
			new Tuple<char, string>('e', "+E"),
			new Tuple<char, string>('f', "+F"),
			new Tuple<char, string>('g', "+G"),
			new Tuple<char, string>('h', "+H"),
			new Tuple<char, string>('i', "+I"),
			new Tuple<char, string>('j', "+J"),
			new Tuple<char, string>('k', "+K"),
			new Tuple<char, string>('l', "+L"),
			new Tuple<char, string>('m', "+M"),
			new Tuple<char, string>('n', "+N"),
			new Tuple<char, string>('o', "+O"),
			new Tuple<char, string>('p', "+P"),
			new Tuple<char, string>('q', "+Q"),
			new Tuple<char, string>('r', "+R"),
			new Tuple<char, string>('s', "+S"),
			new Tuple<char, string>('t', "+T"),
			new Tuple<char, string>('u', "+U"),
			new Tuple<char, string>('v', "+V"),
			new Tuple<char, string>('w', "+W"),
			new Tuple<char, string>('x', "+X"),
			new Tuple<char, string>('y', "+Y"),
			new Tuple<char, string>('z', "+Z"),
		};

		private const string _indexer = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
		private const int START_STOP = 43;
		protected virtual int StartStop { get { return START_STOP; } }

		protected virtual string Indexer { get { return _indexer; } }
		protected virtual Tuple<char, string>[] AsciiReplacements { get { return _asciiReplacements; } }

		protected virtual int CalculateCheckDigit(IEnumerable<int> content)
		{
			var total = content.Sum();
			var check = total % 43;
			return check;
		}

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

			if(barcodeOptions.CalculateCheckDigit)
			{
				barcodeOptions.CheckDigit = CalculateCheckDigit(content);
				content.Add(barcodeOptions.CheckDigit);
			}
			content.Insert(0, StartStop);
			content.Add(StartStop);

			return content.ToArray();
		}

		protected virtual int[] ConvertToNumbers(string inputData)
		{
			if(string.IsNullOrEmpty(inputData))
				return new int[0];
			if(inputData.Any(x => !Indexer.Contains(x)))
			{
				inputData = AsciiReplacements.Aggregate(
					inputData,
					(current, asciiReplacement) => current.Replace(string.Empty + asciiReplacement.Item1, asciiReplacement.Item2));
			}
			var codes = inputData.Select(c => Indexer.IndexOf(c)).Where(value => value >= 0).ToArray();
			return codes;
		}

		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 10; } }

		protected override int QuietWidth { get { return 9; } }

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return dataLength * (PatternWidth + 6) * barWeight;
		}
	}
}