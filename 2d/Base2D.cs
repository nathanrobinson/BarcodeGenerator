using System.Drawing;

using BarcodeGenerator.Interfaces;
using BarcodeGenerator.Options;

namespace BarcodeGenerator._2d
{
	public abstract class Base2d : IBarcodeRendering
	{
		protected abstract float HeightRatio { get; }
		public Image MakeBarcodeImage(string inputData, BarcodeOptions options)
		{
			var matrix = GenerateMatrix(inputData, options);
			var calcHeight = (int) (matrix.Rows * HeightRatio * options.BarWeight);
			var calcWidth = ((matrix.Columns * 17) + 1) * options.BarWeight;
			var offset = new Point(0, 0);
			if(options.AddQuietZone)
			{
				calcHeight += (int) (6 * HeightRatio * options.BarWeight);
				calcWidth += 6 * options.BarWeight;
				offset.X = offset.Y = 3 * options.BarWeight;
			}
			Image dmImage = new Bitmap(calcWidth, calcHeight);
			using(var g = Graphics.FromImage(dmImage))
			{
				g.FillRectangle(Brushes.White, 0, 0, calcWidth, calcHeight);

				for(var row = 0; row < matrix.Rows; row++)
				{
					var colOffset = 0;
					for(var column = 0; column < matrix.Columns; column++)
					{

						var currentChar = matrix[row, column];

						foreach(var b in currentChar)
						{
							if(b)
							{
								g.FillRectangle(
									Brushes.Black,
									offset.X + (colOffset * options.BarWeight),
									offset.Y + (row * HeightRatio * options.BarWeight),
									options.BarWeight,
									options.BarWeight * HeightRatio);
							}
							colOffset++;
						}
					}
				}
				g.Flush();
			}
			return dmImage;
		}

		protected abstract BarMatrix2d GenerateMatrix(string inputData, BarcodeOptions options);
	}
}