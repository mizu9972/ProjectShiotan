using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldBase : MonoBehaviour
{
    [SerializeField] private GameObject BattleCenter;
    private bool IsHostility = false;
    private string PlayerTag = "Player";

    [SerializeField] private List<GameObject> TotalEnemy = new List<GameObject>();
    private int MaxEnemyCount = 0;
    [SerializeField] private List<GameObject> TotalFlock = new List<GameObject>();
    [SerializeField] private HumanoidBase TotalFlockHumanoidBase;

    // Update is called once per frame
    void Update()
    {
        BattleOfEnemAndFlock();
        BattleEndCheck();
        UpdatePosition();
    }

    #region BattleFieldBaseで使う関数
    #region 攻撃系
    /// <summary>
    /// 敵とピラニア群のバトル関数
    /// </summary>
    private void BattleOfEnemAndFlock() 
    {
        // ピラニア群と敵が存在し、敵対している場合に処理を行う
        if ((TotalFlock.Count > 0 && TotalEnemy.Count > 0) && !IsHostility) {
            // ピラニア群からの攻撃
            AttackFlock();
            AttackEnemy();
        }
        // ピラニア群と敵のセンターへの攻撃
        else{
            AttackFoodFlock();
            AttackFoodEnemy();
        }
    }

    /// <summary>
    /// ピラニア群の攻撃    
    /// </summary>
    private void AttackFlock() 
    {
        int AttackCount = 0;
        if (TotalFlock.Count > 0) {
            foreach (GameObject Flock in TotalFlock) {
                foreach (GameObject Piranha in Flock.GetComponent<FlockBase>().ChildPiranha) {
                    if (TotalEnemy.Count > 0) {
                        // ターゲットに順番に攻撃するようにしてます
                        Piranha.GetComponent<PiranhaBase>().Attack(TotalEnemy[AttackCount % TotalEnemy.Count]);

                        // 死んだかの確認
                        if (TotalEnemy[AttackCount % TotalEnemy.Count].GetComponent<HumanoidBase>().DeadCheck()) {
                            // ToDo::死んだときのエフェクト等を表示

                            // ターゲットから削除
                            TotalEnemy.RemoveAt(AttackCount % TotalEnemy.Count);
                        }
                        AttackCount++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 敵の攻撃
    /// </summary>
    private void AttackEnemy() 
    {
        if(TotalEnemy.Count > 0) {
            foreach(GameObject Enemy in TotalEnemy) {
                // ToDo::エフェクトの作成

                // 攻撃
                Enemy.GetComponent<EnemyBase>().Attack(TotalFlockHumanoidBase);
            }
        }
    }

    /// <summary>
    /// ピラニア群の餌への攻撃
    /// </summary>
    private void AttackFoodFlock() {
        if (TotalFlock.Count > 0) {
            foreach (GameObject Flock in TotalFlock) {
                foreach (GameObject Piranha in Flock.GetComponent<FlockBase>().ChildPiranha) {
                    if (BattleCenter != null) {
                        Piranha.GetComponent<PiranhaBase>().Attack(BattleCenter);
                    }
                    else {
                        DestroyThisField();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 敵の餌への攻撃
    /// </summary>
    private void AttackFoodEnemy() {
        if (TotalEnemy.Count > 0) {
            foreach (GameObject Enemy in TotalEnemy) {
                if (BattleCenter != null) {

                    // ToDo::エフェクトの作成

                    // 攻撃
                    Enemy.GetComponent<EnemyBase>().Attack(BattleCenter.GetComponent<HumanoidBase>());
                }
                else {
                    DestroyThisField();
                }
            }
        }
    }
    #endregion
    #region 死亡系
    private void BattleEndCheck() {
        // ピラニア群が死んだか、敵がいなくなった時のみ処理を行う
        if(TotalFlockDeadCheck() || TotalEnemyDeadCheck() || IsFlockAndEnemyNon()) {
            DestroyThisField();
        }
    }

    /// <summary>
    /// プレイヤーのデッドチェック
    /// </summary>
    /// <returns></returns>
    private bool TotalFlockDeadCheck() {
        if (TotalFlock.Count > 0) {
            if (TotalFlockHumanoidBase.NowHP <= 0) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// エネミーのデッドチェック
    /// </summary>
    /// <returns></returns>
    private bool TotalEnemyDeadCheck() {
        if (TotalEnemy.Count <= 0 && MaxEnemyCount > 0) {
            return true;
        }
        return false;
    }


    private bool IsFlockAndEnemyNon() {
        if(TotalFlock.Count <= 0 && TotalEnemy.Count <= 0) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// このフィールドを削除するときに実行
    /// </summary>
    private void DestroyThisField() {
        // 各ピラニアのHPを設定しなおす
        //foreach (GameObject Flock in TotalFlock) {
        //    Flock.GetComponent<HumanoidBase>().NowHP = TotalFlockHumanoidBase.NowHP / TotalFlock.Count;
        //}

        // 餌の時のみ処理を行う(敵対するものだけ処理を行うという風になっている)
        if (!IsHostility) {
            // 餌の時に自動削除再開
            BattleCenter.GetComponent<EsaDestroy>().IsCountDown(true);

            // バトルの中心地を削除
            Destroy(BattleCenter);
        }

        // 削除
        Destroy(gameObject);
    }
    #endregion
    #region etc
    /// <summary>
    /// 座標を更新
    /// </summary>
    private void UpdatePosition() {
        if (BattleCenter) {
            gameObject.transform.position = BattleCenter.transform.position;
        }
    }
    #endregion
    #endregion

    #region 外部からアクセスする関数
    /// <summary>
    ///  バトル地の設定
    /// </summary>
    /// <param name="obj"></param>
    public void SetBattleCenter(GameObject obj) 
    {
        BattleCenter = obj;

        // センターがプレイヤーのタグの場合、敵対しない
        if(BattleCenter.tag == PlayerTag) {
            IsHostility = true;
        }
        else {
            IsHostility = false;
            // 餌の自動削除停止
            obj.GetComponent<EsaDestroy>().IsCountDown(false);
        }
    }

    /// <summary>
    /// 設定されているバトル地を取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetBattleCenter() {
        return BattleCenter;
    }

    /// <summary>
    /// ピラニア群の追加
    /// </summary>
    /// <param name="Flock"></param>
    public void AddFlock(GameObject Flock) 
    {
        if (TotalFlock.Count <= 0) {
            gameObject.AddComponent<HumanoidBase>();
            TotalFlockHumanoidBase = gameObject.GetComponent<HumanoidBase>();
        }

        TotalFlock.Add(Flock);

        TotalFlockHumanoidBase.InitHP += Flock.GetComponent<HumanoidBase>().InitHP;
        TotalFlockHumanoidBase.InitAttackPower += Flock.GetComponent<HumanoidBase>().InitAttackPower;

        TotalFlockHumanoidBase.NowHP += Flock.GetComponent<HumanoidBase>().NowHP;
        TotalFlockHumanoidBase.NowAttackPower += Flock.GetComponent<HumanoidBase>().NowAttackPower;

        // ピラニア群のバトル初動
        foreach(GameObject Piranha in Flock.GetComponent<FlockBase>().ChildPiranha) {
            // 攻撃タイミングの調整
            Piranha.GetComponent<PiranhaBase>().FirstAttackTiming();
        }
    }

    /// <summary>
    /// 敵の追加
    /// </summary>
    /// <param name="Enemy"></param>
    public void AddEnemy(GameObject Enemy) 
    {
        TotalEnemy.Add(Enemy);
        MaxEnemyCount++;
    }

    /// <summary>
    /// ピラニア群の削除
    /// </summary>
    /// <param name="Flock">削除オブジェクト</param>
    public void RemoveFlock(GameObject Flock) {
        if (TotalFlock.Contains(Flock)) {
            // 各パラメータの振り分けと初期の更新(振り分けは行わない)
            //Flock.GetComponent<HumanoidBase>().NowHP = TotalFlockHumanoidBase.NowHP / TotalFlock.Count;
            TotalFlockHumanoidBase.NowHP -= TotalFlockHumanoidBase.NowHP / TotalFlock.Count;
            TotalFlockHumanoidBase.InitHP -= Flock.GetComponent<HumanoidBase>().InitHP;

            //Flock.GetComponent<HumanoidBase>().NowAttackPower = TotalFlockHumanoidBase.NowAttackPower / TotalFlock.Count;
            TotalFlockHumanoidBase.NowAttackPower -= TotalFlockHumanoidBase.NowAttackPower / TotalFlock.Count;
            TotalFlockHumanoidBase.InitAttackPower -= Flock.GetComponent<HumanoidBase>().InitAttackPower;

            TotalFlock.Remove(Flock);
        }
    }

    /// <summary>
    /// 敵の削除
    /// </summary>
    /// <param name="Enemy">削除オブジェクト</param>
    public void RemoveEnemy(GameObject Enemy) {
        if (TotalEnemy.Contains(Enemy)) {
            TotalEnemy.Remove(Enemy);
        }
    }
    #endregion
}