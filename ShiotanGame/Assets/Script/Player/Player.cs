using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Player : MonoBehaviour
{

    [SerializeField, Header("水面")]
    GameObject SeaPlane = null;
    [SerializeField, Header("波形テクスチャ")]
    Texture WaveTex = null;

    [SerializeField, Header("波発生間隔")]
    float WaveInterval = 1.0f;

    private WavePlane m_WavePlaneScript = null;
    // Start is called before the first frame update
    void Start()
    {
        SeaPlane = GameObject.FindGameObjectWithTag("SeaPlane");
        m_WavePlaneScript = SeaPlane.GetComponent<WavePlane>();
        Wave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Wave()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(WaveInterval))
            .Subscribe(_ => m_WavePlaneScript.AwakeWave(this.transform, 0.1f, WaveTex));
    }
}
