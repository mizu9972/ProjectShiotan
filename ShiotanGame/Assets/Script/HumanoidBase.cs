using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBase : MonoBehaviour {
    [SerializeField, Header("初期のHP")] private float m_InitHP = 0;
    [SerializeField, Header("初期の攻撃力")] private float m_InitAttackPower = 0;

    [SerializeField, Header("現在のHP")] private float m_NowHP = 0;
    [SerializeField, Header("現在の攻撃力")] private float m_NowAttackPower = 0;

    [SerializeField, Header("現在攻撃しているオブジェクト")] private GameObject m_AttackObject = null;

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
    void Start() {
        if (m_InitHP <= 0) {
            Debug.LogWarning(gameObject.name + "のHPが0になっています。　設定してください");
        }
        m_NowHP = m_InitHP;
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
