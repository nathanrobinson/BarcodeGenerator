

using System.Collections.Generic;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Code93 : Code39
	{
		private static readonly int[,] _patterns =
		{
		//       B, S, B, S,  B, S
			{1,3,1,1,1,2}, //0
			{1,1,1,2,1,3}, //1
			{1,1,1,3,1,2}, //2
			{1,1,1,4,1,1}, //3
			{1,2,1,1,1,3}, //4
			{1,2,1,2,1,2}, //5
			{1,2,1,3,1,1}, //6
			{1,1,1,1,1,4}, //7
			{1,3,1,2,1,1}, //8
			{1,4,1,1,1,1}, //9
		//       B, S, B, S,  B, S
			{2,1,1,1,1,3}, //10
			{2,1,1,2,1,2}, //11
			{2,1,1,3,1,1}, //12
			{2,2,1,1,1,2}, //13
			{2,2,1,2,1,1}, //14
			{2,3,1,1,1,1}, //15
			{1,1,2,1,1,3}, //16
			{1,1,2,2,1,2}, //17
			{1,1,2,3,1,1}, //18
			{1,2,2,1,1,2}, //19
		//       B, S, B, S,  B, S
			{1,3,2,1,1,1}, //20
			{1,1,1,1,2,3}, //21
			{1,1,1,2,2,2}, //22
			{1,1,1,3,2,1}, //23
			{1,2,1,1,2,2}, //24
			{1,3,1,1,2,1}, //25
			{2,1,2,1,1,2}, //26
			{2,1,2,2,1,1}, //27
			{2,1,1,1,2,2}, //28
			{2,1,1,2,2,1}, //29
		//       B, S, B, S,  B, S
			{2,2,1,1,2,1}, //30
			{2,2,2,1,1,1}, //31
			{1,1,2,1,2,2}, //32
			{1,1,2,2,2,1}, //33
			{1,2,2,1,2,1}, //34
			{1,2,3,1,1,1}, //35
			{1,2,1,1,3,1}, //36
			{3,1,1,1,1,2}, //37
			{3,1,1,2,1,1}, //38
		//       B, S, B, S,  B, S
			{3,2,1,1,1,1}, //39
			{1,1,2,1,3,1}, //40
			{1,1,3,1,2,1}, //41
			{2,1,1,1,3,1}, //42
			{1,2,1,2,2,1}, //43
			{3,1,2,1,1,1}, //44
			{3,1,1,1,2,1}, //45
			{1,2,2,2,1,1}, //46
		//       B, S, B, S,  B, S
			{1,1,1,1,4,1}, // START_STOP
			{1,1,4,1,1,1}, //REV_STOP
		//       B, S, B, S,  B, S
			{4,1,1,1,1,1}, //RESERVED 49 
			{1,1,1,1,3,2}, //RESERVED 50
			{1,1,1,2,3,1}, //RESERVED 51 
			{1,1,3,1,1,2}, //RESERVED 52 
			{1,1,3,2,1,1}, //RESERVED 53 
			{2,1,3,1,1,1}, //RESERVED 54 
			{2,1,2,1,2,1}, //RESERVED 55 
			{1,0,0,0,0,0} //TERMINATE
		};

		private const int START_STOP = 47;
		private const int TERMINATE = 56;
		protected override int StartStop { get { return START_STOP; } }
		protected virtual int Terminate { get { return TERMINATE; } }
		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 6; } }
		protected override int QuietWidth { get { return 9; } }
		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return dataLength * (PatternWidth + 3) * barWeight;
		}

		protected virtual int CalculateCheckDigit(IEnumerable<int> content, int maxWeight)
		{
			var total = content.Select((x, i) => x * (i % maxWeight)).Sum();
			var check = total % 47;
			return check;
		}

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			var content = ConvertToNumbers(inputData).ToList();
			if(inputData.Any(x => !Indexer.Contains(x)))
			{
				content = content.Select(x => x == 39 ? 43 : x == 40 ? 45 : x == 41 ? 46 : x == 42 ? 44 : x).ToList();
			}

			content.Add(CalculateCheckDigit(content, 20));
			content.Add(CalculateCheckDigit(content, 15));
			content.Insert(0, StartStop);
			content.Add(StartStop);
			content.Add(Terminate);

			return content.ToArray();
		}
	}
}