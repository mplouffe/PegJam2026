using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestTile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_sprite;

    [SerializeField]
    private Color m_emptyColor = Color.white;

    [SerializeField]
    private Color m_occupiedColor = Color.blue;

    public NestTileState State { get; private set; }

    public void SetState(NestTileState newState)
    {
        switch (newState)
        {
            case NestTileState.Empty:
                m_sprite.color = m_emptyColor;
                break;
            case NestTileState.Occupied:
                m_sprite.color = m_occupiedColor;
                break;
        }
        State = newState;
    }

    public bool IsOccupied { get { return State == NestTileState.Occupied; } } 
}

public enum NestTileState
{
    Empty,
    Occupied,
}
