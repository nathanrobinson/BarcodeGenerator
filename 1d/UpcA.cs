using System.Collections.Generic;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class UpcA : UpcABase
	{
		protected override int Length { get { return 12; } }
		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

			while(content.Count < Length - 1)
				content.Add(0);

			if(content.Count > Length)
				content.RemoveRange(Length, content.Count - Length);

			if(content.Count % 2 == 1)
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

			content.Insert(0, START_STOP);
			content.Insert(Midpoint, MIDDLE_1);
			content.Insert(Midpoint + 1, MIDDLE_2);
			content.Add(START_STOP);
			return content.ToArray();
		}

		protected override int ClipBar(int i)
		{
			return i == 0 || i == Midpoint || i == Midpoint + 1 || i == (Midpoint * 2 + 1) ? 2 :
				i > 1 && i < Midpoint * 2 ? 7 :
				0;
		}

		protected override bool InvertBars(int i)
		{
			return i > 0 && i < Midpoint;
		}
	}
}