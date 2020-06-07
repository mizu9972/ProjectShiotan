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

    [Header("クリア用テキスト")]
    public ClearText clearText;

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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isPlaying = MyPlayer.isPlaying;//現在のビデオの再生状態を取得
    }

    private void VideoEndStartFunc()//ビデオが終了した後に呼ばれる
    {
        PlayClearBGM();
        StartDrawText();
        AddInput();
    }
    private void PlayClearBGM()
    {
        AudioManager.Instance.PlayMainBGM("BGM_CLEAR", true);
    }

    private void StartDrawText()
    {
        clearText.StartFade();
    }

    private void AddInput()//入力検知機能をLateUpdate関数に追加
    {
        this.LateUpdateAsObservable().
            Where(_ => Input.anyKeyDown).Take(1).
            Subscribe(_ => InputFunc());
    }

    private void InputFunc()
    {
        //入力検知で遷移を実行
        AudioManager.Instance.PlaySE("SE_ENTER");
        mainCamera.GetComponent<SceneTransition>().SetTransitionRun("TitleScene");
    }
}
