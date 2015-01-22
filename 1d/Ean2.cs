using System.Collections.Generic;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Ean2 : Ean5
	{
		protected override int Length { get { return 2; } }
		protected override int WidthModules { get { return Length * 9 + 3; } }

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrWhiteSpace(inputData))
				return new int[0];

			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

			while(content.Count < 2)
				content.Add(0);

			if(content.Count > 2)
				content.RemoveRange(2, content.Count - 2);

			content[1] += 10;

			content.Insert(0, START_STOP2);
			content.Insert(2, SEP);
			return content.ToArray();
		}
	}
}