using lvl_0;
using UnityEngine;

public class NestTile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_sprite;

    [SerializeField]
    private Color m_emptyColor = Color.white;

    [SerializeField]
    private Duration m_scoringDuration;

    [SerializeField]
    private float m_popScale;

    private Card m_card = null;
    private ItemVisual m_visualization = null;

    public NestTileState State { get; private set; }

    private void Update()
    {
        switch (State)
        {
            case NestTileState.Scoring:
                if (m_scoringDuration.UpdateCheck())
                {
                    transform.localScale = Vector3.one;
                    SetState(NestTileState.Empty);
                }
                else
                {
                    var currentScale = Mathf.Lerp(m_popScale, 1, m_scoringDuration.CurvedDelta());
                    transform.localScale = new Vector3(currentScale, currentScale,1);
                }
                break;
        }
    }

    public void Occupy(Card card, ItemVisual visualization)
    {
        m_card = card;
        m_visualization = visualization;
        SetState(NestTileState.Occupied);
    }

    public int ScoreTile()
    {
        SetState(NestTileState.Scoring);
        return m_card?.cardValue ?? 0;
    }

    public ECardType GetTileType()
    {
        return m_card?.cardType ?? ECardType.Misc;
    }

    public string GetTileName()
    {
        return m_card?.cardDesciption ?? string.Empty;
    }

    public Color GetTileColor()
    {
        return m_card?.cardColor ?? Color.clear;
    }

    public void SetState(NestTileState newState)
    {
        switch (newState)
        {
            case NestTileState.Empty:
                m_sprite.color = m_emptyColor;
                break;
            case NestTileState.Occupied:
                m_sprite.color = m_card.cardColor;
                break;
            case NestTileState.Scoring:
                m_visualization?.Pop();
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
