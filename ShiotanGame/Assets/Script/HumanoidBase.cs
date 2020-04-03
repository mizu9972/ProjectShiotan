using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBase : MonoBehaviour 
{
    public float m_HP = 0;
    public float m_AttackPower = 0;

    // Start is called before the first frame update
    void Start() {
        if (m_HP < 0) {
            Debug.LogWarning(gameObject.name + "のHPが0になっています。　設定してください");
        }
    }

    /// <summary>
    /// HPが0になっているかをチェック
    /// </summary>
    /// <returns>死んでいたらtrueを返す</returns>
    public bool DeadCheck() {
        // 死んでいたらtrueを返す
        if (m_HP < 0) {
            return true;
        }
        // 生きているときにfalseを返す
        return false;
    }
}
