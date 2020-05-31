using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour {
    [SerializeField, Header("取得するアイテムTag")]
    private List<string> ItemTag;

    [SerializeField, Header("ピラニアが攻撃する際のプレハブ")] private GameObject BattlePrefab;
    private string BattleFlockTag = "FlockPiranhaBattleField";
    [SerializeField] private List<GameObject> NearBattleFlock = new List<GameObject>();  // 近くでピラニアが群れで攻撃しているオブジェクトを保存
    [SerializeField] private GameObject AffiliationBattleField;

    [SerializeField, Header("ピラニアがフリーで攻撃するタグ")] private List<string> FreeBiteTag;

    private void Update() {
        // Missingになったオブジェクトがあれば削除する
        List<int> DeleteArrayNum = new List<int>();
        for (int i = 0; i < NearBattleFlock.Count; i++) {
            if (NearBattleFlock[i] == null) {
                NearBattleFlock.Remove(NearBattleFlock[i]);
            }
        }

        if (AffiliationBattleField == null) {
            AffiliationBattleField = null;
            transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;
        }
    }

    /// <summary>
    /// バトルから抜ける処理
    /// </summary>
    public void RemoveBattle() {
        if (AffiliationBattleField) {
            gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = null;
            transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = false;

            AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveFlock(gameObject.transform.parent.gameObject);
            AffiliationBattleField = null;
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
                    FoundObject.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    FoundObject.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                    AffiliationBattleField = FoundObject;
                }
                else {
                    GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                    CreateObj.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    CreateObj.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                    AffiliationBattleField = CreateObj;
                }
                transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = true;
            }
        }
    }

    // 攻撃(FreeBite)
    private void OnTriggerStay(Collider other) {
        foreach(string tag in FreeBiteTag) {
            if(other.tag == tag) {
                foreach(GameObject Piranha in transform.parent.gameObject.GetComponent<FlockBase>().ChildPiranha) {
                    Piranha.GetComponent<PiranhaBase>().Attack(other.gameObject, "SE_BITE");
                }
            }
        }

        // バトルが行われていないとき
        if (!AffiliationBattleField) {
            // ターゲットがいるときのみ処理を行う
            if (transform.parent.gameObject.GetComponent<AIFlock>().TargetList.Count > 0) {
                // 追いかけているオブジェクトと同一なら攻撃開始
                if (other.gameObject == transform.parent.gameObject.GetComponent<AIFlock>().TargetList[0]) {
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
                        FoundObject.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                        FoundObject.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                        AffiliationBattleField = FoundObject;
                    }
                    else {
                        GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                        CreateObj.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                        CreateObj.GetComponent<BattleFieldBase>().AddFlock(gameObject.transform.parent.gameObject);
                        AffiliationBattleField = CreateObj;
                    }
                    transform.parent.gameObject.GetComponent<AIFlock>().IsAttack = true;
                }
            }
        }
    }

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
