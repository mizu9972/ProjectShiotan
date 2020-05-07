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
    
    //private Rigidbody rb;

    // ターゲットが流れの上に乗った時に呼ばれるメソッド
    private void OnTriggerStay(Collider other)
    {
        Vector3 pos = other.transform.position;

        ////流れの勢い　計算したものを入れる変数
        //float SaveX = 0;
        //float SaveZ = 0;

        //乗ったオブジェクトごとに勢いの値変わる
        if (other.tag == "Player")
        {
            pos.x += XDir * P_Flow;    // x座標へ加算
            pos.z += ZDir * P_Flow;    // z座標へ加算
        }
        if (other.tag == "Esa")
        {
            pos.x += XDir * EsaFlow;    // x座標へ加算
            pos.z += ZDir * EsaFlow;    // z座標へ加算
        }
        if (other.tag == "Enemy")
        {
            pos.x += XDir * E_Flow;    // x座標へ加算
            pos.z += ZDir * E_Flow;    // z座標へ加算
        }
        if (other.tag == "Fish")
        {
            pos.x += XDir * FishFlow;    // x座標へ加算
            pos.z += ZDir * FishFlow;    // z座標へ加算
        }

        //rb = other.GetComponentInParent<Rigidbody>();

        //Rigidbodyに力を加える
        //rb.AddForce(SaveX, 0, SaveZ);

        other.transform.position = pos;
    }

    private void OnTriggerExit(Collider other)
    {

    }
    
}
