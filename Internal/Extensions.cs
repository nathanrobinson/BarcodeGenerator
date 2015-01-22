using System.Collections.Generic;
using System.Linq;

namespace BarcodeGenerator.Internal
{
	public static class Extensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return (source == null || !source.Any());
		}

		public static IEnumerable<T> NullCheck<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		public static T[] SubArray<T>(this IEnumerable<T> source, int start, int length = 0)
		{
			var query = source.NullCheck().Where((x, i) => i >= start);
			if(length > 0)
			{
				var end = start + length;
				query = query.Where((x, i) => i < end);
			}
			return query.ToArray();
		}
	}
}
