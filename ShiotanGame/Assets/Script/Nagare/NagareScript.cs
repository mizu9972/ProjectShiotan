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

    // ターゲットが流れの上に乗った時に呼ばれるメソッド
    private void OnTriggerStay(Collider other)
    {
        Vector3 pos = other.transform.position;

        //流れの勢い　計算したものを入れる変数
        float SaveX = 0;
        float SaveZ = 0;

        //乗ったオブジェクトごとに勢いの値変わる
        if (other.tag == "Player")
        {
            SaveX += XDir * P_Flow;    // x座標へ加算
            SaveZ += ZDir * P_Flow;    // z座標へ加算
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
        rb.AddForce(SaveX, 0, SaveZ);
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
