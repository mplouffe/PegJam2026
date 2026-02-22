using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDrop : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_sprites;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private Rigidbody2D m_rb;

    [SerializeField]
    private float m_maxSpin;

    public void PrepareDrop()
    {
        m_spriteRenderer.sprite = m_sprites[Random.Range(0, m_sprites.Count)];
        m_rb.angularVelocity = Random.Range(-m_maxSpin, m_maxSpin);
    }
}
