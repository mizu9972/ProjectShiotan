using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBase : MonoBehaviour {

    [SerializeField, Header("ピラニアオブジェクト")]
    private GameObject Piranha;      // ピラニアオブジェクトを入れるもの（なくてもできるけど一応配置してる）

    [SerializeField, Header("ピラニア生成数")]
    private int PiranhaCount = 0;   // プランナーがレベルデザインやりやすい用

    [HideInInspector] public List<GameObject> ChildPiranha;   // スクリプトで管理しないと、子の検索をかけた時に全ての子を返してくれないことがあったのでこういう設計にしています。

    [SerializeField, Header("ピラニア生成の生み出す誤差")]
    private Vector3 InstantPositionCorrct;     // ピラニア生成の座標の誤差をどこまで設定しますか?
    [SerializeField, Header("ピラニアのY軸の位置調整")]
    private float PiranhaYCorrection;

    [SerializeField, Header("再攻撃までのクールタイム")]
    public float AttackCoolTime = 0.0f;

    private AttackField ThisAttackField;

    private bool IsChese = true;

    private void Awake()
    {
        CreatePiranha();    // 子を作成し、初期化する
        gameObject.GetComponent<PiranhaAnimation>().InitPiranhaAnimation(ChildPiranha); // アニメーションモデルをセット
    }

    void Start() 
    {
        ThisAttackField = gameObject.transform.Find("AttackField").gameObject.GetComponent<AttackField>();
    }

    void Update() 
    {
        // 群衆AIの処理を行う
        if (IsChese) {
            gameObject.GetComponent<AIFlock>().AIUpdate();
        }
        else {
            ThisAttackField.RemoveBattle();
            gameObject.GetComponent<AIFlock>().CompulsionReturnPosition();
            IsChese = gameObject.GetComponent<AIFlock>().ReturnHomeCompleted();
        }

        // 各ピラニアの動き
        if (ChildPiranha.Count > 0) {
            // 子のすべての動きをここで再現させる
            foreach (GameObject obj in ChildPiranha) {
                // とりあえずピラニアのアップデートを掛ける
                obj.GetComponent<PiranhaBase>().PiranhaUpdate();

                // ToDo::追いかけ状態かそうでないかで動きが変わるかも？
            }
        }
        else {
            Debug.LogWarning(gameObject.transform.name + "にはピラニアが1匹もいません。");
        }
    }

    private void CreatePiranha() 
    {
        // ピラニアカウント数分ピラニアを生成する
        for (int i = 0; i < PiranhaCount; i++) {
            Vector3 CreatePos = new Vector3(Random.Range(-InstantPositionCorrct.x, InstantPositionCorrct.x), Random.Range(-InstantPositionCorrct.y, InstantPositionCorrct.y) + PiranhaYCorrection * 2.0f, Random.Range(-InstantPositionCorrct.z, InstantPositionCorrct.z));
            GameObject newObj = Instantiate(Piranha, gameObject.transform.position + CreatePos, Quaternion.identity, gameObject.transform);

            // ピラニアをリストに追加
            ChildPiranha.Add(newObj);

            // ピラニアのスクリプト等初期化 ToDo::ピラニアの動き方等もここで設定したい
            ChildInit(newObj);
        }
    }

    private void ChildInit(GameObject obj) 
    {
        // 子にそれぞれの攻撃力を持たせる
        obj.GetComponent<PiranhaBase>().AttackPower = gameObject.GetComponent<HumanoidBase>().NowAttackPower / PiranhaCount;

        // 攻撃の初期遅延タイミングを設定
        obj.GetComponent<PiranhaBase>().AttackTimingDecision();
    }

    /// <summary>
    /// 強制散会させるやつ
    /// </summary>
    public void ForcedMeeting() {
        IsChese = false;
        // ToDo::BattleFieldに所属しているときに、脱退処理
    }
}
