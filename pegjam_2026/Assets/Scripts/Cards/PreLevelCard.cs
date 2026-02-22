using lvl_0;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreLevelCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_titleText;

    [SerializeField]
    private GoalField m_goalFieldPrefab;

    [SerializeField]
    private Transform m_goalsContainer;

    [SerializeField]
    private Button m_pickLevelButton;

    [SerializeField]
    private AudioClip m_buttonClickedClip;

    private Level m_level;
    private int m_index;

    private void Awake()
    {
        m_pickLevelButton.onClick.AddListener(OnPickLevelClicked);
    }

    private void OnDestroy()
    {
        m_pickLevelButton.onClick.RemoveListener(OnPickLevelClicked);
    }

    public void OnPickLevelClicked()
    {
        LevelManager.Instance.PickLevel(m_index);
        AudioManager.Instance.PlaySfx(m_buttonClickedClip);
    }

    public void SetLevel(Level level, int index)
    {
        m_index = index;
        m_level = level;
        m_titleText.text = level.LevelName;
        foreach(var goal in level.LevelGoals)
        {
            var goalPrefab = Instantiate(m_goalFieldPrefab, m_goalsContainer);
            goalPrefab.PopulateGoal(goal);
        }
    }

}
