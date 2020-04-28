using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteWallBase : MonoBehaviour
{
    [SerializeField, Header("破壊エフェクト")]
    protected GameObject ParticleEffect = null;
    private ParticleEffectScript m_ParEffScr = null;

    private void Start()
    {

        //エフェクト生成
        m_ParEffScr = ParticleEffect.GetComponent<ParticleEffectScript>();

    }
    // Update is called once per frame
    void Update()
    {
        // HPが0以下になったら削除
        if (gameObject.GetComponent<HumanoidBase>().DeadCheck()) {

            Dead();
        }
    }

    [ContextMenu("Dead")]
    void Dead()
    {

        //エフェクト再生
        Instantiate(ParticleEffect, this.gameObject.transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
