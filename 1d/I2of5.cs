using System.Collections.Generic;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class I2of5 : Base1D
	{
		private static readonly int[,] _patterns = 
		{
			                             {1,1,3,3,1}, //0    
			                             {3,1,1,1,3}, //1
			                             {1,3,1,1,3}, //2    
			                             {3,3,1,1,1}, //3    
			                             {1,1,3,1,3}, //4    
			                             {3,1,3,1,1}, //5    
			                             {1,3,3,1,1}, //6    
			                             {1,1,1,3,3}, //7    
			                             {3,1,1,3,1}, //8    
			                             {1,3,1,3,1}, //9  
										 {1,1,0,0,0}, //START1
										 {1,1,0,0,0}, //START2
										 {3,1,0,0,0}, // STOP1
										 {1,0,0,0,0} //STOP2
		};

		private const int START1 = 10;
		private const int START2 = 11;
		private const int STOP1 = 12;
		private const int STOP2 = 13;

		protected override bool IsInterleaved { get { return true; } }
		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 5; } }
		protected override bool SupportsBearerBars { get { return true; } }

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return ((dataLength - 3) * (PatternWidth + 4) * barWeight);
		}

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData));

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

			content.Insert(0, START1);
			content.Insert(1, START2);
			content.Add(STOP1);
			content.Add(STOP2);
			return content.ToArray();
		}

		protected virtual int CalculateCheckDigit(IEnumerable<int> content)
		{
			var total = content
				.Select((x, i) => new { value = x, index = i })
				.Aggregate(0, (t, x) => t + (x.index % 2 == 0 ? x.value * 3 : x.value));

			var remainder = total % 10;
			var result = remainder > 0 ? 10 - remainder : 0;
			return result;
		}
	}
}