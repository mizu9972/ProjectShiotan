using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftSplashControl : MonoBehaviour
{
    //プレイヤーの移動状態
    private enum MoveState
    {
        UNMOVE,
        LEFT,
        RIGHT
    }
    private MoveState m_MoveState = MoveState.UNMOVE;

    private float m_RaftSpeed = 0f;//イカダの移動速度

    [Header("イカダの移動スクリプト")]
    public RaftMove m_RaftMove = null;

    
    public ParticleEffectScript m_partScript = null;
    public SplashControl m_SplashCtr = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        CheckMoveState();
    }

    private void CheckMoveState()
    {
        //イカダの速度を取得
        m_RaftSpeed = m_RaftMove.RaftSpead;
        //速度がプラスなら左移動、マイナスなら右移動,0なら停止のステートを代入
        if (m_RaftSpeed < 0f)
        {
            m_MoveState = MoveState.LEFT;
        }
        else if(m_RaftSpeed > 0f)
        {
            m_MoveState = MoveState.RIGHT;
        }
        else
        {
            m_MoveState = MoveState.UNMOVE;
        }

        //ステート変数の状態でパーティクルの挙動を制御
        ParticleControl();
    }

    private void ParticleControl()
    {
        switch(m_MoveState)
        {
            case MoveState.LEFT:
                m_SplashCtr.SizeLeft = m_SplashCtr.m_SizeMax;
                m_SplashCtr.SizeRight = m_SplashCtr.m_SizeMax;
                //左をローカルに切り替え
                break;

            case MoveState.RIGHT:
                m_SplashCtr.SizeLeft = m_SplashCtr.m_SizeMax;
                m_SplashCtr.SizeRight = m_SplashCtr.m_SizeMax;
                //右をローカルに切り替え
                break;

            case MoveState.UNMOVE:
                //デフォルトに戻す
                m_SplashCtr.SizeLeft = m_SplashCtr.m_SizeBase;
                m_SplashCtr.SizeRight = m_SplashCtr.m_SizeBase;
                break;
        }
        //状態によってパーティクルの挙動を制御
        m_SplashCtr.LockParticleMove((int)m_MoveState);
    }
}
