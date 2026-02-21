using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonClicked : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        LevelAttendant.Instance.LoadGameState(GameState.GameStart);
    }
}
