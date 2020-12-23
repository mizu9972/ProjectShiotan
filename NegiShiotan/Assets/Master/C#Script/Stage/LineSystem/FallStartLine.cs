using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStartLine : MonoBehaviour
{
    [SerializeField, Header("落下地点のPlane")]
    private GameObject FallEndPlane = null;

    private SplashControl m_SplashControl = null;

    //落下地点のY座標
    float m_FallEndPositionY;
    //コンベアシステムオブジェクト
    private GameObject m_StageConveyor = null;

    // Start is called before the first frame update
    void Start()
    {
        m_StageConveyor = GameObject.FindGameObjectWithTag("StageConveyor");
        m_SplashControl = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SplashControl>();

        if(FallEndPlane == null)
        {
            Debug.Break();
            Debug.Log("落下地点のオブジェクトが設定されていません。");
        }
        m_FallEndPositionY = FallEndPlane.GetComponent<Transform>().position.y;
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        //PlayerPointと触れたら
        if (other.gameObject.layer == 11)
        {
            FallLine();
        }
    }

    //FallStartLineにPlayerPointが到達したらコンベアシステムに通知する
    private void FallLine()
    {
        m_SplashControl.isActive = false;//滝から落ちる時にイカダのエフェクトを消す
        var FallLineSyatem_ = m_StageConveyor.GetComponent<IStageConveyorSystem>();
        if(FallLineSyatem_ != null)
        {
            m_StageConveyor.GetComponent<IStageConveyorSystem>().OnFallLineSystem(m_FallEndPositionY);
        }
    }
}
