using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class CameraTest : MonoBehaviour
{
    [SerializeField]
    Camera PlayerCamera;

    [Header("追従するターゲット")]
    public GameObject Target = null;

    [Header("移動範囲(指定した値の正負が移動範囲になります)")]
    public Vector3 Range = new Vector3(10.0f,50.0f,10.0f);

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
        MyTrans.position = new Vector3(Mathf.Clamp(MyTrans.position.x,-Range.x,Range.x),
                                       Mathf.Clamp(MyTrans.position.y, -Range.y, Range.y),
                                       Mathf.Clamp(MyTrans.position.z, -Range.z+Distance.z, Range.z));
    }
}
