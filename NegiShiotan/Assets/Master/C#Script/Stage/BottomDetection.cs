using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class BottomDetection : MonoBehaviour
{
    private GameObject m_Player = null;
    private SplashControl m_SplashControl = null;
    private ParticleEffectScript m_PartScript = null;
    // Start is called before the first frame update
    void Start()
    {
        m_Player= GameObject.FindGameObjectWithTag("Player");
        m_SplashControl = m_Player.GetComponentInChildren<SplashControl>();
        m_PartScript = m_Player.GetComponentInChildren<SplashDown>().m_PartScript;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            m_PartScript.StartEffect();
            m_SplashControl.isActive = true;//イカダのエフェクト再生
        }
    }
}
