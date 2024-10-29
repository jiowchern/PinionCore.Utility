using System.Collections.Generic;

namespace PinionCore.Extensions
{
    public static class NumberExtensions
    {
        public static IEnumerable<int> GetSeries(this int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return i;
            }
        }
    }
}
