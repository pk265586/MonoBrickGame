using System;
using System.Collections.Generic;

namespace MonoTetris.Utils
{
    public static class EnumerableUtils
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action) 
        {
            foreach (var item in sequence)
                action(item);
        }
    }
}
