using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
public class BgmSelect : MonoBehaviour
{
    public enum AudioType
    {
        NONE,
        BGM_GAMEMAIN
    };
    [Header("BGMタイプ")]
    public AudioType audioType;

    private string keyName = null;//再生するBGMのキー名

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable().Take(1).Subscribe(_ => BGMInit());
    }

    private void BGMInit()
    {
        keyName = audioType.ToString();//定義情報名を文字情報に変換
        Debug.Log("再生するBGMのキー名:" + keyName);

        if (audioType != AudioType.NONE)//そのシーンにBGMの割り当てがあれば再生
        {
            AudioManager.Instance.PlayMainBGM(keyName, true);
        }
        else//BGMの割り当てがなければBGM停止
        {
            AudioManager.Instance.StopBGM();
        }
    }
}
