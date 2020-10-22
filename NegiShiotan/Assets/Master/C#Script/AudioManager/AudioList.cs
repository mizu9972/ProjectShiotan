using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    

    [Header("配列のサイズ")]
    public uint ArraySize;

    [Header("オーディオクリップリスト")]
    public AudioClip[] Audiolist;

    [Header("オーディオのキー名リスト")]
    public string[] KeyList;

    [Header("オーディオリスト")]
    public Dictionary<string, AudioClip> AudioDict = new Dictionary<string, AudioClip>();


    private void Start()
    {
        for (int cnt = 0; ArraySize > cnt; cnt++)//オーディオのリストにキー名とファイルを登録
        {
            if (KeyList[cnt] != null && Audiolist[cnt])
            {
                AudioDict[KeyList[cnt]] = Audiolist[cnt];
            }
        }
    }
    public void AudioListUpdate()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnValidate()//インスペクタ上の数値が変更されたら
    {
        //配列達の初期化
        //Audiolist = new AudioClip[ArraySize];
        //KeyList = new string[ArraySize];

        //AudioDict.Clear();//オーディオのリストを一旦消去

        for (int cnt = 0; ArraySize > cnt; cnt++)//オーディオのリストにキー名とファイルを登録
        {
            if(KeyList[cnt]!=null&&Audiolist[cnt])
            {
                AudioDict[KeyList[cnt]] = Audiolist[cnt];
            }
        }
    }

    public uint GetArraySize()
    {
        return ArraySize;
    }
}
