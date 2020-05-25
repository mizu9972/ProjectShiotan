using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSeControl : MonoBehaviour
{
    [Header("SEチャンネル数")]
    public const int Channel = 5;
    [Header("SEチャンネル")]
    public AudioSource[] SEChannel;
    // Start is called before the first frame update
    void Awake()
    {
        LoopSEInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoopSEInit()
    {
        SEChannel = new AudioSource[Channel];//配列の確保
        for(int cnt=0;cnt<=Channel;cnt++)
        {
            if(SEChannel[cnt]==null)
            {
                SEChannel[cnt]=this.gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void PlayLoopSe(string keyname,int cnannel,bool isloop)
    {
        if (!SEChannel[cnannel].isPlaying)//再生中であれば再生関数を飛ばす
        {
            SEChannel[cnannel].loop = isloop;
            SEChannel[cnannel].clip = AudioManager.Instance.GetDictionalyClip(keyname);//指定したキー名のオーディオクリップをセット
            SEChannel[cnannel].Play();//指定したクリップを再生
        }
    }

    public void StopLoopSe(int cnannel)//ループしているSEを停止
    {
        if (SEChannel[cnannel].isPlaying)
        {
            SEChannel[cnannel].Stop();
        }
        SEChannel[cnannel].clip = null;
    }

    public bool GetisPlaying(int cnannel)//ループSEが再生中かを取得
    {
        return SEChannel[cnannel].isPlaying;
    }

    public void SetLoopSeVolume(float vol, int cnannel)//ループするSEの音量設定(0~1で設定されます)
    {
        SEChannel[cnannel].volume = Mathf.Clamp(vol, 0f, 1.0f);
    }
}
