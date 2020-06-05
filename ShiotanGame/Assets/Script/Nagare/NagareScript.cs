using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagareScript : MonoBehaviour
{
    [Header("プレイヤー・エサ・魚・敵の流れる割合")]
    public float P_Flow;
    public float EsaFlow;
    public float FishFlow;
    public float E_Flow;

    [Header("X方向の流れ　速さ")]
    public float XDir;
    [Header("Y方向の流れ　速さ")]
    public float ZDir;

    private Rigidbody rb;

    private ProtoMove2 spd;

    [Header("加速時 川の流れからの影響")]
    public float NagareDawn;
    [Header("川の流れからの影響　最大値")]
    public float MaxNagareDawn;
    [Header("川の流れからの影響　最小値")]
    public float MinNagareDawn;
    [Header("川の流れからの影響　減らす量")]
    public float NagareDawnPower;
    [Header("川の流れの影響　回復量量")]
    public float NagarePowerRecovery;

    private void Start()
    {
        NagareDawn = MaxNagareDawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spd = other.GetComponentInParent<ProtoMove2>();
        }
    }

    // ターゲットが流れの上に乗っている時に呼ばれるメソッド
    private void OnTriggerStay(Collider other)
    {
        //流れの勢い　計算したものを入れる変数
        float SaveX = 0;
        float SaveZ = 0;

        //加速してるとき　川の流れからの影響下げる
        if ((Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.B)))
        {
            NagareDawn -= NagareDawnPower;
            if(NagareDawn < MinNagareDawn)
            {
                NagareDawn = MinNagareDawn;
            }
        }
        else
        {
            //川の流れの影響　回復
            if(NagareDawn<MaxNagareDawn)
            {
                NagareDawn += NagarePowerRecovery;
                if (NagareDawn >= MaxNagareDawn)
                {
                    NagareDawn = MaxNagareDawn;
                }
            }
        }

        //乗ったオブジェクトごとに勢いの値変わる
        if (other.tag == "Player")
        {
            SaveX += XDir * P_Flow* NagareDawn/ MaxNagareDawn;    // x座標へ加算
            SaveZ += ZDir * P_Flow * NagareDawn/ MaxNagareDawn;    // z座標へ加算
            spd.SetNagare((SaveX + SaveZ)* (SaveX + SaveZ)/10);
        }
        if (other.tag == "Esa")
        {
            SaveX += XDir * EsaFlow;    // x座標へ加算
            SaveZ += ZDir * EsaFlow;    // z座標へ加算
        }
        if (other.tag == "Enemy")
        {
            SaveX += XDir * E_Flow;    // x座標へ加算
            SaveZ += ZDir * E_Flow;    // z座標へ加算
        }
        if (other.tag == "Fish")
        {
            SaveX += XDir * FishFlow;    // x座標へ加算
            SaveZ += ZDir * FishFlow;    // z座標へ加算
        }

        rb = other.GetComponentInParent<Rigidbody>();

        //Rigidbodyに力を加える
        rb.AddForce(SaveX, 0, SaveZ, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
