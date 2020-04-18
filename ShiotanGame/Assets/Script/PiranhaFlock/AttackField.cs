﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour {
    [SerializeField, Header("取得するアイテムTag")]
    private List<string> ItemTag;

    [SerializeField, Header("ピラニアが攻撃する際のプレハブ")] private GameObject BattlePrefab;
    private string BattleFlockTag = "FlockPiranhaBattleField";
    [SerializeField]private List<GameObject> NearBattleFlock = new List<GameObject>();  // 近くでピラニアが群れで攻撃しているオブジェクトを保存

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

        // 近くで攻撃している群れがあれば保存
        if (other.tag == BattleFlockTag) {
            if (!NearBattleFlock.Contains(other.gameObject.transform.parent.gameObject)) {
                NearBattleFlock.Add(other.gameObject.transform.parent.gameObject);
            }
        }

        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            // 追いかけているオブジェクトと同一なら攻撃開始
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = other.gameObject;
                GameObject FoundObject = null;
                if (NearBattleFlock.Count > 0) {
                    BattlePiranhaFlockBase test = NearBattleFlock[0].GetComponent<BattlePiranhaFlockBase>();
                    foreach (GameObject Battle in NearBattleFlock) {
                        if (Battle.GetComponent<BattlePiranhaFlockBase>().BattleCenter == gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject) {
                            FoundObject = Battle;
                            break;
                        }
                    }
                }

                if (FoundObject) {
                    FoundObject.GetComponent<BattlePiranhaFlockBase>().ParticipationFlock(gameObject.transform.parent.gameObject);
                    FoundObject.GetComponent<BattlePiranhaFlockBase>().BattleCenter = gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject;
                }
                else {
                    GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                    CreateObj.GetComponent<BattlePiranhaFlockBase>().ParticipationFlock(gameObject.transform.parent.gameObject);
                    CreateObj.GetComponent<BattlePiranhaFlockBase>().BattleCenter = gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject;
                }
                transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = true;
            }
        }
    }

    // 攻撃
    //private void OnTriggerStay(Collider other) {
    //    // ターゲットがいるときのみ処理を行う
    //    if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
    //        if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
    //            foreach (GameObject Piranha in transform.parent.gameObject.GetComponent<FlockBase>().ChildPiranha) {
    //                Piranha.GetComponent<PiranhaBase>().Attack();

    //                // 死んだかチェック
    //                if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0].gameObject.GetComponent<HumanoidBase>().DeadCheck()) {
    //                    // 死んでいる場合、ターゲットを削除し、攻撃を終了する
    //                    transform.parent.gameObject.GetComponent<AIFlock>().TargetList.RemoveAt(0);
    //                    transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //}

    // 攻撃中断
    private void OnTriggerExit(Collider other) {
        if (other.tag == BattleFlockTag) {
            // 近くで攻撃している群れであれば削除
            if (NearBattleFlock.Contains(other.gameObject)) {
                NearBattleFlock.Remove(other.gameObject);
            }
        }

        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = null;
                transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
            }
        }
    }
}
