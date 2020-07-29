using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSeControl : MonoBehaviour
{
    [Header("SEチャンネル数")]
    public const int Channel = 5;
    [Header("SEチャンネル")]
    public List<AudioSource> SEChannel;
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
        SEChannel = new List<AudioSource>(3);//配列の確保  とりあえず３こ
        for (int cnt = 0; cnt < SEChannel.Count; cnt++) {
            if (SEChannel[cnt] == null) {
                SEChannel[cnt] = this.gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public int PlayLoopSe(string keyname,bool isloop)
    {
        int UseNumber = UseLoopNumber();
        if (!SEChannel[UseNumber].isPlaying)//再生中であれば再生関数を飛ばす
        {
            SEChannel[UseNumber].loop = isloop;
            SEChannel[UseNumber].clip = AudioManager.Instance.GetDictionalyClip(keyname);//指定したキー名のオーディオクリップをセット
            SEChannel[UseNumber].Play();//指定したクリップを再生
        }
        return UseNumber;
    }

    public void StopLoopSe(int cnannel)//ループしているSEを停止
    {
        if (SEChannel[cnannel].isPlaying)
        {
            SEChannel[cnannel].Stop();
        }
        SEChannel[cnannel].clip = null;
    }

    public void StopLoopSeAll()
    {
        foreach (AudioSource mysource in SEChannel)
        {
            if (mysource.isPlaying)
            {
                mysource.Stop();
            }
            mysource.clip = null;
        }
    }

    public bool GetisPlaying(int cnannel)//ループSEが再生中かを取得
    {
        return SEChannel[cnannel].isPlaying;
    }

    public void SetLoopSeVolume(float vol, int cnannel)//ループするSEの音量設定(0~1で設定されます)
    {
        SEChannel[cnannel].volume = Mathf.Clamp(vol, 0f, 1.0f);
    }

    public void SetLoopSeVolumeAll(float vol)//全てのループSEのボリュームをセット
    {
        //foreach(AudioSource mysource in SEChannel)
        //{
        //    mysource.volume= Mathf.Clamp(vol, 0f, 1.0f);
        //}
    }

    private int UseLoopNumber() {
        int returnNum = -1;
        for(int i = 0; i < SEChannel.Count; i++) {
            if(SEChannel[i].clip == null) {
                returnNum = i;
                return returnNum;
            }
        }

        // 空いているリストが見つからなかった時
        SEChannel.Add(gameObject.AddComponent<AudioSource>());
        returnNum = SEChannel.Count - 1;
        return returnNum;
    }
}
