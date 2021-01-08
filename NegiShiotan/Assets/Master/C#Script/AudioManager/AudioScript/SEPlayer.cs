using System.Collections;
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

    [Header("SEをループさせるか")]
    public bool isLoop = false;

    [Header("音量"), Range(0f, 1f)]
    public float m_Volume = 1f;

    [HideInInspector]
    [Range(0f, 5f)] public float m_DopplerLevel = 1f;


    private AudioSource m_audioSource = null;//自分のオーディオソース
    
    // Start is called before the first frame update
    void Awake()
    {
        m_audioSource = this.GetComponent<AudioSource>();
        SetUseDoppler();
        m_audioSource.volume = m_Volume;
    }

    private void Update()
    {
        m_audioSource.volume = m_Volume;//ボリュームをセット
    }

    public void PlaySound()//SEプレイ関数
    {
        if(isLoop)//ループ再生なら
        {
            m_audioSource.clip = m_SEData;//データセット
            m_audioSource.loop = true;
            m_audioSource.Play();//再生
        }
        else
        {
            m_audioSource.PlayOneShot(m_SEData);
        }
    }

    public void StopSound()
    {
        m_audioSource.Stop();//再生中のSEを停止
        m_audioSource.clip = null;//クリップデータを空に
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

    public void StopUse3DAudio()//立体音響を中止にする
    {
        m_audioSource.spatialBlend = 0f;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(SEPlayer))]
public class SEPlayerEditor : Editor
{
    SEPlayer m_SePlayer;

    private bool disabled;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        m_SePlayer = (SEPlayer)target;

        EditorGUI.BeginChangeCheck();

        if(m_SePlayer.m_UseDoppler)
        {
            EditorGUILayout.BeginToggleGroup("ドップラーレベル", true);
            m_SePlayer.m_DopplerLevel 
                = EditorGUILayout.FloatField("0～5", Mathf.Clamp(m_SePlayer.m_DopplerLevel, 0f, 5f));
            EditorGUILayout.EndToggleGroup();
            
        }
        

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_SePlayer, "ChangeSePlayer");
            EditorUtility.SetDirty(m_SePlayer);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif