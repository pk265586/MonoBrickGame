using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoTetris.Utils;

namespace MonoTetris.Logic
{
    internal class Brick
    {
        public BoardCell[] Cells { get; set; }

        private Vector2 shift;

        public Brick Clone() 
        {
            var newBrick = (Brick)MemberwiseClone();
            newBrick.Cells = (BoardCell[])Cells.Clone();
            return newBrick;
        }

        /*private string GetState() 
        {
            return $"({Cells[0].Column}, {shift.X:0.####})";
        }

        private void ReportState(string prefix) 
        {
            Debug.WriteLine($"{DateTime.Now:hh:mm:ss.fff} {prefix} {GetState()}");
        }*/

        public void Move(float dx, float dy) 
        {
            /*if (dx != 0)
                ReportState($"Before Move [{dx:0.####}]");*/

            shift.X += dx;
            shift.Y += dy;

            var intShift = shift.Truncate();
            if (!intShift.IsDefault())
            {
                int dxInt = (int)intShift.X;
                int dyInt = (int)intShift.Y;

                MoveInt(dxInt, dyInt);

                if (dxInt != 0)
                    shift.X = 0;

                if (dyInt != 0)
                    shift.Y = 0;
            }

            /*if (dx != 0)
                ReportState($" After Move [{dx:0.####}]");*/
        }

        private void MoveInt(int dxInt, int dyInt)
        {
            for (int idx = 0; idx < Cells.Length; idx++)
            {
                Cells[idx].Row += dyInt;
                Cells[idx].Column += dxInt;
            }
        }

        public Brick CloneAndMove(float dx, float dy) 
        {
            var newBrick = Clone();
            newBrick.Move(dx, dy);
            return newBrick;
        }

        public void Rotate() 
        {
            var midX = (int)Cells.Average(c => c.Column);
            var midY = (int)Cells.Average(c => c.Row);
            for (int idx = 0; idx < Cells.Length; idx++)
            {
                int saveRow = Cells[idx].Row;
                Cells[idx].Row = midY + (Cells[idx].Column - midX);
                Cells[idx].Column = midX + (midY - saveRow);
            }
        }

        public void FitToRectangle(Rectangle rectangle) 
        {
            var dxFromLeft = Cells.Select(c => rectangle.X - c.Column).Append(0).Max();
            var dxFromTop = Cells.Select(c => rectangle.Y - c.Row).Append(0).Max();

            var dxFromRight = Cells.Select(c => c.Column - rectangle.Right).Append(0).Max();
            var dxFromBottom = Cells.Select(c => c.Row - rectangle.Bottom).Append(0).Max();

            int dx = dxFromLeft == 0 || dxFromRight == 0 ? dxFromLeft - dxFromRight : 0;
            int dy = dxFromTop == 0 || dxFromBottom == 0 ? dxFromTop - dxFromBottom : 0;
            if (dx != 0 || dy != 0)
                MoveInt(dx, dy);
        }
    }
}
