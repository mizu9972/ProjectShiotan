using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//パーティクルシステム制御クラス
public class ParticleEffectScript : MonoBehaviour
{
    [SerializeField,Header("パーティクルシステム")]
    protected GameObject ParticleSystemObject = null;
    private ParticleSystem m_ParticleSystem = null;

    private void Start()
    {
        m_ParticleSystem = ParticleSystemObject.GetComponent<ParticleSystem>();
    }

    //再生
    [ContextMenu("Start")]
    public void StartEffect()
    {
        if (m_ParticleSystem)
        {
            m_ParticleSystem.Play();
        }
    }

    //一時停止
    [ContextMenu("Pause")]
    public void PauseEffect()
    {
        if (m_ParticleSystem.isPlaying)
        {
            m_ParticleSystem.Pause();
        }
    }

    //停止
    [ContextMenu("Stop")]
    public void StopEffect()
    {
        if (m_ParticleSystem.isPlaying)
        {
            m_ParticleSystem.Stop();
        }
    }
}
