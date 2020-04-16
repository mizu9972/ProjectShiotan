using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM用のメインオーディオソース")]
    public AudioSource BGM_audioSource;

    [Header("サブBGM用のオーディオソース")]
    public AudioSource BGM_subaudioSource;

    [Header("SE用のオーディオソース")]
    public AudioSource SE_audioSource;

    private Dictionary<string, AudioClip> ClipList ;

    private uint arraySize;//オーディオリストのサイズ

    //private int Channel = 4;
    void Awake()
    {
        arraySize = this.GetComponent<AudioList>().GetArraySize();//要素数を取得

        //オーディオリストを取得
        ClipList = new Dictionary<string, AudioClip>(this.GetComponent<AudioList>().AudioDict);
    }

    void Update()
    {

    }

    //メインBGM再生関数
    public void PlayMainBGM(string KeyName,bool isLoop)//再生したい音源のキー名とループするかを引数で指定(trueでループ)
    {
        BGM_audioSource.loop = isLoop;//ループするかを設定
        BGM_audioSource.clip = ClipList[KeyName];//指定したキー名のオーディオクリップをセット
        int a = 0;
        BGM_audioSource.Play();//指定したクリップを再生
    }

    //サブBGM再生関数
    public void PlaySubBGM(string KeyName, bool isLoop)
    {
        BGM_subaudioSource.loop = isLoop;//ループするかを設定
        BGM_subaudioSource.clip = ClipList[KeyName];//指定したキー名のオーディオクリップをセット
        BGM_subaudioSource.Play();//指定したクリップを再生
    }

    //SE再生
    public void PlaySE(string KeyName)
    {
        SE_audioSource.PlayOneShot(ClipList[KeyName]);
    }

    //private void ClipListInit()
    //{
    //    for (int cnt = 0; arraySize > cnt; cnt++)//オーディオのリストにキー名とファイルを登録
    //    {
    //        ClipList.Add();
            
    //    }
    //}
}
