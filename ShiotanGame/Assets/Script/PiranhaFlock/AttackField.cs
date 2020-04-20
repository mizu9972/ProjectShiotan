﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour {
    [SerializeField, Header("取得するアイテムTag")]
    private List<string> ItemTag;

    [SerializeField, Header("ピラニアが攻撃する際のプレハブ")] private GameObject BattlePrefab;
    private string BattleFlockTag = "FlockPiranhaBattleField";
    [SerializeField] private List<GameObject> NearBattleFlock = new List<GameObject>();  // 近くでピラニアが群れで攻撃しているオブジェクトを保存
    [SerializeField] private GameObject AffiliationBattleField;

    private void Update() {
        // Missingになったオブジェクトがあれば削除する
        List<int> DeleteArrayNum = new List<int>();
        for (int i = 0; i < NearBattleFlock.Count; i++) {
            if (NearBattleFlock[i] == null) {
                DeleteArrayNum.Add(i);
            }
        }

        if (NearBattleFlock.Count == DeleteArrayNum.Count) {
            NearBattleFlock.Clear();
        }
        else {
            for (int i = DeleteArrayNum.Count; i > 0; i--) {
                NearBattleFlock.RemoveAt(DeleteArrayNum[i]);
            }
        }
    }

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
            if (!NearBattleFlock.Contains(other.gameObject)) {
                NearBattleFlock.Add(other.gameObject);
            }
        }

        // ターゲットがいるときのみ処理を行う
        if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
            // 追いかけているオブジェクトと同一なら攻撃開始
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
                // 現在所属のフィールドが存在するなら抜ける処理を行う
                if (AffiliationBattleField) {
                    AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveFlock(gameObject.transform.parent.gameObject);
                    AffiliationBattleField = null;
                }

                gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = other.gameObject;
                GameObject FoundObject = null;
                if (NearBattleFlock.Count > 0) {
                    //BattlePiranhaFlockBase test = NearBattleFlock[0].GetComponent<BattlePiranhaFlockBase>();
                    foreach (GameObject Battle in NearBattleFlock) {
                        if (Battle.GetComponent<BattleFieldBase>().GetBattleCenter() == gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject) {
                            FoundObject = Battle;
                            break;
                        }
                    }
                }

                if (FoundObject) {
                    FoundObject.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                    FoundObject.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    AffiliationBattleField = FoundObject;
                }
                else {
                    GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                    CreateObj.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                    CreateObj.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    AffiliationBattleField = CreateObj;
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

        // バトルから抜ける
        if(other.gameObject == AffiliationBattleField) {
            gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = null;
            transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;

            AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveFlock(gameObject.transform.parent.gameObject);
            AffiliationBattleField = null;
        }

        //// ターゲットがいるときのみ処理を行う
        //if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
        //    if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
        //        gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = null;
        //        transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
        //    }
        //}
    }
}
