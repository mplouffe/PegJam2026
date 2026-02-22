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
    private CanvasGroup m_levelInfoCanvsGroup;

    [SerializeField]
    private PreLevelCard m_levelCardPrefab;

    [SerializeField]
    private PostLevelCard m_postLevelCardPrefab;

    [SerializeField]
    private LevelTextShaker m_failedLevelText;

    [SerializeField]
    private LevelTextShaker m_passedLevelText;

    [SerializeField]
    private Transform m_levelCardContainer;

    private LevelTextShaker m_activeTextReference;
    
    [SerializeField]
    private QuitPopup m_quitPopupPrefab;

    private QuitPopup m_quitPopup = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

        m_pausedCanvasGroup.alpha = 0;
        m_pausedCanvasGroup.blocksRaycasts = false;
        m_levelInfoCanvsGroup.alpha = 0;
        m_levelInfoCanvsGroup.blocksRaycasts = false;
    }

    public void LevelPassed(Level passedLevel)
    {
        m_levelInfoCanvsGroup.alpha = 1;
        m_levelInfoCanvsGroup.blocksRaycasts = true;
        var levelCard = Instantiate(m_postLevelCardPrefab, m_levelCardContainer);
        m_activeTextReference = Instantiate(m_passedLevelText, m_levelCardContainer.parent);
        m_activeTextReference.HideText();
        levelCard.SetLevel(passedLevel, -1, m_activeTextReference);
    }

    public void LevelFailed(Level failedLevel)
    {
        m_levelInfoCanvsGroup.alpha = 1;
        m_levelInfoCanvsGroup.blocksRaycasts = true;
        var levelCard = Instantiate(m_postLevelCardPrefab, m_levelCardContainer);
        m_activeTextReference = Instantiate(m_failedLevelText, m_levelCardContainer.parent);
        m_activeTextReference.HideText();
        levelCard.SetLevel(failedLevel, -1, m_activeTextReference);
    }

    public void ShowLevels(List<Level> levels)
    {
        m_levelInfoCanvsGroup.alpha = 1;
        m_levelInfoCanvsGroup.blocksRaycasts = true;
        foreach (var level in levels)
        {
            var levelCard = Instantiate(m_levelCardPrefab, m_levelCardContainer);
            levelCard.SetLevel(level);
        }
    }

    public void ClearLevelInfo()
    {
        m_levelInfoCanvsGroup.alpha = 0;
        m_levelInfoCanvsGroup.blocksRaycasts = false;
        foreach (Transform child in m_levelCardContainer)
            Destroy(child.gameObject);
        if (m_activeTextReference != null)
            Destroy(m_activeTextReference.gameObject);
        m_activeTextReference = null;
    }

    public void Quit()
    {
        m_levelInfoCanvsGroup.alpha = 1;
        m_levelInfoCanvsGroup.blocksRaycasts = true;
        m_quitPopup = Instantiate(m_quitPopupPrefab, m_levelCardContainer.parent);
    }

    public void HideQuitPopup()
    {
        m_levelInfoCanvsGroup.alpha = 0;
        m_levelInfoCanvsGroup.blocksRaycasts = false;
        if(m_quitPopup != null)
        {
            Destroy(m_quitPopup.gameObject);
        }
    }
}
