using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ピラニアの性格enum
enum PiranhaType {
    Normal,
    Attack,
    Escape,
    MaxCount
}

enum PiranhaNormalType {
    Normal1,    // 魚群の中心点を軸にまわる動き
    Normal2,    // ジグザグ動き
    Normal3,    // 
    MaxCount
}

enum PiranhaAttackType {
    Attack1,    // 
    Attack2,    // 
    Attack3,    // 
    MaxCount
}

enum PiranhaEscapeType {
    Escape1,    // 
    Escape2,    // 
    Escape3,    // 
    MaxCount
}
#endregion

public class PiranhaBase : MonoBehaviour
{
    private PiranhaType NowType;    // 大まかな性格
    private int DetailType; // 性格詳細
    private delegate void PiranhaAction();
    PiranhaAction Action;
    private Rigidbody rb;

    [SerializeField, Header("ピラニア1匹の攻撃力")]
    public float AttackPower;

    [SerializeField, Header("攻撃タイミングの最大誤差(s)")]
    private float InstantAttackTimingCorrct = 0.0f;
    private float LastAttackTime = 0.0f;

    // Normal1
    [SerializeField] private float AroundFlockMinAngle;
    [SerializeField] private float AroundFlockMaxAngle;

    // Normal2
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float TurningTime;
    [SerializeField] private int TurnTiming;
    private float StartTime = 0.0f;
    private int TurnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        NowType = PiranhaType.Normal;       // 大まかな性格をノーマルにセット
        ChangeDetailType();     // 細かい性格の設定
        CheckAllType();             // 性格をログで確認
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update
    public void PiranhaUpdate()
    {
        // 各タイプの動きをさせる
        Action.Invoke();    // Delegateタイプ

        // Switchタイプ
        /*switch (NowType) {
            case PiranhaType.Normal:
                ActionNormal();
                break;

            case PiranhaType.Attack:
                ActionAttack();
                break;

            case PiranhaType.Escape:
                ActionEscape();
                break;

            default:
                Debug.LogWarning(this.GetType().Name + ".PiranhaUpdate() " + "にてピラニアの大まかな性格が指定値以外を取得しました　[取得した値:" + (int)NowType + "]");
                break;
        }*/
    }

    private void ChangeDetailType() 
    {
        switch (NowType) {
            case PiranhaType.Normal:
                DetailType = Random.Range(0, (int)PiranhaNormalType.MaxCount);
                ActionNormal();
                break;

            case PiranhaType.Attack:
                DetailType = Random.Range(0, (int)PiranhaAttackType.MaxCount);
                break;

            case PiranhaType.Escape:
                DetailType = Random.Range(0, (int)PiranhaEscapeType.MaxCount);
                break;

            default:
                Debug.LogWarning(this.GetType().Name + ".ChangeDetailType() " + "にてピラニアの大まかな性格が指定値以外を取得しました　[取得した値:"+ (int)NowType + "]");
                break;
        }
    }

#region ピラニアの行動タイプ Action〇〇 で統一　○○は各タイプ名
    private void ActionNormal() 
    {
        switch((PiranhaNormalType)DetailType) {
            case PiranhaNormalType.Normal1:
                Action = ActionNormal1;
                break;

            case PiranhaNormalType.Normal2:
                Action = ActionNormal2;
                break;

            case PiranhaNormalType.Normal3:
                Action = ActionNormal3;
                break;

            default:
                Debug.LogWarning(this.GetType().Name + "ActionNormal() 内で \"PiranhaNormalType\" が指定値以外を取得しました [Max: " + PiranhaNormalType.MaxCount + " 取得した値:" + DetailType + "]");
                break;
        }
    }

    private void ActionAttack() 
    {
        switch ((PiranhaAttackType)DetailType) {
            case PiranhaAttackType.Attack1:
                Action = ActionAttack1;
                break;

            case PiranhaAttackType.Attack2:
                Action = ActionAttack2;
                break;

            case PiranhaAttackType.Attack3:
                Action = ActionAttack3;
                break;

            default:
                Debug.LogWarning(this.GetType().Name + "ActionNormal() 内で \"PiranhaAttackType\" が指定値以外を取得しました [Max: " + PiranhaAttackType.MaxCount + " 取得した値:" + DetailType + "]");
                break;
        }
    }

    private void ActionEscape() 
    {
        switch ((PiranhaEscapeType)DetailType) {
            case PiranhaEscapeType.Escape1:
                Action = ActionEscape1;
                break;

            case PiranhaEscapeType.Escape2:
                Action = ActionEscape2;
                break;

            case PiranhaEscapeType.Escape3:
                Action = ActionEscape3;
                break;

            default:
                Debug.LogWarning(this.GetType().Name + "ActionNormal() 内で \"PiranhaEscapeType\" が指定値以外を取得しました [Max: " + PiranhaEscapeType.MaxCount + " 取得した値:" + DetailType + "]");
                break;
        }
    }

#region それぞれの動きのメソッドの初期化
#region ノーマル
    private void InitActionNormal2() 
    {
    }
#endregion
#endregion

#region それぞれの動きメソッド
#region ノーマル
    private void ActionNormal1() {
            transform.RotateAround(transform.parent.gameObject.transform.localPosition, transform.up, Random.Range(AroundFlockMinAngle, AroundFlockMaxAngle));
    }

    private void ActionNormal2() {
        rb.velocity = transform.forward * MoveSpeed;
    }

    private void ActionNormal3() {
        
    }
#endregion
#region アタック
    private void ActionAttack1() {

    }

    private void ActionAttack2() {

    }

    private void ActionAttack3() {

    }

#endregion
#region エスケープ
    private void ActionEscape1() {

    }
    private void ActionEscape2() {

    }
    private void ActionEscape3() {

    }
#endregion
#endregion

#endregion

#region ピラニアタイプ変更 Switch〇〇Type() で統一 〇〇は各タイプ名
    public void SwitchNormalType() 
    {
        NowType = PiranhaType.Normal;
        ChangeDetailType();
    }

    public void SwitchAttackType() 
    {
        NowType = PiranhaType.Attack;
        ChangeDetailType();
    }

    public void SwitchEscapeType() 
    {
        NowType = PiranhaType.Escape;
        ChangeDetailType();
    }
#endregion

#region デバッグ用確認ログ欄
    private void CheckAllType() 
    {
        Debug.Log("大まかな性格 :" + NowType.ToString() +" 細かい性格 :" + DetailType.ToString());
    }

    private void CheckDeteilType() 
    {
        Debug.Log("細かい性格 :" + DetailType.ToString());
    }

#endregion

    public void AttackTimingDecision() 
    {
        InstantAttackTimingCorrct = Random.Range(0.0f, InstantAttackTimingCorrct);
    }

    public void FirstAttackTiming() {
        if (LastAttackTime + transform.parent.GetComponent<FlockBase>().AttackCoolTime <= Time.time) {
            LastAttackTime = Time.time - transform.parent.GetComponent<FlockBase>().AttackCoolTime + InstantAttackTimingCorrct;
        }
    }

    public void SetPiranhaDirection(Transform Target) {
        gameObject.transform.LookAt(Target);
    }

    // ピラニアがY軸のみターゲットを向かせる
    public void SetPiranhaDirection(Vector3 Target) {
        gameObject.transform.LookAt(Target);
        gameObject.transform.localEulerAngles = new Vector3(0.0f, gameObject.transform.localEulerAngles.y, 0.0f);
    }

    public void ResetPiranhaDirection() {
        gameObject.transform.eulerAngles = Vector3.zero;
    }

    public void Attack() 
    {
        // 攻撃できるかをチェック
        if(LastAttackTime + transform.parent.GetComponent<FlockBase>().AttackCoolTime <= Time.time) {
            // 攻撃
            transform.parent.GetComponent<AIFlock>().TargetList[0].gameObject.GetComponent<HumanoidBase>().Damage(AttackPower);

            // 攻撃時間を更新
            LastAttackTime = Time.time;
        }
    }

    public void Attack(GameObject Target) 
    {
        // 攻撃できるかをチェック
        if (LastAttackTime + transform.parent.GetComponent<FlockBase>().AttackCoolTime <= Time.time) {
            // 攻撃
            Target.GetComponent<HumanoidBase>().Damage(AttackPower);
            AudioManager.Instance.PlaySE("SE_BITE");    // SE再生

            // 攻撃時間を更新
            LastAttackTime = Time.time;
        }
    }
}
