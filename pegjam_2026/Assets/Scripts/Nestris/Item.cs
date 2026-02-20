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

    private void Awake()
    {
        m_camera = Camera.main;

        for (int i = 0; i < ItemShape.size; i++)
        {
            for (int j = 0; j < ItemShape.size; j++)
            {
                if (ItemShape.Get(i,j))
                {
                    Instantiate(m_tilePrefab, new Vector3(j + transform.position.x, transform.position.y - i, 0), Quaternion.identity, transform);
                }
            }
        }
    }

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
                transform.position = m_board.GetCellWorldPosition(cell.x, cell.y);
            }
            else
            {
                transform.position = worldPos;
            }
        }
    }
}
