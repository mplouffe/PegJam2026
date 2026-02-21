using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupsManager : MonoBehaviour
{
    public static PopupsManager Instance;

    [SerializeField]
    private CanvasGroup m_pausedCanvasGroup;

    [SerializeField]
    private CanvasGroup m_levelPassedCanvasGroup;

    [SerializeField]
    private CanvasGroup m_levelFailedCanvasGroup;

    [SerializeField]
    private CanvasGroup m_levelSelectionCanvsGroup;

    [SerializeField]
    private LevelCard m_levelCardPrefab;

    [SerializeField]
    private Transform m_levelCardContainer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

        m_pausedCanvasGroup.alpha = 0;
        m_pausedCanvasGroup.blocksRaycasts = false;
        m_levelPassedCanvasGroup.alpha = 0;
        m_levelPassedCanvasGroup.blocksRaycasts = false;
        m_levelFailedCanvasGroup.alpha = 0;
        m_levelFailedCanvasGroup.blocksRaycasts = false;
        m_levelSelectionCanvsGroup.alpha = 0;
        m_levelFailedCanvasGroup.blocksRaycasts = false;
    }

    public void LevelPassed(Level passedLevel)
    {
        m_levelPassedCanvasGroup.alpha = 1;
    }

    public void LevelFailed(Level failedLevel)
    {
        m_levelFailedCanvasGroup.alpha = 1;
    }

    public void ShowLevels(List<Level> levels)
    {
        m_levelSelectionCanvsGroup.alpha = 1;
        m_levelSelectionCanvsGroup.blocksRaycasts = true;
        int i = 0;
        foreach (var level in levels)
        {
            var levelCard = Instantiate(m_levelCardPrefab, m_levelCardContainer);
            levelCard.SetLevel(level, i);
            i++;
        }
    }

    public void ClearLevels()
    {
        m_levelSelectionCanvsGroup.alpha = 0;
        m_levelSelectionCanvsGroup.blocksRaycasts = false;
        foreach (Transform child in m_levelCardContainer)
            Destroy(child.gameObject);
    }
}
