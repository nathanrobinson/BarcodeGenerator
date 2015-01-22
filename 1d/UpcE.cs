using System.Collections.Generic;

using BarcodeGenerator.Options;

namespace BarcodeGenerator._1d
{
	public class UpcE : UpcA
	{
		protected override int WidthModules { get { return 51; } }

		private static readonly int[,][] _parities =
		{
			{new[]{0,1,2}, new []{3,4,5}},
			{new[]{0,1,3}, new []{2,4,5}},
			{new[]{0,1,4}, new []{2,3,5}},
			{new[]{0,1,5}, new []{2,3,4}},
			{new[]{0,2,3}, new []{1,4,5}},
			{new[]{0,3,4}, new []{1,2,5}},
			{new[]{0,4,5}, new []{1,2,3}},
			{new[]{0,2,4}, new []{1,3,5}},
			{new[]{0,2,5}, new []{1,3,4}},
			{new[]{0,3,5}, new []{1,2,4}},
		};

		protected override int[] GetContent(string inputData, BarcodeOptions barcodeOptions)
		{
			if(string.IsNullOrWhiteSpace(inputData) || inputData.Length != 7 || (!inputData.StartsWith("0") && !inputData.StartsWith("1")))
				return new int[0];

			var system = int.Parse(inputData.Substring(0, 1));

			var content = new List<int>();
			content.AddRange(ConvertToNumbers(inputData.Substring(1)));

			while(content.Count < 6)
				content.Add(0);

			if(content.Count > 6)
				content.RemoveRange(6, content.Count - 6);

			barcodeOptions.CheckDigit = CalculateCheckDigit(ExpandCode(system, content));

			foreach(var parity in _parities[barcodeOptions.CheckDigit, system])
			{
				content[parity] += 10;
			}

			content.Insert(0, START_STOP);
			content.Add(MIDDLE_2);
			content.Add(START_STOP);
			return content.ToArray();
		}

		private static int[] ExpandCode(int system, IList<int> content)
		{
			var expanded = new List<int>(12) { system };
			var switcher = content[5];
			for(var c = 0; c < 6; c++)
			{
				if(c < 5 || switcher > 4)
					expanded.Add(content[c]);
				if(c == 1 && switcher < 3)
					expanded.AddRange(new[] { switcher, 0, 0, 0, 0 });
				if((c == 2 && switcher == 3) || (c == 3 && switcher == 4))
					expanded.AddRange(new[] { 0, 0, 0, 0, 0 });
				if(c == 4 && switcher > 4)
					expanded.AddRange(new[] { 0, 0, 0, 0 });
			}
			return expanded.ToArray();
		}

		protected override int ClipBar(int i)
		{
			return i == 0 || i >= 7 ? 2 : 7;
		}
	}
}
