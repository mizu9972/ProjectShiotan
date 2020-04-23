using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryFieldAlligator : MonoBehaviour
{
    [SerializeField] private List<string> TargetTag;

    // ターゲットが群衆探索範囲に触れた時に呼ばれるメソッド
    private void OnTriggerEnter(Collider other) {
        // タグの検索
        bool IsFind = false;
        foreach (string Tag in TargetTag) {
            if (Tag == other.tag) {
                IsFind = true;
                break;
            }
        }

        if (IsFind) {
            // ターゲットが既にリストにないかをチェック
            bool IsAdd = true;
            foreach (GameObject Target in gameObject.transform.parent.GetComponent<AIAlligator>().TargetList) {
                if (Target == other.gameObject) {
                    IsAdd = false;
                    break;
                }
            }

            // かぶっていなければ追加する
            if (IsAdd) {
                gameObject.transform.parent.GetComponent<AIAlligator>().TargetList.Add(other.gameObject);
            }
        }
    }
}
