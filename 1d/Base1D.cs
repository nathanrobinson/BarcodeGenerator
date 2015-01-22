using System;
using System.Collections.Generic;
using System.Drawing;

using BarcodeGenerator.Enums;
using BarcodeGenerator.Interfaces;
using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public abstract class Base1D : IBarcodeRendering
	{
		public virtual Image MakeBarcodeImage(string inputData, BarcodeOptions options)
		{
			var codes = GetContent(inputData, options);

			var width = CalculateWidth(codes.Length, options.BarWeight);
			var height = Convert.ToInt32(Math.Ceiling(width * HeightRatio)) + (StaticHeight * options.BarWeight);

			if(options.AddQuietZone)
			{
				width += 2 * QuietWidth * options.BarWeight;
			}

			var bearerBarWidth = options.BarWeight * 2;

			if(SupportsBearerBars && options.BearerBarType != BearerBarType.None)
			{
				height += (2 * bearerBarWidth);

				if(options.BearerBarType == BearerBarType.Box)
				{
					width += (2 * bearerBarWidth);
				}
			}

			Image barcodeImage = new Bitmap(width, height);
			using(var gr = Graphics.FromImage(barcodeImage))
			{
				var whiteRect = new Rectangle(0, 0, width, height);

				if(SupportsBearerBars && options.BearerBarType != BearerBarType.None)
				{
					gr.FillRectangle(Brushes.Black, whiteRect);
					whiteRect.Y = bearerBarWidth;
					whiteRect.Height -= 2 * bearerBarWidth;

					if(options.BearerBarType == BearerBarType.Box)
					{
						whiteRect.X = bearerBarWidth;
						whiteRect.Width -= 2 * bearerBarWidth;
					}
				}

				gr.FillRectangle(Brushes.White, whiteRect);

				var cursor = options.AddQuietZone ? QuietWidth * options.BarWeight : 0;

				if(IsInterleaved)
				{
					DrawInterleaved(options, codes, gr, cursor, height);
				}
				else
				{
					DrawNonInterleaved(options, codes, gr, cursor, height);
				}
			}

			return barcodeImage;
		}

		protected virtual int StaticHeight { get { return 0; } }

		protected virtual void DrawInterleaved(BarcodeOptions barcodeOptions, int[] codes, Graphics gr, int cursor, int height)
		{
			for(var c = 0; c < codes.Length; c += 2)
			{
				var oddCodes = codes[c];
				var evenCodes = codes[c + 1];

				for(var bar = 0; bar < PatternWidth; bar++)
				{
					var barwidth = Patterns[oddCodes, bar] * barcodeOptions.BarWeight;
					var spcwidth = Patterns[evenCodes, bar] * barcodeOptions.BarWeight;

					cursor = DrawBar(barcodeOptions, gr, cursor, height, c, barwidth, spcwidth);
				}
			}
		}

		protected virtual int DrawBar(BarcodeOptions barcodeOptions, Graphics gr, int cursor, int height, int c, int barwidth, int spcwidth)
		{
			var clip = ClipBar(c);
			if(clip > 0)
			{
				height -= clip * barcodeOptions.BarWeight;
			}
			var invertBars = InvertBars(c);

			if(!invertBars && barwidth > 0)
			{
				gr.FillRectangle(Brushes.Black, cursor, 0, barwidth, height);
			}

			cursor += barwidth;

			if(invertBars && spcwidth > 0)
			{
				gr.FillRectangle(Brushes.Black, cursor, 0, spcwidth, height);
			}

			cursor += spcwidth;
			return cursor;
		}

		protected virtual void DrawNonInterleaved(BarcodeOptions barcodeOptions, int[] codes, Graphics gr, int cursor, int height)
		{
			for(var c = 0; c < codes.Length; c++)
			{
				var code = codes[c];

				for(var bar = 0; bar < PatternWidth; bar += 2)
				{
					var barwidth = Patterns[code, bar] * barcodeOptions.BarWeight;
					var spcwidth = (bar == 8 ? 1 : Patterns[code, bar + 1]) * barcodeOptions.BarWeight;

					cursor = DrawBar(barcodeOptions, gr, cursor, height, c, barwidth, spcwidth);
				}
			}
		}

		protected virtual bool IsInterleaved { get { return false; } }
		protected abstract int[,] Patterns { get; }
		protected abstract int PatternWidth { get; }
		protected abstract int CalculateWidth(int dataLength, int barWeight);
		protected virtual int QuietWidth { get { return 10; } }
		protected virtual decimal HeightRatio { get { return 0.15m; } }
		protected virtual bool SupportsBearerBars { get { return false; } }
		protected abstract int[] GetContent(string inputData, BarcodeOptions barcodeOptions);
		protected virtual bool InvertBars(int i) { return false; }
		protected virtual int ClipBar(int i) { return 0; }

		protected virtual int[] ConvertToNumbers(string inputData)
		{
			var numbers = new List<int>();
			foreach(var character in inputData)
			{
				var number = 0;
				if(int.TryParse(character.ToString(), out number))
				{
					numbers.Add(number);
				}
			}
			return numbers.ToArray();
		}
	}
}