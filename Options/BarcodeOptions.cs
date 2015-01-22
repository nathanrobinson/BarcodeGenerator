
using BarcodeGenerator.Enums;

namespace BarcodeGenerator.Options
{
	public class BarcodeOptions
	{
		public int BarWeight { get; set; }
		public bool AddQuietZone { get; set; }
		public bool CalculateCheckDigit { get; set; }
		public int CheckDigit { get; set; }
		public BearerBarType BearerBarType { get; set; }
	}
}