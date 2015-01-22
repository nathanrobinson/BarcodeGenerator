using System.Drawing;

using BarcodeGenerator.Options;

namespace BarcodeGenerator.Interfaces
{
	public interface IBarcodeRendering
	{
		/// <summary>
		/// Make an image of a Code128 barcode for a given string
		/// </summary>
		/// <param name="inputData">Message to be encoded</param>
		/// <param name="options"></param>
		/// <returns>An Image of the Code128 barcode representing the message</returns>
		Image MakeBarcodeImage(string inputData, BarcodeOptions options);
	}
}