using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    ParticleSystem MyParticle;

    private bool m_isActive = false;

    public bool isActive
    {
        get { return m_isActive; }
    }
    // Start is called before the first frame update
    void Start()
    {
        MyParticle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()//パーティクル再生
    {
        MyParticle.Play();
        m_isActive = true;
    }
    public void OnParticleSystemStopped()
    {
        m_isActive = false;
    }
}
