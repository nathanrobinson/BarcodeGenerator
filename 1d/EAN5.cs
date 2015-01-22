using System.Collections.Generic;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Ean5 : UpcA
	{
		protected override int Length { get { return 5; } }
		protected override int WidthModules { get { return Length * 9 + 3; } }

		private static readonly int[][] _parities =
		{
			new[]{0,1},
			new[]{0,2},
			new[]{0,3},
			new[]{0,4},
			new[]{1,2},
			new[]{2,3},
			new[]{3,4},
			new[]{1,3},
			new[]{1,4},
			new[]{2,4},
		};

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrWhiteSpace(inputData))
				return new int[0];

			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

			while(content.Count < 5)
				content.Add(0);

			if(content.Count > 5)
				content.RemoveRange(5, content.Count - 5);

			barcodeOptions.CheckDigit = CalculateCheckDigit(content);

			foreach(var parity in _parities[barcodeOptions.CheckDigit])
			{
				content[parity] += 10;
			}

			content.Insert(0, START_STOP2);
			content.Insert(2, SEP);
			content.Insert(4, SEP);
			content.Insert(6, SEP);
			content.Insert(8, SEP);
			return content.ToArray();
		}

		protected override int ClipBar(int i)
		{
			return 0;
		}

		protected override bool InvertBars(int i)
		{
			return true;
		}

		protected override int CalculateCheckDigit(IEnumerable<int> content)
		{
			var total = content
				.Select((x, i) => new { value = x, index = i })
				.Aggregate(0, (t, x) => t + (x.value * (x.index % 2 == 0 ? 3 : 9)));

			var remainder = total % 10;
			return remainder;
		}
	}
}
