using System;
using System.Collections.Generic;
using MonoTetris.Controls;

namespace MonoTetris.Logic
{
    internal class LevelModel
    {
        public Board Board { get; set; }
        public Brick ActiveBrick { get; set; }

        public int Score { get; set; }

        public GameStatus GameStatus { get; set; }

        public int GameSpeed { get; set; }

        public List<GameObject> GameObjects { get; } = new List<GameObject>();
    }
}
