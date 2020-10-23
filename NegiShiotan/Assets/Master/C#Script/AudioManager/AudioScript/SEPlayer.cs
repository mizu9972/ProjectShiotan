﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//AudioSourceがなければ追加
[RequireComponent(typeof(AudioSource))]
public class SEPlayer : MonoBehaviour
{
    [Header("再生するSEデータ")]
    public AudioClip m_SEData = null;

    [Header("ドップラー使用")]
    public bool m_UseDoppler = false;

    [Header("ドップラーレベル")]
    [Range(0f, 5f)] public float m_DopplerLevel = 1f;


    private AudioSource m_audioSource = null;//自分のオーディオソース
    
    // Start is called before the first frame update
    void Awake()
    {
        m_audioSource = this.GetComponent<AudioSource>();
        SetUseDoppler();
    }

    public void PlaySound()//SEプレイ関数
    {
        m_audioSource.PlayOneShot(m_SEData);
    }

    private void SetUseDoppler()//ドップラー使用切り替え
    {
        m_audioSource.spatialBlend = 1f;
        switch(m_UseDoppler)
        {
            case false:
                m_audioSource.dopplerLevel = 0f;
                break;

            case true:
                m_audioSource.dopplerLevel = m_DopplerLevel;
                break;
        }
    }
}