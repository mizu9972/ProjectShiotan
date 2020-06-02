using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int m_DropFood = 0;
    [SerializeField] private int m_DropKey = 0;
    [SerializeField] private float AttackCoolDown = 1.0f;
    [SerializeField] private GameObject KeyUI;
    private float AttackTime = 0.0f;

    private void Start() {
        ShowKey();
    }

    // 攻撃
    public void Attack(HumanoidBase Target,string SE_KEY) {
        AttackTime += Time.deltaTime;
        if(AttackTime > AttackCoolDown) {
            Target.Damage(gameObject.GetComponent<HumanoidBase>().NowAttackPower);
            AudioManager.Instance.PlaySE("SE_BITE");    // SE再生
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

    // 鍵の表示
    private void ShowKey() {
        if(m_DropKey > 0) {
            KeyUI.SetActive(true);
            return;
        }
        KeyUI.SetActive(false);
    }
}
