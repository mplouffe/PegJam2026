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
        m_leaveLevelButton.onClick.AddListener(OnLeaveLevelClicked);
        m_cancelButton.onClick.AddListener(OnCancelClicked);
    }

    private void OnDestroy()
    {
        m_leaveLevelButton.onClick.RemoveListener(OnLeaveLevelClicked);
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
