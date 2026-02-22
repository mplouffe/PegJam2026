using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTextShaker : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_shakeTarget;

    [SerializeField]
    private Duration m_shakeDuration;

    [SerializeField]
    private float m_shakeIntensity;

    [SerializeField]
    private AudioClip m_sfx;

    private Vector3 m_originalPosition;

    private ShakerState m_state;

    private void Awake()
    {
        m_originalPosition = m_shakeTarget.localPosition;
    }

    private void Update()
    {
        switch(m_state)
        {
            case ShakerState.Shaking:
                if (m_shakeDuration.UpdateCheck())
                {
                    SetState(ShakerState.Steady);
                }
                else
                {
                    var shakeX = Random.Range(-m_shakeIntensity, m_shakeIntensity);
                    var shakeY = Random.Range(-m_shakeIntensity, m_shakeIntensity);
                    m_shakeTarget.localPosition = new Vector3(m_originalPosition.x + shakeX, m_originalPosition.y + shakeY, m_originalPosition.z);
                }
                break;
        }
    }

    private void SetState(ShakerState newState)
    {
        switch (newState)
        {
            case ShakerState.Shaking:
                gameObject.SetActive(true);
                m_shakeDuration.Reset();
                AudioManager.Instance.PlaySfx(m_sfx);
                break;
            case ShakerState.Steady:
                m_shakeTarget.localPosition = m_originalPosition;
                break;
        }
        m_state = newState;
    }

    public void ShakeText()
    {
        SetState(ShakerState.Shaking);
    }

    public void HideText()
    {
        m_state = ShakerState.Steady;
        gameObject.SetActive(false);
    }
}

public enum ShakerState
{
    Steady,
    Shaking
}
