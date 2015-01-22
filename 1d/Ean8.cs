namespace BarcodeGenerator._1d
{
	public class Ean8 : UpcA
	{
		protected override int Length { get { return 8; } }

		protected override int ClipBar(int i)
		{
			return i == 0 || i == Midpoint || i == Midpoint + 1 || i == (Midpoint * 2 + 1) ? 2 : 7;
		}

	}
}