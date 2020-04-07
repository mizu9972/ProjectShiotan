using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour {
    [SerializeField, Header("取得するアイテムTag")]
    private List<string> ItemTag;

    // 攻撃開始
    private void OnTriggerEnter(Collider other) {
        // アイテム探索
        bool IsItem = false;
        foreach(string tag in ItemTag) {
            if(other.tag == tag) {
                IsItem = true;
                break;
            }
        }

        // アイテム使用処理
        if (IsItem) {
            other.gameObject.GetComponent<ItemBase>().UseItem();
        }

        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            // 追いかけているオブジェクトと同一なら攻撃開始
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = true;
                foreach (GameObject Piranha in transform.parent.gameObject.GetComponent<FlockBase>().ChildPiranha) {
                    Piranha.GetComponent<PiranhaBase>().FirstAttackTiming();
                }
            }
        }
    }

    // 攻撃
    private void OnTriggerStay(Collider other) {
        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                foreach (GameObject Piranha in transform.parent.gameObject.GetComponent<FlockBase>().ChildPiranha) {
                    Piranha.GetComponent<PiranhaBase>().Attack();

                    // 死んだかチェック
                    if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0].gameObject.GetComponent<HumanoidBase>().DeadCheck()) {
                        // 死んでいる場合、ターゲットを削除し、攻撃を終了する
                        transform.parent.gameObject.GetComponent<AIFlock>().TargetList.RemoveAt(0);
                        transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
                        return;
                    }
                }
            }
        }
    }

    // 攻撃中断
    private void OnTriggerExit(Collider other) {
        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
            }
        }
    }
}
