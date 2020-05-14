using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

[RequireComponent(typeof(Rigidbody))]

//移動時のみ発生させるエフェクトの制御
public class MoveEffect : MonoBehaviour
{
    
    [SerializeField, Header("エフェクト")]
    private GameObject Effect = null;
    private ParticleEffectScript m_ParEffScr = null;

    [SerializeField, Header("移動判定速度")]
    private float ActiveMoveSpeed;

    private Rigidbody m_RigidBody = null;
    private ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(false);
    // Start is called before the first frame update
    void Start()
    {
        m_ParEffScr = Effect.GetComponent<ParticleEffectScript>();
        m_RigidBody = this.gameObject.GetComponent<Rigidbody>();
        if (m_ParEffScr == null)
        {
            this.GetComponent<MoveEffect>().enabled = false;
        }

        //移動時のみパーティクルエフェクトを発生
        isMoving
            .DistinctUntilChanged()
            .Subscribe(x =>
        {
            if (x == true)
            {
                m_ParEffScr.StartEffect();
            }
            else if(x == false)
            {
                m_ParEffScr.StopEffect();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        //移動判定
        if(m_RigidBody.velocity.magnitude >= ActiveMoveSpeed)
        {
            isMoving.Value = true;
        }
        else
        {
            isMoving.Value = false;
        }
    }
}
