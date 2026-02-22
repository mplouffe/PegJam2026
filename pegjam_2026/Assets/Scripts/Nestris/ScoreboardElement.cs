using lvl_0;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_elementLabel;

    [SerializeField]
    private TextMeshProUGUI m_elementValue;

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private Transform m_popTransform;

    [SerializeField]
    private Duration m_popDuration;

    [SerializeField]
    private float m_popScale;

    [SerializeField]
    private bool m_persistent;

    private ScoreboardElementState m_state;

    private void Awake()
    {
        SetState(ScoreboardElementState.None);    
    }

    public void Pop(float score, string name = "Score", Color color = default)
    {
        m_elementValue.text = score.ToString();
        m_elementLabel.text = name;
        m_elementLabel.color = color;
        m_elementValue.color = color;
        SetState(ScoreboardElementState.Popping);
    }

    private void SetState(ScoreboardElementState newState)
    {
        switch (newState)
        {
            case ScoreboardElementState.Popping:
                m_popTransform.localScale = new Vector3(m_popScale, m_popScale, 1);
                m_popDuration.Reset();
                m_canvasGroup.alpha = 1;
                break;
            case ScoreboardElementState.None:
                m_popTransform.localScale = Vector3.one;
                m_canvasGroup.alpha = m_persistent ? 1 : 0;
                break;
        }
        m_state = newState;
    }

    private void Update()
    {
        switch (m_state)
        {
            case ScoreboardElementState.Popping:
                if (m_popDuration.UpdateCheck())
                {

                    SetState(ScoreboardElementState.None);
                }
                else
                {
                    var currentScale = Mathf.Lerp(m_popScale, 1, m_popDuration.CurvedDelta());
                    m_popTransform.localScale = new Vector3(currentScale, currentScale, 1);
                }
                break;
        }
    }
}

public enum ScoreboardElementState
{
    None,
    Popping
}