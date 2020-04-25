﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    [SerializeField,Header("何秒でダメージはいるか")]
    private float DamageTime;
    
    private HumanoidBase HPcnt;

    private ProtoMove2 MoveStop;

    private float time;

    private RespawnScript resscript;

    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    private Rigidbody rb;

    [Header("沈む深さ"), SerializeField] private float Depth;

    void Start()
    {
        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();

        resscript = GameObject.Find("Respawn").GetComponent<RespawnScript>();
        MoveStop = GetComponent<ProtoMove2>();
        HPcnt = GetComponent<HumanoidBase>();
        time = 0;
    }

    void Update()
    {
        // 座標を取得
        Vector3 pos = transform.position;
        if (HPcnt.DeadCheck())
        {
            rb.useGravity = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponentInChildren<BoxCollider>().enabled = false;
            this.GetComponent<WaveAct>().enabled = false;

            MoveStop.Stop();

            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-70f, 0, 0), Time.deltaTime/2);

            //沈む処理
            if(pos.y> Depth)
            {
                pos.y -= 1 * Time.deltaTime;
                transform.position = pos;
            }
        }
    }

    private void OnDestroy()
    {
        resscript.Respawn = true;
    }

    //当たるのをやめたとき
    void OnCollisionExit(Collision other)
    {
        time = 0;
    }

    public float GetNowHP()
    {
        return HPcnt.NowHP;
    }
}
