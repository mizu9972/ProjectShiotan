using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarcBase : MonoBehaviour
{
    [SerializeField, Header("再攻撃までのクールタイム")]
    public float AttackCoolTime = 0.0f;

    void Start() {
    }

    void Update() {
        // ピラルクAIの処理を行う
        gameObject.GetComponent<AIPillarc>().AIUpdate();
    }
}
