using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Header("左端オブジェ")]
    private GameObject LeftObj = null;

    [SerializeField, Header("右端オブジェ")]
    private GameObject RightObj = null;

    [SerializeField, Header("左端オブジェとの距離")]
    private float LeftDistance = 0f;

    [SerializeField, Header("右端オブジェとの距離")]
    private float RightDistance = 0f;

    [SerializeField, Header("Z座標")]
    private float ZPos = 0f;

    private Transform MyPos=null;//ポジション

    [SerializeField, Header("左側へ移動可能か")]
    private bool isMoveLeft = true;

    [SerializeField, Header("右側へ移動可能か")]
    private bool isMoveRight = true;

    [Header("プレイヤー")]
    public GameObject Player = null;

    [Header("ターゲットからどれくらい離れたら追従開始するか")]
    public float ChaseStartDistance = 0f;

    [SerializeField, Header("追従状態か")]
    private bool isChase = false;
    // Start is called before the first frame update
    void Start()
    {
        MyPos = this.GetComponent<Transform>();
        ZPos = MyPos.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        ChaceTarget();
        CalcDistance();
    }

    private void CalcTargetDistance()//追従対象との距離を求める
    {
        float distance = Mathf.Abs(ZPos - Player.transform.position.z);//z座標の差分を求める
        if(distance>=ChaseStartDistance)//指定した範囲外にいけば追従開始フラグを立てる
        {
            isChase = true;
        }
        else
        {
            isChase = false;
        }
    }

    private void ChaceTarget()//ターゲット追従処理
    {
        CalcTargetDistance();//ターゲットとの距離を計算
        ZPos = Mathf.Lerp(MyPos.position.z, Player.transform.position.z, Time.deltaTime);
        //MyPos.position=
        if(isChase)
        {
            Vector3 workpos = new Vector3(MyPos.position.x, MyPos.position.y, ZPos);
            MyPos.position = workpos;
        }
    }

    private void CalcDistance()//左右の端オブジェとの距離を求める
    {
        ZPos = MyPos.position.z;//z座標を更新
        
        LeftDistance = Mathf.Abs(ZPos - LeftObj.transform.position.z);//左端との距離を求める
        RightDistance= Mathf.Abs(ZPos - RightObj.transform.position.z);//右端との距離を求める
        
        //距離が0になったらカメラの移動可能フラグをfalseに
        if (LeftDistance <= 0f)
        {
            isMoveLeft = false;
        }
        else
        {
            isMoveLeft = true;
        }
        if(RightDistance <= 0f)
        {
            isMoveRight = false;
        }
        else
        {
            isMoveRight = true;
        }
    }
}
