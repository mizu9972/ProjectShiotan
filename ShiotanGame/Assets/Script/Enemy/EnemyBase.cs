using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int m_DropFood = 0;
    [SerializeField] private int m_DropKey = 0;
    [SerializeField] private float AttackCoolDown = 1.0f;
    private float AttackTime = 0.0f;
    
    // 攻撃
    public void Attack(HumanoidBase Target) {
        AttackTime += Time.deltaTime;
        if(AttackTime > AttackCoolDown) {
            Target.Damage(gameObject.GetComponent<HumanoidBase>().NowAttackPower);
            AttackTime = 0.0f;
        }
    }

    // アイテムﾄﾞﾛｯﾌﾟ
    public int DropFood() {
        return m_DropFood;
    }

    // 鍵ﾄﾞﾛｯﾌﾟ
    public int DropKey() {
        return m_DropKey;
    }
}
