using System;
using Microsoft.Xna.Framework;

namespace MonoTetris.Utils
{
    public static class RectangleUtils
    {
        public static void SetSize(this ref Rectangle rectangle, int width, int height, int scale = 1) 
        {
            rectangle.Width = width * scale;
            rectangle.Height = height * scale;
        }

        public static void SetCenter(this ref Rectangle rectangle, int x, int y, int scale = 1) 
        {
            rectangle.X = x * scale - rectangle.Width / 2;
            rectangle.Y = y * scale - rectangle.Height / 2;
        }

        public static Rectangle CreateByBottomLeft(int left, int bottom, int width, int height) 
        {
            return new Rectangle(left, bottom - height, width, height);
        }

        public static Rectangle SubRectangle(this Rectangle origin, int x, int y, int width, int height, int scale = 1) 
        {
            return new Rectangle(origin.X + x * scale, origin.Y + y * scale, width * scale, height * scale);
        }
    }
}
