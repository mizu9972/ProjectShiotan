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
    public GameObject SE_LoopSource;

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

    private AudioFade audioFade;
    private bool isFadeOut = false;
    //private int Channel = 4;
    private void Start()
    {
        audioFade = this.GetComponent<AudioFade>();
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
        if(isFadeOut)//BGMのフェードがあれば実行
        {
            if(audioFade.AudioFadeOut())
            {
                FadeEndFunc();
            }
        }

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

    public int PlayLoopSe(string KeyName,bool isloop)
    {
        return SE_LoopSource.GetComponent<LoopSeControl>().PlayLoopSe(KeyName, isloop);
    }

    public void StopLoopSe(int channel)//ループしているSEを停止
    {
        SE_LoopSource.GetComponent<LoopSeControl>().StopLoopSe(channel);
    }

    public void StopLoopSeAll()
    {
        SE_LoopSource.GetComponent<LoopSeControl>().StopLoopSeAll();
    }

    public bool GetisPlaying(int channel)//ループSEが再生中かを取得
    {
        return SE_LoopSource.GetComponent<LoopSeControl>().GetisPlaying(channel);
    }

    public void SetLoopSeVolume(float vol,int channel)//ループするSEの音量設定(0~1で設定されます)
    {
        SE_LoopSource.GetComponent<LoopSeControl>().SetLoopSeVolume(vol,channel);
    }

    public void SetLoopSeVolumeAll(float vol)
    {
        SE_LoopSource.GetComponent<LoopSeControl>().SetLoopSeVolumeAll(vol);
    }

    public void StopBGM()
    {
        BGM_audioSource.clip = null;
        BGM_subaudioSource.clip = null;
    }

    public AudioClip GetDictionalyClip(string keyname)//オーディオリストの取得
    {
        return ClipList[keyname];
    }

    public void AudioFadeOutStart()//オーディオのフェードアウト開始
    {
        isFadeOut = true;
    }

    public void FadeEndFunc()//フェードが終了した後に通る
    {
        isFadeOut = false;
        BgmVol = 1.0f;
    }
}
