using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 電気ウナギの特殊攻撃
/// </summary>
public class SparkEelSparkAttackField : MonoBehaviour
{
    private List<GameObject> Target = new List<GameObject>();
    [SerializeField, Header("ターゲットにするタグ")] private List<string> TargetTag = new List<string>();
    private float time;
    [SerializeField,Header("攻撃力(毎フレーム与える)")] private float AttackPower = 0.0f;
    [SerializeField] private float CoolTime = 3.0f;
    [SerializeField] private float AttackTime = 1.5f;
    [SerializeField] private bool IsAttack = false;
    [SerializeField] private GameObject SparkEffect;

    // Start is called before the first frame update
    void Start()
    {
        ResetTime();
    }

    #region 毎フレーム処理
    /// <summary>
    /// メイン処理
    /// </summary>
    public void SparkFieldUpdate()
    {
        // Missingになったオブジェクトがあれば削除する
        for (int i = 0; i < Target.Count; i++) {
            if (Target[i] == null) {
                Target.Remove(Target[i]);
            }
        }

        // 攻撃
        if (IsAttack) {
            Attack();
        }
        // クールタイム
        else {
            CoolDown();
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void Attack() {
        if (time < AttackTime) {
            Spark();
            AddTime();
        }
        else {
            IsAttack = !IsAttack;
            ResetTime();
            SparkEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    /// <summary>
    /// 電撃攻撃
    /// </summary>
    private void Spark() {
        foreach(GameObject target in Target) {
            target.GetComponent<HumanoidBase>().NowHP -= AttackPower;
        }
    }

    /// <summary>
    /// クールダウン処理
    /// </summary>
    private void CoolDown() {
        if (time < CoolTime) {
            AddTime();
        }
        else {
            IsAttack = !IsAttack;
            ResetTime();
            SparkEffect.GetComponent<ParticleSystem>().Play();
        }
    }

    /// <summary>
    /// タイム更新
    /// </summary>
    private void AddTime() {
        time += Time.deltaTime;
    }

    /// <summary>
    /// タイムリセット
    /// </summary>
    private void ResetTime() {
        time = 0.0f;
    }
    #endregion
    #region 当たり判定
    private void OnTriggerEnter(Collider other) {
        // ターゲットの追加処理
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
            foreach (GameObject target in Target) {
                if (target == other.gameObject) {
                    IsAdd = false;
                    break;
                }
            }

            // かぶっていなければ追加する
            if (IsAdd) {
                Target.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // ターゲット削除処理
        if (Target.Contains(other.gameObject)) {
            Target.Remove(other.gameObject);
        }
    }
    #endregion
}
