using System.Collections.Generic;

namespace Regulus.Extensions
{
   public static class NumberExtensions
    {
        public static IEnumerable<int> GetSeries(this int count) 
        {
            for (int i = 0 ; i < count; i++)
            {
                yield return i;
            }
        }
    }
}