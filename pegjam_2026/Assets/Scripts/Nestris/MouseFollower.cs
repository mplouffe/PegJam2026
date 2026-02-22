using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : SingletonBase<MouseFollower>
{
    [SerializeField]
    private Nest m_board;

    [SerializeField]
    private AudioClip m_dropCardSFX;

    [SerializeField]
    private AudioClip m_lockPostionSFX;

    [SerializeField]
    private AudioClip m_rotateSFX;

    [SerializeField]
    private AudioClip m_placePieceSFX;

    [SerializeField]
    private AudioClip m_placeDeniedSFX;

    private Item m_activeItem = null;
    private CardManager m_activeItemCard = null;
    private Camera m_camera;
    private Vector2Int m_previousCell = new Vector2Int(-1, -1);

    protected override void Awake()
    {
        base.Awake();
        m_camera = Camera.main;
    }

    public void SelectPiece(Item newItem, CardManager card)
    {
        m_activeItem = newItem;
        m_activeItemCard = card;
    }

    private void Update()
    {
        if (m_activeItem != null)
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
                    AudioManager.Instance.PlaySfx(m_lockPostionSFX);
                    m_previousCell = cell;
                    m_activeItem.ValidateAgainstBoard(cell, m_board);
                    m_activeItem.transform.position = m_board.GetCellWorldPosition(cell.x, cell.y);
                    m_activeItem.Locked = true;
                }

                if (Input.GetMouseButton(0))
                {
                    if (m_activeItem.ValidateAgainstBoard(cell, m_board))
                    {
                        if (m_activeItem.PlacePiece(cell, m_board))
                        {
                            AudioManager.Instance.PlaySfx(m_placePieceSFX);
                            m_activeItemCard.SetState(CardState.Used);
                            m_activeItem = null;
                            return;
                        }
                    }

                    AudioManager.Instance.PlaySfx(m_placeDeniedSFX);
                }
            }
            else
            {
                if (m_activeItem.Locked)
                {
                    m_activeItem.Locked = false;
                    m_activeItem.InvalidateShape();
                    m_previousCell = new Vector2Int(-1, -1);
                }
                m_activeItem.transform.position = worldPos;

                if (Input.GetMouseButton(0))
                {
                    AudioManager.Instance.PlaySfx(m_dropCardSFX);
                    Destroy(m_activeItem.gameObject);
                    m_activeItemCard.SetState(CardState.Dealt);
                    m_activeItemCard = null;
                    m_activeItem = null;
                }
            }

            if (Input.GetMouseButtonDown(1) && m_activeItem != null)
            {
                AudioManager.Instance.PlaySfx(m_rotateSFX);
                m_activeItem.Rotate();
                m_previousCell = new Vector2Int(-1, -1);
            }
        }
    }

}
