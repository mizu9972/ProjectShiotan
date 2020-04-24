using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
//波を発生させる処理を呼ぶクラス
public class WaveAct : MonoBehaviour
{

    [SerializeField, Header("水面")]
    GameObject SeaPlane = null;
    [SerializeField, Header("波形テクスチャ")]
    Texture WaveTex = null;

    [SerializeField, Header("波の大きさ")]
    private float WaveSize = 0.1f;

    [SerializeField, Header("波発生間隔"),Tooltip("表示用")]
    private float WaveInterval = 1.0f;

    [SerializeField, Header("停止時の波発生間隔")]
    private float WaveInterval_Stop = 1.0f;

    [SerializeField, Header("移動時の波発生間隔")]
    private float WaveInterval_Move = 0.3f;

    [SerializeField, Header("移動を検知する速度")]
    private float MoveHorizon = 3.0f;

    [SerializeField, Header("波紋の発生位置をランダムにさせる")]
    private bool RandomFlag = true;

    private WavePlane m_WavePlaneScript = null;
    private Rigidbody m_myRigidbody = null;
    // Start is called before the first frame update
    void Start()
    {

        m_myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        //波処理
        SeaPlane = GameObject.FindGameObjectWithTag("SeaPlane");
        if (SeaPlane != null)
        {
            m_WavePlaneScript = SeaPlane.GetComponent<WavePlane>();
            //StartCoroutine("Wave");
            AwakeMultiWave();
        }

        this.UpdateAsObservable()
            .Select(_ => m_myRigidbody.velocity.magnitude)
            //.DistinctUntilChanged()
            .Subscribe(x =>
            {

                if (x < MoveHorizon)
                {
                    WaveInterval = WaveInterval_Stop;
                }
                else
                {
                    WaveInterval = WaveInterval_Move;
                };

            });
    }

    IEnumerator Wave()
    {
        Debug.Log(this.gameObject.name);
        //一定間隔毎に波を発生させる
        float SetFlag;
        if (RandomFlag)
        {
            SetFlag = 1.0f;
        }
        else
        {
            SetFlag = 0.0f;
        }
        m_WavePlaneScript.AwakeWave(this.transform, WaveSize, WaveTex, SetFlag);

        yield return new WaitForSeconds(WaveInterval);

        StartCoroutine("Wave");

        yield break;
    }

    public void AwakeMultiWave()
    {
        m_WavePlaneScript.AwakeWave(this.transform, WaveSize, WaveTex, 1.0f, 10.0f, 0.1f);
    }
}
