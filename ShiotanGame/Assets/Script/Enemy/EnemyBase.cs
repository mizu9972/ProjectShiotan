using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int DropFood = 0;
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
    public int Drop() {
        return DropFood;
    }
}
