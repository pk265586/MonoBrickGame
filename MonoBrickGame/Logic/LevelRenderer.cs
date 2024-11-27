using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTetris.Utils;

namespace MonoTetris.Logic
{
    internal class LevelRenderer
    {
        ContentManager contentManager;
        GraphicsDeviceManager graphics;
        LevelModel level;

        Texture2D borderTexture;
        Texture2D activeTileTexture;
        Texture2D inactiveTileTexture;

        Rectangle playFieldRect;
        Rectangle borderRect;

        private SpriteFont hudFont;

        Vector2 hudLocation;

        Color scoreColor = Color.Maroon;
        Color speedColor = Color.Maroon;

        public LevelRenderer(ContentManager contentManager, GraphicsDeviceManager graphics, LevelModel levelModel)
        {
            this.contentManager = contentManager;
            this.graphics = graphics;
            this.level = levelModel;

            LoadContent();
            SetBorderRect();

            hudLocation = new Vector2(10, 10);
        }

        private void LoadContent()
        {
            borderTexture = contentManager.Load<Texture2D>("Border");
            activeTileTexture = contentManager.Load<Texture2D>("TetrisTile");
            inactiveTileTexture = contentManager.Load<Texture2D>("TetrisInactiveTile");

            hudFont = contentManager.Load<SpriteFont>("Fonts/Hud");
        }

        private void SetBorderRect()
        {
            playFieldRect.SetSize(level.Board.Width, level.Board.Height, scale: GameConst.TileSize);
            playFieldRect.SetCenter(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            borderRect = new Rectangle(
                playFieldRect.X - GameConst.TileSize,
                playFieldRect.Y,
                playFieldRect.Width + GameConst.TileSize * 2,
                playFieldRect.Height + GameConst.TileSize);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap);

            DrawBoard(spriteBatch);
            DrawActiveBrick(spriteBatch);
            DrawPlayField(spriteBatch);

            level.GameObjects.ForEach(gameObject => gameObject.Draw(gameTime, spriteBatch));

            DrawHud(spriteBatch);

            spriteBatch.End();
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            DrawScore(spriteBatch);
            DrawSpeed(spriteBatch);

            if (level.GameStatus == GameStatus.Over)
                DrawGameOver(spriteBatch);
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            string scoreString = $"Score: {level.Score:#,##0}";
            DrawShadowedString(spriteBatch, hudFont, scoreString, hudLocation, scoreColor);
        }

        private void DrawSpeed(SpriteBatch spriteBatch)
        {
            DrawShadowedString(spriteBatch, hudFont, $"Speed: {level.GameSpeed}", hudLocation.Move(0, 50), speedColor);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            string text = "Game Over";
            int scale = 5;
            var textSize = hudFont.MeasureString(text) * scale;
            var position = new Vector2((graphics.PreferredBackBufferWidth - textSize.X) / 2, 50);
            DrawShadowedString(spriteBatch, hudFont, text, position, Color.Black, 5);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color, int scale = 1)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, value, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        private void DrawBoard(SpriteBatch spriteBatch)
        {
            var ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            spriteBatch.DrawWithRepeat(borderTexture,
                new Rectangle(borderRect.X, borderRect.Y, GameConst.TileSize, borderRect.Height));

            spriteBatch.DrawWithRepeat(borderTexture,
                new Rectangle(borderRect.Right - GameConst.TileSize, borderRect.Y, GameConst.TileSize, borderRect.Height));

            spriteBatch.DrawWithRepeat(borderTexture,
                RectangleUtils.CreateByBottomLeft(borderRect.X + GameConst.TileSize, borderRect.Bottom, borderRect.Width - GameConst.TileSize * 2, GameConst.TileSize));
        }

        private void DrawActiveBrick(SpriteBatch spriteBatch)
        {
            if (level.ActiveBrick != null)
                level.ActiveBrick.Cells.ForEach(c => DrawActiveTile(spriteBatch, c));
        }

        private void DrawPlayField(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < level.Board.PlayfieldState.GetLength(0); row++)
            {
                for (int column = 0; column < level.Board.PlayfieldState.GetLength(1); column++)
                {
                    if (level.Board.PlayfieldState[row, column])
                        DrawInactiveTile(spriteBatch, new BoardCell(row, column));
                }
            }
        }

        private void DrawActiveTile(SpriteBatch spriteBatch, BoardCell cell)
        {
            DrawTile(spriteBatch, activeTileTexture, cell);
        }

        private void DrawInactiveTile(SpriteBatch spriteBatch, BoardCell cell)
        {
            DrawTile(spriteBatch, inactiveTileTexture, cell);
        }

        private void DrawTile(SpriteBatch spriteBatch, Texture2D texture, BoardCell cell)
        {
            var rectangle = playFieldRect.SubRectangle(cell.Column, cell.Row, 1, 1, GameConst.TileSize);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
