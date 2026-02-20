using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class Nest : MonoBehaviour
{
    [SerializeField]
    private NestTile m_nestTilePrefab;

    [SerializeField]
    private int m_nestHeight = 3;

    [SerializeField]
    private int m_nestWidth = 8;

    private List<List<NestTile>> m_nest = new List<List<NestTile>>();

    private void Awake()
    {
        for(int i = 0; i < m_nestHeight; i++)
        {
            m_nest.Add(new List<NestTile>());
            for (int j = 0; j < m_nestWidth; j++)
            {
                Vector3 spawnPos = new Vector3(j + transform.position.x, i + transform.position.y, 0);
                m_nest[i].Add(Instantiate(m_nestTilePrefab, spawnPos, Quaternion.identity, transform));
            }
        }
    }

    public Vector3 GetCellWorldPosition(int col, int row)
    {
        Vector3 origin = transform.position;
        return origin + new Vector3(col, row, 0f);
    }

    public Bounds GetBounds()
    {
        Vector3 center = transform.position + new Vector3(
            (m_nestWidth - 1) / 2f,
            (m_nestHeight - 1) / 2f, 0f);
        return new Bounds(center, new Vector3(m_nestWidth, m_nestHeight, 1f));
    }

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        Vector3 local = worldPos - transform.position;
        int col = Mathf.RoundToInt(local.x);
        int row = Mathf.RoundToInt(local.y);
        return new Vector2Int(col, row);
    }

    public bool IsCellValid(int col, int row)
    {
        // Out of bounds
        if (col < 0 || col >= m_nestWidth || row < 0 || row >= m_nestHeight)
            return false;

        return !m_nest[row][col].IsOccupied;
    }
}
