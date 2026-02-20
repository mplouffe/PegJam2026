using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    public BoolGrid ItemShape = new BoolGrid();

    [SerializeField]
    private ItemTile m_tilePrefab;

    [SerializeField]
    private Nest m_board;

    private Camera m_camera;
    private bool m_active = true;

    private Dictionary<Vector2Int, ItemTile> m_tiles = new Dictionary<Vector2Int, ItemTile>();

    private void Awake()
    {
        m_camera = Camera.main;

        BuildVisualization();
    }

    private void BuildVisualization()
    {
        foreach(var tile in m_tiles.Values)
            Destroy(tile.gameObject);
        m_tiles.Clear();

        for (int i = 0; i < ItemShape.size; i++)
        {
            for(int j = 0;j < ItemShape.size; j++)
            {
                if (ItemShape.Get(i,j))
                {
                    var tile = Instantiate(m_tilePrefab, new Vector3(j + transform.position.x, transform.position.y - i, 0), Quaternion.identity, transform);
                    m_tiles.Add(new Vector2Int(j, i), tile);
                }
            }
        }
    }

    private void ValidateAgainstBoard(Vector2Int boardAnchor)
    {
        // Debug.Log(boardAnchor.ToString());
        foreach(var kvp in m_tiles)
        {
            int c = kvp.Key.x;
            int r = kvp.Key.y;

            int boardCol = boardAnchor.x + c;
            int boardRow = boardAnchor.y - r;


            bool valid = m_board != null && m_board.IsCellValid(boardCol, boardRow);
            Debug.Log(boardCol + " " + boardRow + ": " + valid);
            kvp.Value.SetState(valid ? ItemTileState.ActiveValid : ItemTileState.ActiveInvalid);
        }
    }

    private void InvalidateShape()
    {
        foreach(var kvp in m_tiles)
        {
            kvp.Value.SetState(ItemTileState.ActiveInvalid);
        }
    }

    private Vector2Int m_previousCell = new Vector2Int(-1, -1);
    private bool m_locked = true;

    private void Update()
    {
        if (m_active)
        {
            Vector3 mouseScreen = Input.mousePosition;

            if (mouseScreen.x < 0 || mouseScreen.x > Screen.width ||
                mouseScreen.y < 0 || mouseScreen.y > Screen.height)
            {
                return;
            }

            Vector3 worldPos = m_camera.ScreenToWorldPoint(mouseScreen);
            worldPos.z = transform.position.z;

            if (m_board != null && m_board.GetBounds().Contains(worldPos))
            {
                Vector2Int cell = m_board.WorldToCell(worldPos);
                if (m_previousCell != cell)
                {
                    m_previousCell = cell;
                    ValidateAgainstBoard(cell);
                    transform.position = m_board.GetCellWorldPosition(cell.x, cell.y);
                    m_locked = true;
                }
            }
            else
            {
                if (m_locked)
                {
                    m_locked = false;
                    InvalidateShape();
                }
                transform.position = worldPos;
            }
        }
    }
}
