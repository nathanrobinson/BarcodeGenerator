using System;
using System.Drawing;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class Planet : Postnet
	{
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
					var barHeight = Convert.ToInt32(((Math.Abs(Patterns[code, bar] - 1) * .5m) + .5m) * height);
					var barwidth = barcodeOptions.BarWeight;
					var spcwidth = 2 * barcodeOptions.BarWeight;

					cursor = DrawBar(barcodeOptions, gr, cursor, barHeight, c, barwidth, spcwidth);
				}
			}
			DrawBar(barcodeOptions, gr, cursor, height, -1, barcodeOptions.BarWeight, 2 * barcodeOptions.BarWeight);
			gr.ResetTransform();
		}
	}
}