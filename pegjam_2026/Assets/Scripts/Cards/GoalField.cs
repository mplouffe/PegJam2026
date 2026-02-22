using lvl_0;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalField : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_goalDescriptionField;

    [SerializeField]
    private Image m_goalImage;

    [SerializeField]
    private List<Sprite> m_goalIcons;

    [SerializeField]
    private float m_scaleSize;

    [SerializeField]
    private Duration m_scaleDuration;

    [SerializeField]
    private AudioClip m_goalCompleteSFX;

    [SerializeField]
    private AudioClip m_goalFailedSFX;

    private GoalFieldState m_state;

    private void Update()
    {
        switch (m_state)
        {
            case GoalFieldState.Animating:
                if (m_scaleDuration.UpdateCheck())
                {
                    SetState(GoalFieldState.Still);
                }
                else
                {
                    var currentScale = Mathf.Lerp(m_scaleSize, 1, m_scaleDuration.CurvedDelta());
                    m_goalImage.transform.localScale = new Vector3(currentScale, currentScale, 1);
                }
                break;
        }
    }

    private void SetState(GoalFieldState newState)
    {
        switch (newState)
        {
            case GoalFieldState.Animating:
                m_scaleDuration.Reset();
                m_goalImage.transform.localScale = new Vector3(m_scaleSize, m_scaleSize, 1);
                break;
            case GoalFieldState.Still:
                m_goalImage.transform.localScale = Vector3.one;
                break;
        }

        m_state = newState;
    }

    public void SetComplete(bool isComplete)
    {
        if (isComplete)
        {
            m_goalImage.sprite = m_goalIcons[1];
        }
        else
        {
            m_goalImage.sprite = m_goalIcons[2];
        }
        AudioManager.Instance.PlaySfx(isComplete ? m_goalCompleteSFX : m_goalFailedSFX);
        SetState(GoalFieldState.Animating);
    }

    public void PopulateGoal(Goal goal)
    {
        m_goalDescriptionField.text = goal.Description;
        m_goalImage.sprite = m_goalIcons[0];
    }
}

public enum GoalFieldState
{
    Spawned,
    Animating,
    Still
}
