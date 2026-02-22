using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    [SerializeField]
    private Duration m_fuze;

    // Update is called once per frame
    void Update()
    {
        if (m_fuze.UpdateCheck())
        {
            Destroy(gameObject);
        }
    }
}
