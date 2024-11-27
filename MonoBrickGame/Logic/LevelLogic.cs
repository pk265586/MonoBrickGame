using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoTetris.Utils;
using Microsoft.Xna.Framework.Input;
using MonoTetris.Controls;
using System.Reflection.Metadata;
using System.Reflection.Emit;

namespace MonoTetris.Logic
{
    internal class LevelLogic
    {
        LevelModel level;
        LevelRenderer renderer;
        ContentManager contentManager;
        GraphicsDeviceManager graphics;

        BrickRepository brickRepository;

        const float MoveDownSpeedFactor = 0.2f;
        const float ControlSpeedFactor = 1f;
        const int StartSpeed = 5;
        const int MaxHoldCount = 3;

        KeyboardState previousKeyState;

        int leftHoldCount = 0;
        int rightHoldCount = 0;

        public LevelLogic(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            this.contentManager = contentManager;
            this.graphics = graphics;
            brickRepository = new BrickRepository(contentManager);
            LoadContent();

            Reset();
        }

        private void Reset()
        {
            level = new LevelModel
            {
                Board = new Board(GameConst.BoardWidth, GameConst.BoardHeight),
                GameStatus = GameStatus.Active,
                GameSpeed = StartSpeed
            };

            renderer = new LevelRenderer(contentManager, graphics, level);

            AddButton(10, 100, "Speed +", () => ChangeSpeed(1));
            AddButton(10, 150, "Speed -", () => ChangeSpeed(-1));
            AddButton(10, 200, "New Game", Reset);
        }

        private void ChangeSpeed(int delta)
        {
            level.GameSpeed += delta;
        }

        private void AddButton(int x, int y, string text, Action onClick) 
        {
            var button = new Button(contentManager.Load<Texture2D>("Button"), contentManager.Load<SpriteFont>("Fonts/Hud"))
            {
                Position = new Vector2(x, y),
                Text = text,

            };
            level.GameObjects.Add(button);
            button.Click += (s, e) => onClick();
        }

        private void LoadContent()
        {
            brickRepository.LoadBrickTemplates();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            renderer.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (level.GameStatus == GameStatus.Active)
            {
                if (level.ActiveBrick == null)
                    CreateActiveBrick();
                else
                    MoveActiveBrick(gameTime, keyboardState);
            }

            level.GameObjects.ForEach(x => x.Update(gameTime));

            previousKeyState = keyboardState;
        }

        private void MoveActiveBrick(GameTime gameTime, KeyboardState keyboardState)
        {
            ApplyControls(gameTime, keyboardState);
            AutoMoveDownBrick(gameTime, keyboardState);
        }

        private void ApplyControls(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up) && previousKeyState.IsKeyUp(Keys.Up))
                TryRotateActiveBrick();

            float keyHoldMoveDistance = level.GameSpeed * ControlSpeedFactor * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float dx = 0;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                float moveDistance = IncrementHoldCountAndGetMoveDistance(ref leftHoldCount, keyHoldMoveDistance);
                dx -= moveDistance;
            }
            else
                leftHoldCount = 0;

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                float moveDistance = IncrementHoldCountAndGetMoveDistance(ref rightHoldCount, keyHoldMoveDistance);
                dx += moveDistance;
            }
            else
                rightHoldCount = 0;

            if (dx != 0)
                TryMoveActiveBrick(dx, 0);
        }

        private float IncrementHoldCountAndGetMoveDistance(ref int holdCount, float keyHoldMoveDistance)
        {
            holdCount = Math.Min(holdCount + 1, MaxHoldCount);
            float moveDistance = holdCount == 1 ? 1f :
                holdCount == MaxHoldCount ? keyHoldMoveDistance :
                0f;
            return moveDistance;
        }

        private void TryRotateActiveBrick()
        {
            var newBrick = level.ActiveBrick.Clone();
            newBrick.Rotate();
            newBrick.FitToRectangle(new Rectangle(0, 0, level.Board.Width, level.Board.Height));
            if (!IntersectsWithField(newBrick))
                level.ActiveBrick = newBrick;
        }

        private void AutoMoveDownBrick(GameTime gameTime, KeyboardState keyboardState)
        {
            float moveDistance = level.GameSpeed * MoveDownSpeedFactor * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Down))
                moveDistance += level.GameSpeed * ControlSpeedFactor * 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(moveDistance) > 1)
                moveDistance = Math.Sign(moveDistance);

            if (!TryMoveActiveBrick(0, moveDistance))
                StopActiveBrick();
        }

        private bool TryMoveActiveBrick(float dx, float dy)
        {
            var newBrick = level.ActiveBrick.CloneAndMove(dx, dy);
            bool canMove = !IntersectsWithField(newBrick);
            if (canMove)
                level.ActiveBrick = newBrick;

            return canMove;
        }

        private void StopActiveBrick()
        {
            level.ActiveBrick.Cells.ForEach(level.Board.MakeCellBusy);
            level.ActiveBrick = null;

            int rowsDeleted = level.Board.DeleteFullRows();
            level.Score += GetScoreForRows(rowsDeleted);
        }

        private int GetScoreForRows(int rowsDeleted)
        {
            return 10 * ((int)Math.Pow(2, rowsDeleted) - 1);
        }

        private void CreateActiveBrick()
        {
            var newBrick = brickRepository.GetRandomBrick();

            int spaceToRight = level.Board.Width - newBrick.Cells.Max(c => c.Column);
            newBrick.Move(spaceToRight / 2, 0);

            level.ActiveBrick = newBrick;

            if (IntersectsWithField(level.ActiveBrick))
                level.GameStatus = GameStatus.Over;
        }

        private bool IntersectsWithField(Brick activeBrick)
        {
            return activeBrick.Cells.Any(level.Board.IsCellBusy);
        }
    }
}
