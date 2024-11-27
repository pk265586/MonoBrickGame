using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTetris.Logic;

namespace MonoTetris
{
    public class MonoTetrisGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private LevelLogic level;

        private KeyboardState keyboardState;

        public MonoTetrisGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            level = new LevelLogic(Content, _graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Handle polling for our input and handling high-level input
            HandleInput(gameTime);

            // update our level, passing down the GameTime along with all of our input states
            level.Update(gameTime, keyboardState);

            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // The drawing code is here

            level.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
