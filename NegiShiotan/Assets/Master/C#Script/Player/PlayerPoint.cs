using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{

    private Transform m_myTrans = null;
    private Transform m_playerTrans = null;
    // Start is called before the first frame update
    void Start()
    {
        m_myTrans = this.gameObject.transform;
        m_playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        if (m_playerTrans != null)
        {
            m_myTrans.position = m_playerTrans.position;
        }
    }
}
