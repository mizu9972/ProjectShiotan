using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("BGM用のメインオーディオソース")]
    public AudioSource BGM_audioSource;

    [Header("サブBGM用のオーディオソース")]
    public AudioSource BGM_subaudioSource;

    [Header("SE用のオーディオソース")]
    public AudioSource SE_audioSource;

    [SerializeField, Header("BGMの音量")]
    [Range(0, 1)] private float BgmVol = 1.0f;

    [SerializeField, Header("SEの音量")]
    [Range(0, 1)] private float SeVol = 1.0f;

    private Dictionary<string, AudioClip> ClipList ;

    private uint arraySize;//オーディオリストのサイズ

    //private int Channel = 4;
    private void Start()
    {
        arraySize = this.GetComponent<AudioList>().GetArraySize();//要素数を取得

        //オーディオリストを取得
        ClipList = new Dictionary<string, AudioClip>(this.GetComponent<AudioList>().AudioDict);

    }

    void Update()
    {
        BGM_audioSource.volume = BgmVol;//BGMの音量設定
        BGM_subaudioSource.volume = BgmVol;//サブBGMの音量設定(BGMと同じ)

        SE_audioSource.volume = SeVol;//SEの音量設定
    }

    //メインBGM再生関数
    public void PlayMainBGM(string KeyName,bool isLoop)//再生したい音源のキー名とループするかを引数で指定(trueでループ)
    {
        BGM_audioSource.loop = isLoop;//ループするかを設定
        BGM_audioSource.clip = ClipList[KeyName];//指定したキー名のオーディオクリップをセット
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

    public void SetSeVolume(float vol)
    {
        SeVol = Mathf.Clamp(vol, 0f, 1.0f);//0~1の範囲で音量をセット
    }

    public void SetBgmVolume(float vol)
    {
        BgmVol= Mathf.Clamp(vol, 0f, 1.0f);//0~1の範囲で音量をセット
    }

    public float GetSeVolume()
    {
        return SeVol;
    }

    public float GetBgmVolume()
    {
        return BgmVol;
    }
    //private void ClipListInit()
    //{
    //    for (int cnt = 0; arraySize > cnt; cnt++)//オーディオのリストにキー名とファイルを登録
    //    {
    //        ClipList.Add();
            
    //    }
    //}

    public void StopBGM()
    {
        BGM_audioSource.clip = null;
        BGM_subaudioSource.clip = null;
    }
}
