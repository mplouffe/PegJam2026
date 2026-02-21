using System.Collections;
using System.Collections.Generic;
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

    public void SetComplete(bool isComplete)
    {
        if (isComplete)
        {
            m_goalImage.sprite = m_goalIcons[1];
        }
        else
        {
            m_goalImage.sprite = m_goalIcons[0];
        }
    }

    public void SetGoal(Goal goal)
    {
        m_goalDescriptionField.text = goal.Description;
        SetComplete(goal.GoalCompleted());
    }
}
