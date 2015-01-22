using System.Collections.Generic;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class MSI : Base1D
	{
		private static readonly int[,] _patterns =
		{
			{1,2,1,2,1,2,1,2}, //0
			{1,2,1,2,1,2,2,1}, //1
			{1,2,1,2,2,1,1,2}, //2
			{1,2,1,2,2,1,2,1}, //3
			{1,2,2,1,1,2,1,2}, //4
			{1,2,2,1,1,2,2,1}, //5
			{1,2,2,1,2,1,1,2}, //6
			{1,2,2,1,2,1,2,1}, //7
			{2,1,1,2,1,2,1,2}, //8
			{2,1,1,2,1,2,2,1}, //9
			{2,1,0,0,0,0,0,0}, //START
			{1,2,1,0,0,0,0,0} //STOP
		};

		private const int START = 10;
		private const int STOP = 11;

		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 8; } }

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return ((dataLength - 2) * (PatternWidth + 4) + 7) * barWeight;
		}

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			var content = ConvertToNumbers(inputData).ToList();
			if(barcodeOptions.CalculateCheckDigit)
			{
				barcodeOptions.CheckDigit = CalculateCheckDigit(content);
				content.Add(barcodeOptions.CheckDigit);
			}
			content.Insert(0, START);
			content.Add(STOP);
			return content.ToArray();
		}
		protected virtual int CalculateCheckDigit(IEnumerable<int> content)
		{
			var total = content.Sum();

			var remainder = total % 10;
			var result = remainder > 0 ? 10 - remainder : 0;
			return result;
		}
	}
}