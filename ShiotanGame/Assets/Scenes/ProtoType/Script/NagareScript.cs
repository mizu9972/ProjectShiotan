using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagareScript : MonoBehaviour
{
    public float XDir;
    public float ZDir;

    // ターゲットが群衆探索範囲に触れた時に呼ばれるメソッド
    private void OnTriggerStay(Collider other)
    {
        // 座標を取得
        Vector3 pos = other.transform.position;
        pos.x += XDir;    // x座標へ加算
        pos.z += ZDir;    // z座標へ加算

        if (other.tag == "Player" || other.tag == "Esa"|| other.tag == "Enemy" || other.tag == "Fish")
        {
            other.transform.position=pos;
        }
    }
}
