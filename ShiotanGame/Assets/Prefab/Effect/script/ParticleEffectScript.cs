using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//パーティクルシステム制御クラス
public class ParticleEffectScript : MonoBehaviour
{
    [SerializeField,Header("パーティクルシステム")]
    protected List<GameObject> ParticleSystemObjects = new List<GameObject>();
    private List<ParticleSystem> m_ParticleSystem = new List<ParticleSystem>();

    private void Start()
    {
        foreach(var IParticle in ParticleSystemObjects)
        {
            m_ParticleSystem.Add(IParticle.GetComponent<ParticleSystem>());
        }
    }

    //再生
    [ContextMenu("Start")]
    public void StartEffect()
    {
        foreach(var IParticle in m_ParticleSystem)
        {
            if(IParticle != null)
            {
                IParticle.Play();
            }
        }
    }

    //一時停止
    [ContextMenu("Pause")]
    public void PauseEffect()
    {
        foreach(var IParticle in m_ParticleSystem)
        {
            if (IParticle.isPlaying)
            {
                IParticle.Pause();
            }
        }
    }

    //停止
    [ContextMenu("Stop")]
    public void StopEffect()
    {
        foreach (var IParticle in m_ParticleSystem)
        {
            if (IParticle.isPlaying)
            {
                IParticle.Stop();
            }
        }
    }
}
