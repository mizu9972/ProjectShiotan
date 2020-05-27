using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UniRx;
using UniRx.Triggers;

public class HumanoidBase : MonoBehaviour {
    [SerializeField, Header("初期のHP")] private float m_InitHP = 0;
    [SerializeField, Header("初期の攻撃力")] private float m_InitAttackPower = 0;

    [SerializeField, Header("現在のHP")] private float m_NowHP = 0;
    [SerializeField, Header("現在の攻撃力")] private float m_NowAttackPower = 0;

    [SerializeField, Header("現在攻撃しているオブジェクト")] private GameObject m_AttackObject = null;

    [SerializeField, Header("ダメージ時のエフェクト")] private GameObject m_DamageEffect = null;
    private ParticleEffectScript m_ParEffScr = new ParticleEffectScript();

    [SerializeField, Header("ダメージエフェクトの発生最低間隔")]
    private float DamageEffInterval = 1.0f;
    private float m_LastDamageTime = 0.0f;//最後にダメージエフェクトを発生させた時間

    [SerializeField, Header("ダメージを受けた時の加算色")]
    private Color DamageAddColor = new Color(1, 1, 1, 1);

    private Material m_myMat;

    #region Getter/Setter
    public float InitHP {
        get { return m_InitHP; }
        set { m_InitHP = value; }
    }

    public float InitAttackPower {
        get { return m_InitAttackPower; }
        set { m_InitAttackPower = value; }
    }

    public float NowHP {
        get { return m_NowHP; }
        set { m_NowHP = value; }
    }

    public float NowAttackPower {
        get { return m_NowAttackPower; }
        set { m_NowAttackPower = value; }
    }

    public GameObject AttackObject {
        get { return m_AttackObject; }
        set { m_AttackObject = value; }
    }
    #endregion

    // Start is called before the first frame update
    void Awake() {
        if (m_InitHP <= 0) {
            Debug.LogWarning(gameObject.name + "のHPが0になっています。　設定してください");
        }
        m_NowHP = m_InitHP;

        m_NowAttackPower = m_InitAttackPower;

        m_LastDamageTime = -1.0f * DamageEffInterval;
    }

    private void Start()
    {
        m_myMat = gameObject.GetComponent<Renderer>().material;

        if (m_DamageEffect != null)
        {
            var ParticleSystem_ = Instantiate(m_DamageEffect, this.gameObject.transform.position, Quaternion.identity);
            ParticleSystem_.transform.parent = this.transform;
            m_ParEffScr = ParticleSystem_.GetComponent<ParticleEffectScript>();
        }
    }

    //被弾
    public void Damage(float AttackPower)
    {
        m_NowHP -= AttackPower;

        if (m_ParEffScr != null)
        {
            if (Time.time - m_LastDamageTime > DamageEffInterval)
            {
                m_ParEffScr.StartEffect();

                m_LastDamageTime = Time.time;

                //ダメージ時に色を変える処理
                m_myMat.SetColor("_Color", DamageAddColor);
                //元に戻す
                Observable.Timer(System.TimeSpan.FromSeconds(DamageEffInterval / 4.0f)).Subscribe(
                    _ => m_myMat.SetColor("_Color", new Color(1, 1, 1, 1))
                    );
            }
        }        
    }

    /// <summary>
    /// HPが0になっているかをチェック
    /// </summary>
    /// <returns>死んでいたらtrueを返す</returns>
    public bool DeadCheck() {
        // 死んでいたらtrueを返す
        if (m_NowHP <= 0) {
            return true;
        }
        // 生きているときにfalseを返す
        return false;
    }
}
