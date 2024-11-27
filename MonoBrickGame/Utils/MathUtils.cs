using System;

namespace MonoTetris.Utils
{
    public static class MathUtils
    {
        public static bool IsBetween<T>(T value, T from, T to) where T : IComparable 
        {
            return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }
    }
}
