using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoTetris.Controls
{
    public abstract class GameObject
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
