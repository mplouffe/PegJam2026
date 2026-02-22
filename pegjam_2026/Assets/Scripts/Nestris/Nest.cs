using lvl_0;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Nest : SingletonBase<Nest>
{
    [SerializeField]
    private NestTile m_nestTilePrefab;

    [SerializeField]
    private int m_nestHeight = 3;

    [SerializeField]
    private int m_nestWidth = 8;

    [SerializeField]
    private float m_scoreMultiplerIncrement = 0.15f;

    [SerializeField]
    private Duration m_scoringWaitDuration;

    [SerializeField]
    private float m_scoreDecrement;

    [SerializeField]
    private float m_scoreStartingWait;

    [SerializeField]
    private float m_scoreMinWait;

    [SerializeField]
    private Duration m_postScoreWaitDuration;

    [SerializeField]
    private Button m_scoreNestButton;

    [SerializeField]
    private TextMeshProUGUI m_scoreField;

    public float Score { get { return m_finalScore; } }

    private NestState m_nestState;

    private Vector2Int m_scoringTracker = Vector2Int.zero;
    private float m_finalScore = 0f;

    private List<List<NestTile>> m_nest = new List<List<NestTile>>();

    private Dictionary<ECardType, float> m_scoreTypeMultipliers = new Dictionary<ECardType, float>();

    protected override void OnDestroy()
    {
        m_scoreNestButton.onClick.RemoveListener(OnScoreNestClicked);
        base.OnDestroy();
    }

    public void OnScoreNestClicked()
    {
        if (m_nestState == NestState.Placing)
        {
            SetState(NestState.Scoring);
        }
    }

    public int GetTileCount(ECardType type)
    {
        int count = 0;
        foreach (List<NestTile> tileList in m_nest)
        {
            foreach (NestTile tile in tileList)
            {
                if (tile.GetTileType() == type)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void SetState(NestState newState)
    {
        if (m_nestState == newState) return;
        switch (newState)
        {
            case NestState.Scoring:
                m_scoringWaitDuration.Reset(m_scoreStartingWait);
                m_scoringTracker = new Vector2Int(m_nestHeight-1, m_nestWidth-1);
                CardDeckManager.Instance.StopDealing();
                m_scoreNestButton.gameObject.SetActive(false);
                break;
            case NestState.Placing:
                m_scoreNestButton.gameObject.SetActive(true);
                break;
            case NestState.Waiting:
                m_postScoreWaitDuration.Reset();
                break;
            case NestState.Ending:
                LevelManager.Instance.EvaluateLevel();
                break;
        }
        m_nestState = newState;
    }

    private void Update()
    {
        switch (m_nestState)
        {
            case NestState.Scoring:
                if (m_scoringWaitDuration.UpdateCheck())
                {
                    NestTile currentNestTile = m_nest[m_scoringTracker.x][m_scoringTracker.y];
                    var score = currentNestTile.ScoreTile();
                    var type = currentNestTile.GetTileType();
                    m_finalScore += score * m_scoreTypeMultipliers[type];
                    ScoreboardManager.Instance.PostScore(score, currentNestTile.GetTileName(), currentNestTile.GetTileColor(), m_scoreTypeMultipliers[type], currentNestTile.GetTileType().ToString(), currentNestTile.GetTileColor(), (int)m_finalScore);
                    if (type != ECardType.Misc) m_scoreTypeMultipliers[type] += m_scoreMultiplerIncrement;

                    m_scoringTracker.y -= 1;
                    if (m_scoringTracker.y < 0)
                    {
                        m_scoringTracker.y = m_nestWidth-1;
                        m_scoringTracker.x -= 1;
                        if (m_scoringTracker.x < 0)
                        {
                            SetState(NestState.Waiting);
                        }
                    }
                    m_scoringWaitDuration.Reset(Mathf.Max(m_scoreMinWait, m_scoringWaitDuration.TotalDuration() - m_scoreDecrement));
                }
                break;
            case NestState.Waiting:
                if (m_postScoreWaitDuration.UpdateCheck())
                {
                    SetState(NestState.Ending);
                }
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < m_nestHeight; i++)
        {
            m_nest.Add(new List<NestTile>());
            for (int j = 0; j < m_nestWidth; j++)
            {
                Vector3 spawnPos = new Vector3(j + transform.position.x, i + transform.position.y, 0);
                m_nest[i].Add(Instantiate(m_nestTilePrefab, spawnPos, Quaternion.identity, transform));
            }
        }

        foreach(ECardType cardType in Enum.GetValues(typeof(ECardType)))
        {
            m_scoreTypeMultipliers.Add(cardType, 1.0f);
        }

        m_scoreNestButton.onClick.AddListener(OnScoreNestClicked);
        m_scoreNestButton.gameObject.SetActive(false);
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

    public bool PlacePiece(int col, int row, Card card, ItemVisual visualization)
    {
        if (col < 0 || col >= m_nestWidth || row < 0 || row >= m_nestHeight)
            return false;

        if (m_nest[row][col].IsOccupied) return false;

        m_nest[row][col].Occupy(card, visualization);

        if (m_nestState == NestState.Empty) SetState(NestState.Placing);
        return true;
    }
}

public enum NestState
{
    Empty,
    Placing,
    Scoring,
    Waiting,
    Ending
}
