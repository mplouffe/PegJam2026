using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonClicked : MonoBehaviour
{
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
