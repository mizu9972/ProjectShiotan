using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class ChaceCamera : MonoBehaviour
{
    [SerializeField]
    Camera PlayerCamera;

    [Header("追従するターゲット")]
    public GameObject Target = null;

    [Header("移動範囲(指定した値が移動範囲の上限値になります)")]
    public Vector3 MaxRange = new Vector3(10.0f,50.0f,10.0f);

    [Header("移動範囲(指定した値が移動範囲の下限値になります)")]
    public Vector3 MinRange = new Vector3(10.0f, 50.0f, 10.0f);

    [Header("ターゲットからどれだけ離れるか")]
    public Vector3 Distance = new Vector3(0.0f, 0.0f, 10.0f);
    //[SerializeField,Header("ステージオブジェクト")]
    //GameObject StageObj;
    [Header("追跡ON")]
    public bool isChase = true;
    [Header("移動制限ON")]
    public bool isRestriction = true;

    private Transform MyTrans;
    void Start()
    {
        PlayerCamera = this.GetComponent<Camera>();
        MyTrans = PlayerCamera.transform;
        this.UpdateAsObservable().
            Where(_ => Target == null).
            Subscribe(_ => Target = GameObject.FindGameObjectWithTag("Player"));
    }

    void Update()
    {
        if(isChase)//追跡
        {
            ChaseTarget();
        }
        if(isRestriction)//移動範囲制限
        {
            MoveRestriction();
        }
    }

    private void ChaseTarget()//ターゲット追跡
    {
        if(Target!=null)
        {
            MyTrans.position = new Vector3(Target.transform.position.x + Distance.x,
                                       MyTrans.position.y + Distance.y,
                                       Target.transform.position.z + Distance.z);
        }
    }

    private void MoveRestriction()//移動制限
    {
        MyTrans.position = new Vector3(Mathf.Clamp(MyTrans.position.x,MinRange.x,MaxRange.x),
                                       Mathf.Clamp(MyTrans.position.y, MinRange.y, MaxRange.y),
                                       Mathf.Clamp(MyTrans.position.z, MinRange.z, MaxRange.z));
    }

    public void SetisChace(bool ischace)//追従状態切り替え
    {
        isChase = ischace;
    }
}
