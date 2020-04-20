using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float AttackCoolDown = 1.0f;
    private float AttackTime = 0.0f;

    public void Attack(HumanoidBase Target) {
        AttackTime += Time.deltaTime;
        if(AttackTime > AttackCoolDown) {
            Target.NowHP -= gameObject.GetComponent<HumanoidBase>().NowAttackPower;
            AttackTime = 0.0f;
        }
    }
}
