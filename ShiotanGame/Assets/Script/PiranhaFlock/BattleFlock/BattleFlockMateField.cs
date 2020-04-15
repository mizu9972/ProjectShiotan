using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFlockMateField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // ピラニアの敵に攻撃する群衆が近くにあるときに攻撃元が同じかを確認する
        if(other.transform.parent.name == gameObject.transform.parent.name) {
            if(other.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().BattleCenter == gameObject.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().BattleCenter) {
                if (!other.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().DeleteFlag) {
                    gameObject.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().BattleCenter = null;
                    gameObject.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().DeleteFlag = false;

                    // 当たったほう側に追加
                    foreach (GameObject Flock in gameObject.transform.parent.gameObject.GetComponent<BattlePiranhaFlockBase>().TotalFlock) {
                        other.gameObject.GetComponent<BattlePiranhaFlockBase>().ParticipationFlock(Flock);
                    }

                    // 基となったオブジェクトは削除
                    Destroy(gameObject.transform.parent.gameObject);
                }
            }
        }
    }
}
