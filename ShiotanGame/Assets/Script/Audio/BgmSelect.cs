using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSelect : MonoBehaviour
{
    public enum AudioType
    {
        NONE,
        BGM_GAMEMAIN
    };
    [Header("BGMタイプ")]
    public AudioType audioType = AudioType.NONE;

    private GameObject audioManager;//オーディオマネージャのオブジェクト

    private string keyName = null;//再生するBGMのキー名

    // Start is called before the first frame update
    void Start()
    {
        keyName = audioType.ToString();//定義情報名を文字情報に変換
        Debug.Log("再生するBGMのキー名:"+keyName);

        audioManager = GameObject.Find("AudioManager");
        
        if (audioType!=AudioType.NONE)//そのシーンにBGMの割り当てがあれば再生
        {
            audioManager.GetComponent<AudioManager>().PlayMainBGM(keyName, true);
        }
    }
}
