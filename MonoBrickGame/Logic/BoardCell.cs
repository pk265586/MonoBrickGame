using System;

namespace MonoTetris.Logic
{
    internal struct BoardCell
    {
        public int Row;
        public int Column;

        public BoardCell(int row, int column) 
        {
            Row = row;
            Column = column;
        }
    }
}
