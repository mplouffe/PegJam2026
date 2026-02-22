using lvl_0;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreboardManager : SingletonBase<ScoreboardManager>
{
    [SerializeField]
    private ScoreboardElement m_tileScoreElement;

    [SerializeField]
    private ScoreboardElement m_multiplierElement;

    [SerializeField]
    private ScoreboardElement m_xElement;

    [SerializeField]
    private ScoreboardElement m_totalScoreElement;

    public void PostScore(int tileScore, string tileName, Color tileColor, float multiplier, string multiplerType, Color multiplierColor, int totalScore)
    {
        m_tileScoreElement.Pop(tileScore, tileName, tileColor);
        m_multiplierElement.Pop(multiplier, multiplerType, multiplierColor);
        m_totalScoreElement.Pop(totalScore, "Score", Color.white);
        if (tileScore > 0) m_xElement.Pop(0, "X", Color.white);
    }
    
}
