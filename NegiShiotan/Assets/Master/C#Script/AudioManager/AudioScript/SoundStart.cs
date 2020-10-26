using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStart : MonoBehaviour
{
    public BGMPlayer bgmPlayer;
    // Start is called before the first frame update
    void Start()
    {
        bgmPlayer.PlayBgm();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
