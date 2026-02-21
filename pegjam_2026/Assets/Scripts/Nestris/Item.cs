using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Item : MonoBehaviour
{
    
    public BoolGrid ItemShape = new BoolGrid();

    public Card ItemCard;

    [SerializeField]
    private ItemTile m_tilePrefab;

    [SerializeField]
    private ItemVisual m_itemVisual;

    public bool Locked = true;

    private Dictionary<Vector2Int, ItemTile> m_tiles = new Dictionary<Vector2Int, ItemTile>();

    private void Awake()
    {
        BuildVisualization();
    }

    private void BuildVisualization()
    {
        ClearTiles();

        int numOfRows = -1;
        int numOfCols = -1;

        for (int i = 0; i < ItemShape.size; i++)
        {
            for (int j = 0; j < ItemShape.size; j++)
            {
                if (ItemShape.Get(i, j))
                {
                    var tile = Instantiate(m_tilePrefab, new Vector3(j + transform.position.x, transform.position.y - i, 0), Quaternion.identity, transform);
                    m_tiles.Add(new Vector2Int(j, i), tile);

                    if (i > numOfRows) numOfRows = i;
                    if (j > numOfCols) numOfCols = j;
                }
            }
        }
        numOfCols++;
        numOfRows++;

        var itemVisual = Instantiate(m_itemVisual, new Vector3(0, 0, 0), Quaternion.identity, transform);
        itemVisual.Init(ItemCard.cardSprite);
        float itemVisualPosXOffset = ((float)numOfCols / 2) - 0.5f;
        float itemVisualPosYOffset = ((float)numOfRows / 2) - 0.5f;

        Vector3 newPos = new Vector3(itemVisualPosXOffset, -itemVisualPosYOffset, 0);
        itemVisual.transform.SetPositionAndRotation(newPos, itemVisual.transform.rotation);
    }

    private void ClearTiles()
    {
        foreach (var tile in m_tiles.Values)
            Destroy(tile.gameObject);
        m_tiles.Clear();
    }

    public bool ValidateAgainstBoard(Vector2Int boardAnchor, Nest board)
    {
        bool overallValid = true;
        foreach(var kvp in m_tiles)
        {
            int c = kvp.Key.x;
            int r = kvp.Key.y;

            int boardCol = boardAnchor.x + c;
            int boardRow = boardAnchor.y - r;

            bool valid = board.IsCellValid(boardCol, boardRow);
            overallValid &= valid;
            kvp.Value.SetState(valid ? ItemTileState.ActiveValid : ItemTileState.ActiveInvalid);
        }
        return overallValid;
    }

    public void InvalidateShape()
    {
        foreach(var kvp in m_tiles)
        {
            kvp.Value.SetState(ItemTileState.ActiveInvalid);
        }
    }

    public bool PlacePiece(Vector2Int boardAnchor, Nest board)
    {
        foreach (var kvp in m_tiles)
        {
            int c = kvp.Key.x;
            int r = kvp.Key.y;

            int boardCol = boardAnchor.x + c;
            int boardRow = boardAnchor.y - r;

            bool valid = board != null && board.PlacePiece(boardCol, boardRow, ItemCard);
            if (!valid)
            {
                Debug.LogError("Error! Trying to place invalid piece");
                return valid;
            }
        }

        ClearTiles();
        if (ItemCard.cardType == ECardType.Person)
        {
            LevelManager.Instance.StartBuilding();
        }
        return true;
    }

    public void Rotate()
    {
        ItemShape.RotateClockwise();
        BuildVisualization();
    }
}
