using System;
using System.Collections.Generic;
using System.Linq;


namespace PinionCore.Extensions
{
    public static class Linq
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> set)
        {
            return set.OrderBy((d) => PinionCore.Utility.Random.Instance.NextDouble());
        }

        public static int Index<T>(this IEnumerable<T> set, Func<T, bool> condition)
        {
            Func<T, bool> instance = condition;
            var index = 0;
            foreach (T item in set)
            {
                if (instance(item))
                {
                    return index;
                }
                index++;
            }
            throw new Exception("The container does not meet the conditions.");
        }
    }
}
