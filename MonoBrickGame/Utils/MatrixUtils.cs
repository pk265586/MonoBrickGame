using System;
using System.Collections.Generic;

namespace MonoTetris.Utils
{
    public static class MatrixUtils
    {
        public static IEnumerable<T> Row<T>(this T[,] matrix, int row) 
        {
            for (int column = 0; column < matrix.GetLength(1); column++)
                yield return matrix[row, column];
        }

        public static void CopyRow<T>(this T[,] matrix, int fromRow, int toRow) 
        {
            for (int column = 0; column < matrix.GetLength(1); column++)
                matrix[toRow, column] = matrix[fromRow, column];
        }

        public static void FillRow<T>(this T[,] matrix, int row, T value) 
        {
            for (int column = 0; column < matrix.GetLength(1); column++)
                matrix[row, column] = value;
        }
    }
}
