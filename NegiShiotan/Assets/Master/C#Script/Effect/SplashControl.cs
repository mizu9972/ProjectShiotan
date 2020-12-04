using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashControl : MonoBehaviour
{
    [Header("左側のしぶき")]
    public ParticleSystem m_LeftPart = null;

    [Header("右側のしぶき")]
    public ParticleSystem m_RightPart = null;

    [Header("ベースのサイズ")]
    public float m_SizeBase = 2.5f;

    [Header("最大サイズ")]
    public float m_SizeMax = 3.5f;

    [Header("最小サイズ")]
    public float m_SizeMin = 1f;

    [SerializeField, Header("左側のサイズ")]
    private float m_SizeLeft;

    public float SizeLeft
    {
        set { m_SizeLeft = value; }
    }

    [SerializeField, Header("右側のサイズ")]
    private float m_SizeRight;
    public float SizeRight
    {
        set { m_SizeRight = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        SetSize();
    }

    private void SetSize()
    {
        var LeftPartSize_ = m_LeftPart.main;
        LeftPartSize_.startSize = m_SizeLeft;

        var RightPartSize_ = m_RightPart.main;
        RightPartSize_.startSize = m_SizeRight;
    }

    private void Init()//パーティクルの大きさ初期化
    {
        m_SizeLeft = m_SizeBase;
        m_SizeRight = m_SizeBase;
    }
}
