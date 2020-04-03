using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

//波を発生させる処理を呼ぶクラス
public class WaveAct : MonoBehaviour
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
        //波処理
        SeaPlane = GameObject.FindGameObjectWithTag("SeaPlane");
        if (SeaPlane != null)
        {
            m_WavePlaneScript = SeaPlane.GetComponent<WavePlane>();
            Wave();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Wave()
    {
        //TODO 移動時と停止時で間隔を変化させたい

        //一定間隔毎に波を発生させる
        Observable.Interval(System.TimeSpan.FromSeconds(WaveInterval))
            .Subscribe(_ => m_WavePlaneScript.AwakeWave(this.transform, 0.1f, WaveTex)).AddTo(this.gameObject);
    }
}
