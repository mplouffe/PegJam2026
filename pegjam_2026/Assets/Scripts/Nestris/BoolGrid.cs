using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolGrid
{
    public int size = 4;
    public bool[] cells; // flattened row-major: index = row * size + col

    public void Resize(int newSize)
    {
        bool[] newCells = new bool[newSize * newSize];
        if (cells != null && cells.Length == size * size)
        {
            int copySize = Mathf.Min(size, newSize);
            for (int r = 0; r < copySize; r++)
                for (int c = 0; c < copySize; c++)
                    newCells[r * newSize + c] = cells[r * size + c];
        }
        size = newSize;
        cells = newCells;
    }

    public bool Get(int row, int col) => cells[row * size + col];
    public void Set(int row, int col, bool val) => cells[row * size + col] = val;

    public void RotateClockwise()
    {
        bool[] newCells = new bool[size * size];
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                newCells[c * size + (size - 1 - r)] = cells[r * size + c];
        cells = newCells;
        NormalizeToTopLeft();
    }

    public void NormalizeToTopLeft()
    {
        // Find the bounding box of occupied cells
        int minRow = size, minCol = size;
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                if (Get(r, c))
                {
                    minRow = Mathf.Min(minRow, r);
                    minCol = Mathf.Min(minCol, c);
                }

        if (minRow == 0 && minCol == 0) return; // already normalized

        bool[] newCells = new bool[size * size];
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
            {
                int newR = r - minRow;
                int newC = c - minCol;
                if (newR >= 0 && newR < size && newC >= 0 && newC < size)
                    newCells[newR * size + newC] = cells[r * size + c];
            }
        cells = newCells;
    }
}
