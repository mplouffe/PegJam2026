using lvl_0;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostLevelCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_titleText;

    [SerializeField]
    private GoalField m_goalFieldPrefab;

    [SerializeField]
    private Transform m_goalsContainer;

    [SerializeField]
    private Button m_endButton;

    [SerializeField]
    private Duration m_goalPopulationWaitDuration;

    [SerializeField]
    private AudioClip m_buttonClickedClip;

    private PostLevelCardState m_state;

    private List<GoalField> m_goals = new List<GoalField>();

    private LevelTextShaker m_levelTextShaker;

    private Level m_level;
    private int m_goalsPopulated;

    private void Awake()
    {
        m_endButton.onClick.AddListener(OnEndClicked);
        m_endButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_endButton.onClick.RemoveListener(OnEndClicked);
    }

    private void Update()
    {
        switch (m_state)
        {
            case PostLevelCardState.Populating:
                if (m_goalPopulationWaitDuration.UpdateCheck())
                {
                    if (m_goalsPopulated >= m_goals.Count)
                    {
                        m_levelTextShaker.ShakeText();
                        SetState(PostLevelCardState.Waiting);
                        return;
                    }
                    m_goals[m_goalsPopulated].SetComplete(m_level.LevelGoals[m_goalsPopulated].GoalCompleted());
                    m_goalPopulationWaitDuration.Reset();
                    m_goalsPopulated++;
                }
                break;
        }
    }

    private void SetState(PostLevelCardState newState)
    {
        switch (newState)
        {
            case PostLevelCardState.Populating:
                m_endButton.gameObject.SetActive(false);
                m_goalPopulationWaitDuration.Reset(m_goalPopulationWaitDuration.TotalDuration());
                m_goalPopulationWaitDuration.Override(0.25f);
                m_goalsPopulated = 0;
                break;
            case PostLevelCardState.Waiting:
                m_endButton.gameObject.SetActive(true);
                break;
        }

        m_state = newState;
    }

    public void OnEndClicked()
    {
        LevelManager.Instance.EndLevel();
        AudioManager.Instance.PlaySfx(m_buttonClickedClip);
    }

    public void SetLevel(Level level, int index, LevelTextShaker levelTextShaker)
    {
        m_level = level;
        m_titleText.text = level.LevelName;
        m_levelTextShaker = levelTextShaker;
        foreach(var goal in level.LevelGoals)
        {
            m_goals.Add(Instantiate(m_goalFieldPrefab, m_goalsContainer));
            m_goals[m_goals.Count-1].PopulateGoal(goal);
        }
        SetState(PostLevelCardState.Populating); 
    }

}

public enum PostLevelCardState
{
    Spawned,
    Populating,
    Waiting,
}
