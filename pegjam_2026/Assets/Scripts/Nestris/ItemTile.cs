using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private Color m_invalidActiveColor = Color.red;

    private Item m_parentItem;

    private ItemTileState m_state;

    private void Awake()
    {
        m_parentItem = GetComponentInParent<Item>();
    }

    public void SetState(ItemTileState newState)
    {
        switch (newState)
        {
            case ItemTileState.ActiveValid:
                m_spriteRenderer.color = m_parentItem.ItemCard.cardColor;
                break;
            case ItemTileState.ActiveInvalid:
                m_spriteRenderer.color = m_invalidActiveColor;
                break;
        }

        m_state = newState;
    }
}

public enum ItemTileState
{
    Inactive,
    ActiveValid,
    ActiveInvalid
}