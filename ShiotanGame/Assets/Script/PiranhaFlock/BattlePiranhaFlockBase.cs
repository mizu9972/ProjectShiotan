using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePiranhaFlockBase : MonoBehaviour
{
    // 現在戦闘を行っているピラニア群
    public List<GameObject> TotalFlock;

    // 攻撃を行うターゲットリスト
    public List<GameObject> TargetList;

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
        
    }

    public void ParticipationFlock(GameObject Flock) 
    {
        // 群衆にすでにいるかを探索。いないときに処理する
        if (!TotalFlock.Contains(Flock)) {
            TotalFlock.Add(Flock);

            // 各パラメータの更新
            TotalHP += Flock.GetComponent<HumanoidBase>().InitHP;
            TotalAttackPower += Flock.GetComponent<HumanoidBase>().InitAttackPower;

            // 現在のパラメータ設定
            NowHP += Flock.GetComponent<HumanoidBase>().NowHP;
            NowAttackPower += Flock.GetComponent<HumanoidBase>().NowAttackPower;
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