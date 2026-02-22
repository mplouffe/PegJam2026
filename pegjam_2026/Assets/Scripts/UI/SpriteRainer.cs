using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRainer : MonoBehaviour
{
    [SerializeField]
    private SpriteDrop m_spriteDropPrefab;

    [SerializeField]
    private float m_durationMin;

    [SerializeField]
    private float m_durationMax;

    [SerializeField]
    private BoxCollider2D m_spawnBounds;

    [SerializeField]
    private Duration m_dropInterval;

    private int m_previousInterval;


    private void Awake()
    {
        m_dropInterval = new Duration(Random.Range(m_durationMin, m_durationMax));
    }

    private void Update()
    {
        if (m_dropInterval.UpdateCheck())
        {
            var currentInterval = -1;
            var randomThird = Random.Range(1, 3);
            switch (m_previousInterval)
            {
                case 0:
                    currentInterval = randomThird;
                    break;
                case 1:
                    currentInterval = randomThird == 1 ? 0 : 2;
                    break;
                case 2:
                    currentInterval = randomThird - 1;
                    break;
            }
            var totalWidth = m_spawnBounds.bounds.max.x - m_spawnBounds.bounds.min.x;
            var third = totalWidth / 3;
            var randomX = Random.Range(m_spawnBounds.bounds.min.x + (third * currentInterval), m_spawnBounds.bounds.max.x - (third * (2 - currentInterval)));
            Vector3 spawnPosition = new Vector3(randomX, m_spawnBounds.bounds.center.y, 0);
            var spriteDrop = Instantiate(m_spriteDropPrefab, spawnPosition, Quaternion.identity);
            spriteDrop.PrepareDrop();
            m_dropInterval.Reset(Random.Range(m_durationMin, m_durationMax));
            m_previousInterval = currentInterval;
        }
    }
}
