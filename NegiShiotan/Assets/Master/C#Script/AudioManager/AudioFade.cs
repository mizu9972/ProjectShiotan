using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [Header("フェードアウトの時間")]
    public float FadeOutTime = 0.5f;

    private float ElapsedTime = 0.0f;
    private float Volume = 1f;
    public bool AudioFadeOut()
    {
        ElapsedTime += Time.deltaTime;//経過時間計測
        Volume= 1f - (ElapsedTime / FadeOutTime);//経過時間をもとにボリューム設定
        if (Volume <= 0f)
        {
            ElapsedTime = 0f;
            AudioManager.Instance.SetBgmVolume(Volume);
            AudioManager.Instance.StopBGM();
            Volume = 1f;
            return true;
        }
        AudioManager.Instance.SetBgmVolume(Volume);
        return false;
    }
}
