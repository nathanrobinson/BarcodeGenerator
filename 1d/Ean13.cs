using System.Collections.Generic;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Ean13 : UpcA
	{
		private static readonly int[][] _parities =
		{
			new int[0],
			new []{2,4,5},
			new []{2,3,5},
			new []{2,3,4},
			new []{1,4,5},
			new []{1,2,5},
			new []{1,2,3},
			new []{1,3,5},
			new []{1,3,4},
			new []{1,2,4}
		};
		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrWhiteSpace(inputData))
				return new int[0];

			var first = 0;
			if(!int.TryParse(inputData.Substring(0, 1), out first))
				return new int[0];

			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

			while(content.Count < 12)
				content.Add(0);

			if(content.Count > 13)
				content.RemoveRange(13, content.Count - 13);

			if(content.Count % 2 == 0)
			{
				if(barcodeOptions.CalculateCheckDigit)
				{
					barcodeOptions.CheckDigit = CalculateCheckDigit(content);
					content.Add(barcodeOptions.CheckDigit);
				}
				else
				{
					content.Insert(0, 0);
				}
			}

			content.RemoveAt(0);

			foreach(var parity in _parities[first])
			{
				content[parity] += 10;
			}

			content.Insert(0, START_STOP);
			content.Insert(Midpoint, MIDDLE_1);
			content.Insert(Midpoint + 1, MIDDLE_2);
			content.Add(START_STOP);
			return content.ToArray();
		}

		protected override int ClipBar(int i)
		{
			return i == 0 || i == Midpoint || i == Midpoint + 1 || i == (Midpoint * 2 + 1) ? 2 : 7;
		}
	}
}