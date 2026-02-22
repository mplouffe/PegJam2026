using lvl_0;
using UnityEngine;
using UnityEngine.UI;

public class QuitPopup : MonoBehaviour
{
    [SerializeField]
    private Button m_leaveLevelButton;

    [SerializeField]
    private Button m_cancelButton;

    [SerializeField]
    private AudioClip m_buttonClickedClip;

    private void Awake()
    {
        if(m_leaveLevelButton != null)
            m_leaveLevelButton.onClick.AddListener(OnLeaveLevelClicked);
        if(m_cancelButton != null)
            m_cancelButton.onClick.AddListener(OnCancelClicked);
    }

    private void OnDestroy()
    {
        if (m_leaveLevelButton != null)
            m_leaveLevelButton.onClick.RemoveListener(OnLeaveLevelClicked);
        if (m_cancelButton != null)
            m_cancelButton.onClick.RemoveListener(OnCancelClicked);
    }

    public void OnLeaveLevelClicked()
    {
        LevelAttendant.Instance.LoadGameState(GameState.Menu);
        AudioManager.Instance.PlaySfx(m_buttonClickedClip);
    }

    public void OnCancelClicked()
    {
        AudioManager.Instance.PlaySfx(m_buttonClickedClip);
        PopupsManager.Instance.HideQuitPopup();
    }
}
