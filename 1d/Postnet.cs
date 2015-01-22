using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Postnet : Base1D
	{
		private static readonly int[,] _patterns = {
													   {1,1,0,0,0}, //0 
			                                           {0,0,0,1,1}, //1
													   {0,0,1,0,1}, //2
													   {0,0,1,1,0}, //3
													   {0,1,0,0,1}, //4
													   {0,1,0,1,0}, //5
													   {0,1,1,0,0}, //6
													   {1,0,0,0,1}, //7
													   {1,0,0,1,0}, //8
													   {1,0,1,0,0} //9
												   };
		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 5; } }

		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return dataLength * 15 * barWeight + 4;
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
			return content.ToArray();
		}

		protected override void DrawNonInterleaved(BarcodeOptions barcodeOptions, int[] codes, Graphics gr, int cursor, int height)
		{
			gr.ScaleTransform(1f, -1f);
			gr.TranslateTransform(0f, -(float) height);
			cursor = DrawBar(barcodeOptions, gr, cursor, height, -1, barcodeOptions.BarWeight, 2 * barcodeOptions.BarWeight);
			for(var c = 0; c < codes.Length; c++)
			{
				var code = codes[c];

				for(var bar = 0; bar < PatternWidth; bar++)
				{
					var barHeight = Convert.ToInt32(((Patterns[code, bar] * .5m) + .5m) * height);
					var barwidth = barcodeOptions.BarWeight;
					var spcwidth = 2 * barcodeOptions.BarWeight;

					cursor = DrawBar(barcodeOptions, gr, cursor, barHeight, c, barwidth, spcwidth);
				}
			}
			DrawBar(barcodeOptions, gr, cursor, height, -1, barcodeOptions.BarWeight, 2 * barcodeOptions.BarWeight);
			gr.ResetTransform();
		}
		protected virtual int CalculateCheckDigit(IEnumerable<int> content)
		{
			var total = content.Sum();

			var remainder = total % 10;
			var result = remainder > 0 ? 10 - remainder : 0;
			return result;
		}

		protected override decimal HeightRatio { get { return 0; } }
		protected override int StaticHeight { get { return 10; } }
	}
}