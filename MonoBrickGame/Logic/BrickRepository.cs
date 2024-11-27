using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MonoTetris.Logic
{
    internal class BrickRepository
    {
        const char brickTileChar = '*';
        List<Brick> brickTemplates;

        ContentManager contentManager;

        Random random;

        public BrickRepository(ContentManager contentManager) 
        {
            this.contentManager = contentManager;
            random = new Random();
        }

        public void LoadBrickTemplates()
        {
            string bricksPath = Path.Combine(contentManager.RootDirectory, "Bricks");
            var brickPaths = Directory.GetFiles(bricksPath);
            brickTemplates = new List<Brick>();
            foreach (var brickFile in brickPaths)
                LoadBrickFile(brickFile);
        }

        private void LoadBrickFile(string brickFile)
        {
            using (Stream fileStream = TitleContainer.OpenStream(brickFile))
                brickTemplates.Add(LoadBrickFromStream(fileStream));

        }

        private Brick LoadBrickFromStream(Stream fileStream)
        {
            var cells = new List<BoardCell>();
            int row = 0;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    for (int column = 0; column < line.Length; column++)
                    {
                        if (line[column] == brickTileChar)
                            cells.Add(new BoardCell(row, column));
                    }
                    row++;
                }
            }

            return new Brick { Cells = cells.ToArray() };
        }

        public Brick GetRandomBrick() 
        {
            int brickIndex = random.Next(brickTemplates.Count);
            return brickTemplates[brickIndex].Clone();
        }
    }
}
