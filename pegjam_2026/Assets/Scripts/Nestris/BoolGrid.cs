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
}
