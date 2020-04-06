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

    // ターゲットが群衆探索範囲に触れた時に呼ばれるメソッド
    private void OnTriggerStay(Collider other)
    {
        // 座標を取得
        Vector3 pos = other.transform.position;

        if (other.tag == "Player")
        {
            //other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(XDir * P_Flow, 0, ZDir * P_Flow);
            pos.x += XDir* P_Flow;    // x座標へ加算
            pos.z += ZDir* P_Flow;    // z座標へ加算
        }
        if (other.tag == "Esa")
        {
            pos.x += XDir*EsaFlow;    // x座標へ加算
            pos.z += ZDir*EsaFlow;    // z座標へ加算
        }
        if (other.tag == "Enemy")
        {
            pos.x += XDir*E_Flow;    // x座標へ加算
            pos.z += ZDir*E_Flow;    // z座標へ加算
        }
        if (other.tag == "Fish")
        {
            pos.x += XDir*FishFlow;    // x座標へ加算
            pos.z += ZDir*FishFlow;    // z座標へ加算
        }

        //位置変更
        other.transform.position = pos;
    }
}
