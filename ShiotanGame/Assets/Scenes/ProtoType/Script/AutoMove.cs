using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoMove : MonoBehaviour
{
    public bool isActive = true;

    [SerializeField,Header("徘徊する際のチェックポイント座標群"),Tooltip("徘徊地点の数を指定して徘徊地点の座標に配置したオブジェクト(空オブジェクトでOK)をドラッグ＆ドロップ")]
    private List<Transform> CheckPointList = new List<Transform>();

    [SerializeField, Header("チェックポイントの到着判定距離")]
    private float ArrivalDistance = 1.0f;

    private Vector3 m_NowTargetPoint = Vector3.zero;
    private int m_Iter;
    private NavMeshAgent m_NavMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = this.GetComponent<NavMeshAgent>();
        //初期地点設定
        if (CheckPointList.Count > 0)
        {
            m_NowTargetPoint = CheckPointList[0].position;
            m_NavMeshAgent.destination = CheckPointList[0].position;
        }
        else
        {
            isActive = false;
            Debug.Log("チェックポイントが設定されてません : " + this.gameObject.GetInstanceID());
        }

    }
    // Update is called once per frame
    void Update()
    {
        //目標の範囲内に到着したら次のチェックポイントに目標を変更する
        if(m_NavMeshAgent.remainingDistance < ArrivalDistance)
        {
            m_Iter++;
            if(m_Iter >= CheckPointList.Count)
            {
                m_Iter = 0;
            }
            m_NowTargetPoint = CheckPointList[m_Iter].position;
            m_NavMeshAgent.destination = CheckPointList[m_Iter].position;

            this.NavStop(2.0f);
        }
    }

    public void setTargetPosition(Transform targetTrans)
    {
        //現在追いかけている対象を保存
        m_NowTargetPoint = m_NavMeshAgent.destination;

        //追いかける対象を変更
        m_NavMeshAgent.destination = targetTrans.position;
    }

    public void ResetTargetPosition()
    {
        //保存した対象へ追いかける
        m_NavMeshAgent.destination = m_NowTargetPoint;
    }

    //その場で停止
    public void NavStop()
    {
        m_NavMeshAgent.isStopped = true;
    }

    //一定時間停止
    public void NavStop(float StoppingSecond)
    {
        m_NavMeshAgent.isStopped = true;
        Observable.Timer(System.TimeSpan.FromSeconds(StoppingSecond)).Take(1).Subscribe(_ => m_NavMeshAgent.isStopped = false).AddTo(this);//一定時間後に再開
    }

    //再開
    public void NavStart()
    {
        m_NavMeshAgent.isStopped = false;
    }
}
