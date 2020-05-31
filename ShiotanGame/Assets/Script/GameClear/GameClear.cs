using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using UniRx;
using UniRx.Triggers;
public class GameClear : MonoBehaviour
{
    private VideoPlayer MyPlayer;
    [SerializeField]
    private bool isPlaying = true;
    // Start is called before the first frame update
    void Start()
    {
        MyPlayer = this.GetComponent<VideoPlayer>();
        //ビデオのプレイが終了したらBGMを流す
        this.LateUpdateAsObservable().
            Where(_ => !isPlaying).Take(1).
            Subscribe(_ => PlayClearBGM());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isPlaying = MyPlayer.isPlaying;//現在のビデオの再生状態を取得
    }

    private void PlayClearBGM()
    {
        AudioManager.Instance.PlayMainBGM("BGM_CLEAR", true);
    }
}
