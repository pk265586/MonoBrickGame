using System.Linq;
using MonoTetris.Utils;

namespace MonoTetris.Logic
{
    internal class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool[,] PlayfieldState { get; }

        public Board(int width, int height) 
        {
            Width = width;
            Height = height;
            PlayfieldState = new bool[Height, Width];
        }

        public void MakeCellBusy(BoardCell cell)
        {
            if (IsValidCell(cell))
                PlayfieldState[cell.Row, cell.Column] = true;
        }

        public bool IsCellBusy(BoardCell cell)
        {
            if (!IsValidCell(cell))
                return true;

            return PlayfieldState[cell.Row, cell.Column];
        }

        private bool IsValidCell(BoardCell cell)
        {
            return MathUtils.IsBetween(cell.Row, 0, PlayfieldState.GetLength(0) - 1) &&
                MathUtils.IsBetween(cell.Column, 0, PlayfieldState.GetLength(1) - 1);
        }

        public int DeleteFullRows() 
        {
            int fullRows = 0;
            for (int idxRow = 0; idxRow < PlayfieldState.GetLength(0); idxRow++)
            {
                if (IsFullRow(idxRow))
                {
                    DeleteRow(idxRow);
                    fullRows++;
                }
            }
            return fullRows;
        }

        private bool IsFullRow(int idxRow)
        {
            return PlayfieldState.Row(idxRow).All(x => x);
        }

        private void DeleteRow(int idxRow)
        {
            for (int currentRow = idxRow; currentRow > 0; currentRow--) 
            {
                PlayfieldState.CopyRow(currentRow - 1, currentRow);
            }
            PlayfieldState.FillRow(0, false);
        }
    }
}
