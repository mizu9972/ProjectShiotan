using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockDiscoveryField : MonoBehaviour
{
    // ターゲットが群衆探索範囲に触れた時に呼ばれるメソッド
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "Esa") {
            //if (!gameObject.transform.parent.GetComponent<FlockBase>().TargetObject) {
            gameObject.transform.parent.GetComponent<FlockBase>().DiscoveryTarget(other.gameObject);
            //}
        }
    }
}
