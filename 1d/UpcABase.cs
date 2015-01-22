using System.Collections.Generic;
using System.Linq;

namespace BarcodeGenerator._1d
{
	public abstract class UpcABase : Base1D
	{
		private static readonly int[,] _patterns =
		{
			{ 3, 2, 1, 1 }, //0
			{ 2, 2, 2, 1 }, //1
			{ 2, 1, 2, 2 }, //2
			{ 1, 4, 1, 1 }, //3
			{ 1, 1, 3, 2 }, //4
			{ 1, 2, 3, 1 }, //5
			{ 1, 1, 1, 4 }, //6
			{ 1, 3, 1, 2 }, //7
			{ 1, 2, 1, 3 }, //8
			{ 3, 1, 1, 2 }, //9

			{ 1, 1, 2, 3 }, //0
			{ 1, 2, 2, 2 }, //1
			{ 2, 2, 1, 2 }, //2
			{ 1, 1, 4, 1 }, //3
			{ 2, 3, 1, 1 }, //4
			{ 1, 3, 2, 1 }, //5
			{ 4, 1, 1, 1 }, //6
			{ 2, 1, 3, 1 }, //7
			{ 3, 1, 2, 1 }, //8
			{ 2, 1, 1, 3 }, //9

			{ 1, 1, 1, 0 }, //START_STOP
			{ 0, 1, 1, 0 }, //MIDDLE_1
			{ 0, 1, 1, 1 }, //MIDDLE_2
			{ 1, 1, 1, 2 }, //START_STOP2
			{ 1, 1, 0, 0 }, //SEP
		};
		protected override int[,] Patterns { get { return _patterns; } }
		protected override int PatternWidth { get { return 4; } }
		protected override int QuietWidth { get { return 9; } }

		protected const int START_STOP = 20;
		protected const int MIDDLE_1 = 21;
		protected const int MIDDLE_2 = 22;
		protected const int START_STOP2 = 23;
		protected const int SEP = 24;

		protected const decimal HEIGHT_SCALE = 25.9m;
		protected const decimal BAR_SCALE = 0.33m;

		protected abstract int Length { get; }

		protected virtual int WidthModules { get { return Length * 7 + 11; } }
		protected override decimal HeightRatio { get { return (HEIGHT_SCALE / BAR_SCALE) / WidthModules; } }
		protected virtual int Midpoint { get { return (Length / 2) + 1; } }
		protected override int CalculateWidth(int dataLength, int barWeight)
		{
			return WidthModules * barWeight;
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