using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class SplashControl : MonoBehaviour
{
    private enum MoveState
    {
        UNMOVE,
        LEFT,
        RIGHT
    }

    [SerializeField, Header("アクティブ状態")]
    private bool m_isActive = true;

    public bool isActive
    {
        get { return m_isActive; }
        set { m_isActive = value; }
    }
    [Header("ワールド側のパーティクル")]
    public GetChildParticle m_WorldParticle = null;

    [Header("ローカル側のパーティクル")]
    public GetChildParticle m_LocalParticle = null;

    [Header("中心のパーティクル")]
    public ParticleSystem m_CenterPart = null;

    [Header("ベースのサイズ")]
    public float m_SizeBase = 2.5f;

    [Header("最大サイズ")]
    public float m_SizeMax = 3.5f;

    [Header("最小サイズ")]
    public float m_SizeMin = 1f;

    [SerializeField, Header("左側のサイズ")]
    private float m_SizeLeft;

    [SerializeField, Header("右側のサイズ")]
    private float m_SizeRight;

    [Header("使用するパーティクル")]
    public List<ParticleSystem> m_Particles = new List<ParticleSystem>();
    public float SizeLeft
    {
        set { m_SizeLeft = value; }
    }
    public float SizeRight
    {
        set { m_SizeRight = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Init();//サイズに初期値代入
    }

    // Update is called once per frame
    void Update()
    {
        DrawSwitch();
        SetSize();//サイズを更新
    }

    private void SetSize()//パーティクルサイズの更新
    {
        var LeftLocalPartSize_ = m_LocalParticle.m_LeftPart.main;
        LeftLocalPartSize_.startSize = m_SizeLeft;

        var RightLocalPartSize_ = m_LocalParticle.m_RightPart.main;
        RightLocalPartSize_.startSize = m_SizeRight;

        var LeftWorldPartSize_ = m_WorldParticle.m_LeftPart.main;
        LeftWorldPartSize_.startSize = m_SizeLeft;

        var RightWorldPartSize_ = m_WorldParticle.m_RightPart.main;
        RightWorldPartSize_.startSize = m_SizeRight;
    }

    private void Init()//パーティクルの大きさ初期化
    {
        m_SizeLeft = m_SizeBase;
        m_SizeRight = m_SizeBase;
    }

    private void DrawSwitch()
    {
        ParticleSystem.MainModule PartContainer;//パーティクルの入れ物用

        if(m_isActive)
        {
            for (int cnt = 0; cnt < m_Particles.Count; cnt++)
            {
                PartContainer = m_Particles[cnt].main;
                PartContainer.maxParticles = 1000;
            }
        }
        else
        {
            for (int cnt = 0; cnt < m_Particles.Count; cnt++)
            {
                PartContainer = m_Particles[cnt].main;
                PartContainer.maxParticles = 0;
            }
        }
    }

    public void LockParticleMove(int val)//trueなら左、falseなら右
    {
        //描画の状態をワールドの方とローカルの方で切り替える(止めるのはストップ)
        //パーティクルのメインモジュールをローカルで宣言
        var WorldLeftPart_ = m_WorldParticle.m_LeftPart.main;
        var WorldRightPart_ = m_WorldParticle.m_RightPart.main;

        var LocalLeftPart_ = m_LocalParticle.m_LeftPart.main;
        var LocalRightPart_ = m_LocalParticle.m_RightPart.main;

        //左固定する時は左のワールドストップしてローカルプレイ
        switch (val)
        {
            case (int)MoveState.LEFT:
                //左を固定(左がローカル、右がワールド)
                m_WorldParticle.m_LeftPart.Stop();
                m_LocalParticle.m_LeftPart.Play();
                m_WorldParticle.m_RightPart.Play();
                m_LocalParticle.m_RightPart.Stop();
                Debug.Log("左固定");
                //LeftPart_.simulationSpace = ParticleSystemSimulationSpace.Local; 
                //RightPart_.simulationSpace = ParticleSystemSimulationSpace.World;

                break;

            case (int)MoveState.RIGHT:
                //右を固定(左がワールド、右がローカル)
                m_WorldParticle.m_RightPart.Stop();
                m_LocalParticle.m_RightPart.Play();
                m_WorldParticle.m_LeftPart.Play();
                m_LocalParticle.m_LeftPart.Stop();
                Debug.Log("右固定");
                //LeftPart_.simulationSpace = ParticleSystemSimulationSpace.World;
                //RightPart_.simulationSpace = ParticleSystemSimulationSpace.Local;
                break;

            case (int)MoveState.UNMOVE:
                //デフォルトに戻す
                m_LocalParticle.m_LeftPart.Stop();//ローカルを全てストップ
                m_LocalParticle.m_RightPart.Stop();
                m_WorldParticle.m_LeftPart.Play();
                m_WorldParticle.m_RightPart.Play();
                Debug.Log("デフォルト");
                //LeftPart_.simulationSpace = ParticleSystemSimulationSpace.World;
                //RightPart_.simulationSpace = ParticleSystemSimulationSpace.World;
                break;
        }
    }
}
