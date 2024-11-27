using System;
using Microsoft.Xna.Framework;

namespace MonoTetris.Utils
{
    public static class Vector2Utils
    {
        public static Vector2 Truncate(this Vector2 input) 
        {
            return new Vector2((int)input.X, (int)input.Y);
        }

        public static bool IsDefault(this Vector2 input) 
        {
            return input.X == 0 && input.Y == 0;
        }

        public static Vector2 Move(this Vector2 input, int dx, int dy) 
        {
            return new Vector2(input.X + dx, input.Y + dy);
        }
    }
}
