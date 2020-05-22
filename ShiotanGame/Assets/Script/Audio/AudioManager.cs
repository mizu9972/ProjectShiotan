using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("BGM用のメインオーディオソース")]
    public AudioSource BGM_audioSource;

    [Header("サブBGM用のオーディオソース")]
    public AudioSource BGM_subaudioSource;

    [Header("SE用のオーディオソース")]
    public AudioSource SE_audioSource;

    [Header("ループSE用のオーディオソース")]
    public AudioSource SE_LoopSource;

    private float SE_LoopVol = 1.0f;//ループSE用のボリューム

    [SerializeField, Header("BGMの音量")]
    [Range(0, 1)] private float BgmVol = 1.0f;

    [SerializeField, Header("SEの音量")]
    [Range(0, 1)] private float SeVol = 1.0f;

    private Dictionary<string, AudioClip> ClipList ;

    private uint arraySize;//オーディオリストのサイズ

    [Header("サブBGM再生するか")]
    public bool isPlaySubBGM = false;
    [Header("サブBGMをBGMの再生時間に合わせるか")]
    public bool isSynchroTime;
    //private int Channel = 4;
    private void Start()
    {
        arraySize = this.GetComponent<AudioList>().GetArraySize();//要素数を取得

        //オーディオリストを取得
        ClipList = new Dictionary<string, AudioClip>(this.GetComponent<AudioList>().AudioDict);

        ////サブBGM再生処理確認用
        //this.UpdateAsObservable().
        //    Where(_ => isPlaySubBGM).Take(1).
        //    Subscribe(_=>PlaySubBGM("BGM_SUBBGM", true));
    }

    void Update()
    {
        BGM_audioSource.volume = BgmVol;//BGMの音量設定
        BGM_subaudioSource.volume = BgmVol;//サブBGMの音量設定(BGMと同じ)

        SE_audioSource.volume = SeVol;//SEの音量設定


        PlaySubBGM("BGM_SUBBGM", true);

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
        if(isPlaySubBGM&&!BGM_subaudioSource.isPlaying)
        {
            if(isSynchroTime)
            {
                BGM_subaudioSource.time = BGM_audioSource.time;
            }
            BGM_subaudioSource.loop = isLoop;//ループするかを設定
            BGM_subaudioSource.clip = ClipList[KeyName];//指定したキー名のオーディオクリップをセット
            BGM_subaudioSource.Play();//指定したクリップを再生
        }
        else if(!isPlaySubBGM)
        {
            BGM_subaudioSource.Stop();
        }
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

    public void PlayLoopSe(string KeyName,bool isloop)
    {
        if(!SE_LoopSource.isPlaying)//再生中であれば再生関数を飛ばす
        {
            SE_LoopSource.loop = isloop;
            SE_LoopSource.clip = ClipList[KeyName];//指定したキー名のオーディオクリップをセット
            SE_LoopSource.Play();//指定したクリップを再生
        }
    }

    public void StopLoopSe()//ループしているSEを停止
    {
        if(SE_LoopSource.isPlaying)
        {
            SE_LoopSource.Stop();
        }
        SE_LoopSource.clip = null;
    }

    public bool GetisPlaying()//ループSEが再生中かを取得
    {
        return SE_LoopSource.isPlaying;
    }

    public void SetLoopSeVolume(float vol)//ループするSEの音量設定(0~1で設定されます)
    {
        SE_LoopSource.volume=Mathf.Clamp(vol, 0f, 1.0f);
    }

    public void StopBGM()
    {
        BGM_audioSource.clip = null;
        BGM_subaudioSource.clip = null;
    }
}
