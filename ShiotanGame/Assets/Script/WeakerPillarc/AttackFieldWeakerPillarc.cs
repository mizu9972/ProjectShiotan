using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFieldWeakerPillarc : MonoBehaviour
{
    [SerializeField, Header("取得するアイテムTag")]
    private List<string> ItemTag;

    [SerializeField, Header("ピラルクが攻撃する際のプレハブ")] private GameObject BattlePrefab;
    private string BattleFlockTag = "FlockPiranhaBattleField";
    [SerializeField] private List<GameObject> NearBattleFlock = new List<GameObject>();  // 近くで攻撃しているオブジェクトを保存
    [SerializeField] private GameObject AffiliationBattleField;

    [SerializeField, Header("ピラルクがフリーで攻撃するタグ")] private List<string> FreeBiteTag;

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
            transform.parent.gameObject.GetComponent<AIWeakerPillarc>().IsAttack = false;
        }
    }

    // 攻撃開始
    private void OnTriggerEnter(Collider other) {
        // 旧Ver
        {
            //// アイテム探索
            //bool IsItem = false;
            //foreach (string tag in ItemTag) {
            //    if (other.tag == tag) {
            //        IsItem = true;
            //        break;
            //    }
            //}

            //// アイテム使用処理
            //if (IsItem) {
            //    other.gameObject.GetComponent<ItemBase>().UseItem();
            //}

            //// ターゲットがいるときのみ処理を行う
            //if (transform.parent.gameObject.GetComponent<AIPillarc>().TargetList.Count > 0) {
            //    // 追いかけているオブジェクトと同一なら攻撃開始
            //    if (other.gameObject == transform.parent.gameObject.GetComponent<AIPillarc>().TargetList[0]) {
            //        transform.parent.gameObject.GetComponent<AIPillarc>().IsAttack = true;
            //    }
            //}
        }

        // 新Ver
        // アイテム探索
        bool IsItem = false;
        foreach (string tag in ItemTag) {
            if (other.tag == tag) {
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
        if (transform.parent.gameObject.GetComponent<AIWeakerPillarc>().TargetList.Count > 0) {
            // 追いかけているオブジェクトと同一なら攻撃開始
            if (other.gameObject == transform.parent.gameObject.GetComponent<AIWeakerPillarc>().TargetList[0]) {
                // 現在所属のフィールドが存在するなら抜ける処理を行う
                if (AffiliationBattleField) {
                    AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveEnemy(gameObject.transform.parent.gameObject);
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
                    FoundObject.GetComponent<BattleFieldBase>().AddEnemy(gameObject.transform.parent.gameObject);
                    FoundObject.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    AffiliationBattleField = FoundObject;
                }
                else {
                    GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                    CreateObj.GetComponent<BattleFieldBase>().AddEnemy(gameObject.transform.parent.gameObject);
                    CreateObj.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                    AffiliationBattleField = CreateObj;
                }
                transform.parent.gameObject.GetComponent<AIWeakerPillarc>().IsAttack = true;
            }
        }
    }

    // 攻撃(FreeBite)
    private void OnTriggerStay(Collider other) {
        foreach (string tag in FreeBiteTag) {
            if (other.tag == tag) {
                gameObject.transform.parent.GetComponent<EnemyBase>().Attack(other.gameObject.GetComponent<HumanoidBase>());
            }
        }

        if (!AffiliationBattleField) {
            // ターゲットがいるときのみ処理を行う
            if (transform.parent.gameObject.GetComponent<AIWeakerPillarc>().TargetList.Count > 0) {
                // 追いかけているオブジェクトと同一なら攻撃開始
                if (other.gameObject == transform.parent.gameObject.GetComponent<AIWeakerPillarc>().TargetList[0]) {
                    // 現在所属のフィールドが存在するなら抜ける処理を行う
                    if (AffiliationBattleField) {
                        AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveEnemy(gameObject.transform.parent.gameObject);
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
                        FoundObject.GetComponent<BattleFieldBase>().AddEnemy(gameObject.transform.parent.gameObject);
                        FoundObject.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                        AffiliationBattleField = FoundObject;
                    }
                    else {
                        GameObject CreateObj = Instantiate(BattlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                        CreateObj.GetComponent<BattleFieldBase>().AddEnemy(gameObject.transform.parent.gameObject);
                        CreateObj.GetComponent<BattleFieldBase>().SetBattleCenter(gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject);
                        AffiliationBattleField = CreateObj;
                    }
                    transform.parent.gameObject.GetComponent<AIPillarc>().IsAttack = true;
                }
            }
        }
    }

    // 攻撃中断
    private void OnTriggerExit(Collider other) {
        // 旧Ver
        {
            //// ターゲットがいるときのみ処理を行う
            //if (transform.parent.gameObject.GetComponent<AIPillarc>().TargetList.Count > 0) {
            //    if (other.gameObject == transform.parent.gameObject.GetComponent<AIPillarc>().TargetList[0]) {
            //        transform.parent.gameObject.GetComponent<AIPillarc>().IsAttack = false;
            //    }
            //}
        }

        // 新Ver
        if (other.tag == BattleFlockTag) {
            // 近くで攻撃している群れであれば削除
            if (NearBattleFlock.Contains(other.gameObject)) {
                NearBattleFlock.Remove(other.gameObject);
            }
        }

        // バトルから抜ける
        if (other.gameObject == AffiliationBattleField) {
            gameObject.transform.parent.gameObject.GetComponent<HumanoidBase>().AttackObject = null;
            transform.parent.gameObject.GetComponent<AIWeakerPillarc>().IsAttack = false;

            AffiliationBattleField.GetComponent<BattleFieldBase>().RemoveEnemy(gameObject.transform.parent.gameObject);
            AffiliationBattleField = null;
            Debug.Log(gameObject.transform.parent.gameObject.name + "にてバトル離脱");
        }
    }
}
