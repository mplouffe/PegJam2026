using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonClicked : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_startButtonSFX;

    public void OnStartButtonClicked()
    {
        AudioManager.Instance.PlaySfx(m_startButtonSFX);
        LevelAttendant.Instance.LoadGameState(GameState.GameStart);
    }
}
