using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePiranhaFlockBase : MonoBehaviour
{
    // 現在戦闘を行っているピラニア群
    public List<GameObject> TotalFlock;

    // 攻撃を行うターゲットリスト
    public List<GameObject> TargetList;

    // 攻撃している餌
    public GameObject BattleCenter;

    // 消す際にtrueにするflag
    public bool DeleteFlag = false;

    // ピラニア総数
    private int TotalPiranhaCount = 0;

    // Init的なもの
    public float TotalHP;
    public float TotalAttackPower;

    // この値でやり取りをする
    public float NowHP;
    public float NowAttackPower;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackTarget();
        UpdateThisHumanoidBase();
    }

    void AttackTarget() 
    {
        int AttackCount = 0;
        foreach(GameObject Flock in TotalFlock) {
            foreach (GameObject Piranha in Flock.GetComponent<FlockBase>().ChildPiranha) {
                if (TargetList.Count > 0) {
                    // ターゲットに順番に攻撃するようにしてます
                    Piranha.GetComponent<PiranhaBase>().Attack(TargetList[AttackCount % TargetList.Count]);

                    // 死んだかの確認
                    if (TargetList[AttackCount % TargetList.Count].GetComponent<HumanoidBase>().DeadCheck()) {
                        // ToDo::死んだときのエフェクト等を表示

                        // ターゲットから削除
                        TargetList.RemoveAt(AttackCount % TargetList.Count);
                    }
                    AttackCount++;
                }
            }
        }
        // 解散チェック
        DissolutionCheck();
    }

    private void UpdateThisHumanoidBase() 
    {
        NowHP = gameObject.GetComponent<HumanoidBase>().NowHP;
        NowAttackPower = gameObject.GetComponent<HumanoidBase>().NowAttackPower;

        // 箸休め程度に攻撃ターゲットを入れる
        if (TargetList.Count > 0) {
            gameObject.GetComponent<HumanoidBase>().AttackObject = TargetList[0];
        }

        // 解散チェック
        DissolutionCheck();
    }

    /// <summary>
    /// 解散チェック
    /// </summary>
    private void DissolutionCheck() 
    {
        // ターゲットがいなくなったか、HPが0になった時のみ処理を行う
        if(TargetList.Count <= 0 || NowHP <= 0) {
            // ToDo::解散処理
            for (int i = 0; i < TotalFlock.Count; i++) {
                // ToDo::群衆のHPバーをOFFにする
                // TargetList[0].GetComponent<>()

                // ピラニア群の攻撃フィールドをONにする
                TargetList[0].transform.Find("AttackField").gameObject.SetActive(true);

                TargetList.Remove(TargetList[0]);
            }
        }
    }

    /// <summary>
    /// 攻撃ピラニア群の追加
    /// </summary>
    /// <param name="Flock">ピラニア群</param>
    public void ParticipationFlock(GameObject Flock) 
    {
        // 群衆にすでにいるかを探索。いないときに処理する
        if (!TotalFlock.Contains(Flock)) {
            TotalFlock.Add(Flock);
            // ToDo::群衆のHPバーをOFFにする
            // Flock.GetComponent<>()

            // ピラニア群の攻撃フィールドをOFFにする
            Flock.transform.Find("AttackField").gameObject.SetActive(false);

            // 各パラメータの更新
            TotalHP += Flock.GetComponent<HumanoidBase>().InitHP;
            TotalAttackPower += Flock.GetComponent<HumanoidBase>().InitAttackPower;

            // 現在のパラメータ設定
            NowHP += Flock.GetComponent<HumanoidBase>().NowHP;
            NowAttackPower += Flock.GetComponent<HumanoidBase>().NowAttackPower;

            // HumanoidBaseの初期ステータスを更新
            gameObject.GetComponent<HumanoidBase>().InitHP = TotalHP;
            gameObject.GetComponent<HumanoidBase>().InitAttackPower = TotalAttackPower;

            // ピラニア総数更新
            TotalPiranhaCount += Flock.GetComponent<FlockBase>().ChildPiranha.Count;
        }
    }

    public void AddTarget(GameObject Target) 
    {
        // ターゲットが既にいるかを探索。いないときに処理する
        if (!TargetList.Contains(Target)) {
            TargetList.Add(Target);
        }
    }
}