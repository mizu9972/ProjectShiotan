using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaFlockAttackField : MonoBehaviour
{
    //[SerializeField, Header("ピラニアが攻撃するタグ")]
    //private List<string> AttackTag;

    //private void OnTriggerEnter(Collider other) {
    //    // 指定したタグかどうかを探索
    //    if (AttackTag.Contains(other.tag)) {
    //        // 敵として追加していない場合追加する
    //        if (!transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().TargetList.Contains(other.gameObject)) {
    //            transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().TargetList.Add(other.gameObject);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().TargetList.Contains(other.gameObject)) {
    //        transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().TargetList.Remove(other.gameObject);
    //    }
    //}
}
