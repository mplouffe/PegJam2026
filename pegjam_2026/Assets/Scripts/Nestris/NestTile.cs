using lvl_0;
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

    [SerializeField]
    private Duration m_scoringDuration;

    [SerializeField]
    private float m_popScale;

    private Card m_card = null;

    public NestTileState State { get; private set; }

    private void Update()
    {
        switch (State)
        {
            case NestTileState.Scoring:
                if (m_scoringDuration.UpdateCheck())
                {
                    transform.localScale = Vector3.one;
                    SetState(NestTileState.Occupied);
                }
                else
                {
                    var currentScale = Mathf.Lerp(m_popScale, 1, m_scoringDuration.CurvedDelta());
                    transform.localScale = new Vector3(currentScale, 1, currentScale);
                }
                break;
        }
    }

    public void Occupy(Card card)
    {
        m_card = card;
        SetState(NestTileState.Occupied);
    }

    public int ScoreTile()
    {
        SetState(NestTileState.Scoring);
        return m_card.cardValue;
    }

    public ECardType GetTileType()
    {
        return m_card?.cardType ?? ECardType.Misc;
    }

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
    Scoring,
}
