using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Header("BGMのデータ")]
    public AudioClip[] BgmData = new AudioClip[1];

    [SerializeField, Header("BGMナンバー(デフォルトは0)")]
    private int BgmNum = 0;

    [Header("オーディオソース")]
    public AudioSource audioSource = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(audioSource==null)//付け忘れの保険
        {
            audioSource = this.GetComponent<AudioSource>();
        }
    }

    public void PlayBgm()
    {
        audioSource.clip = BgmData[BgmNum];
        audioSource.Play();
    }

    public void ChangeBgm(int num)//Bgmの番号変更
    {
        BgmNum = num;
        audioSource.clip = BgmData[BgmNum];
    }
}
