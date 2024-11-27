using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTetris.Utils
{
    public static class SpriteBatchExt
    {
        public static void DrawWithRepeat(this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color? color = null) 
        {
            spriteBatch.Draw(texture, rectangle, rectangle, color ?? Color.White);
        }
    }
}
