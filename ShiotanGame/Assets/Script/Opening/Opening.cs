using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using UniRx;
using UniRx.Triggers;

public class Opening : MonoBehaviour
{
    private VideoPlayer MyPlayer;
    [SerializeField]
    private bool isPlaying = true;
    [Header("メインカメラ")]
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MyPlayer = this.GetComponent<VideoPlayer>();
        //ビデオのプレイが終了したらBGMを流してテキストの描画
        this.LateUpdateAsObservable().
            Where(_ => !isPlaying).Take(1).
            Subscribe(_ => VideoEndStartFunc());
        this.LateUpdateAsObservable().
            Where(_ => Input.GetButtonDown("Pause")&&!GameManager.Instance.GetisFade()).Take(1).
            Subscribe(_ => VideoEndStartFunc());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isPlaying = MyPlayer.isPlaying;//現在のビデオの再生状態を取得
    }
    private void VideoEndStartFunc()//ビデオが終了した後に呼ばれる
    {
        mainCamera.GetComponent<SceneTransition>().SetTransitionRun("TitleScene");
    }
}
