using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AudioSourceがなければ追加
[RequireComponent(typeof(AudioSource))]
public class SEPlayer : MonoBehaviour
{
    [Header("再生するSEデータ")]
    public AudioClip SEData = null;

    private AudioSource audioSource = null;//自分のオーディオソース
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlaySound()//SEプレイ関数
    {
        audioSource.PlayOneShot(SEData);
    }
}
