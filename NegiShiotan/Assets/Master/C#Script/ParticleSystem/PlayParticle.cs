using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    ParticleSystem MyParticle;
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
    }
}
