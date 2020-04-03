using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBase : MonoBehaviour {

    [SerializeField] private float m_HP {
        get { return m_HP; }
        set { m_HP = value; }
    }
    [SerializeField] private float m_AttackPower {
        get { return m_AttackPower; }
        set { m_AttackPower = value; }
    }



    // Start is called before the first frame update
    void Start() {
        if (m_HP <= 0) {
            Debug.LogWarning(gameObject.name + "のHPが0になっています。　設定してください");
        }
    }

    /// <summary>
    /// HPが0になっているかをチェック
    /// </summary>
    /// <returns>死んでいたらtrueを返す</returns>
    public bool DeadCheck() {
        // 死んでいたらtrueを返す
        if (m_HP <= 0) {
            return true;
        }
        // 生きているときにfalseを返す
        return false;
    }
}
