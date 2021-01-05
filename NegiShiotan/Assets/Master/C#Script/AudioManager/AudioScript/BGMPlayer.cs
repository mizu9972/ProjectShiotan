using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Header("BGMのデータ")]
    public AudioClip[] m_BgmData = new AudioClip[1];

    [SerializeField, Header("BGMナンバー(デフォルトは0)")]
    private int m_BgmNum = 0;

    [Header("オーディオソース")]
    public AudioSource m_audioSource = null;

    [Header("音量"), Range(0f, 1f)]
    public float m_Volume = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        if(m_audioSource==null)//付け忘れの保険
        {
            m_audioSource = this.GetComponent<AudioSource>();
        }
        //m_audioSource.spatialBlend = 0f;//立体音響不使用 //滝で立体音響使用するのでコメントアウトしました a.m
        m_audioSource.volume = m_Volume;
    }

    private void Update()
    {
        m_audioSource.volume = m_Volume;//ボリュームをセット
    }

    public void PlayBgm()
    {
        m_audioSource.clip = m_BgmData[m_BgmNum];
        m_audioSource.Play();
    }

    public void ChangeBgm(int num)//Bgmの番号変更
    {
        m_BgmNum = num;
        m_audioSource.clip = m_BgmData[m_BgmNum];
    }
}
