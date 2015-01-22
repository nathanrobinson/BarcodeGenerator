


namespace BarcodeGenerator._2d
{
	public class BarMatrix2d
	{
		public BarMatrix2d()
		{
			_bars = new bool[0, 0][];
		}

		public BarMatrix2d(int rows, int columns)
		{
			_bars = new bool[0, 0][];
			Rows = rows;
			Columns = columns;
		}

		private int _rows;
		private int _columns;
		private bool[,][] _bars;
		public bool[] this[int row, int column]
		{
			get { return _bars[row, column]; }
			set { _bars[row, column] = value; }
		}

		public int Rows
		{
			get
			{
				return _rows;
			}
			set
			{
				if(_rows == value)
					return;
				_rows = value;
				ResizeBars();
			}
		}
		public int Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				if(_columns == value)
					return;
				_columns = value;
				ResizeBars();
			}
		}

		private void ResizeBars()
		{
			var lBars = new bool[Rows, Columns][];
			var rows = _bars.GetUpperBound(0);
			var cols = _bars.GetUpperBound(1);
			if(rows > 0 && cols > 0)
				for(var r = 0; r < rows; r++)
				{
					if(r < Rows)
					{
						for(var c = 0; c < cols; c++)
						{
							if(c < Columns)
							{
								lBars[r, c] = _bars[r, c];
							}
						}
					}
				}
			_bars = lBars;
		}
	}
}