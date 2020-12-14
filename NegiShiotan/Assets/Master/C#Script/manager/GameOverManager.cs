using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UniRx;

public class GameOverManager : MonoBehaviour
{
    [SerializeField, Header("HPでのゲームオーバーキャンバス")]
    private GameObject HPGameOverObj = null;
    [SerializeField, Header("残機でのゲームオーバーキャンバス")]
    private GameObject ZankiGameOverObj = null;

    [SerializeField, Header("復活演出開始までの時間")]
    private float RebornStartTime = 0.5f;

    [SerializeField, Header("復活演出中のゲームスピード")]
    private float GameSpeed_reborn = 0.001f;

    [SerializeField, Header("ゲームスピードが元に戻るまでの時間")]
    private float GameSpeedResetTime = 3.0f;

    [SerializeField, Header("ゲームオーバー演出開始までの時間")]
    private float DeadStartTime = 0.5f;

    

    [SerializeField, Header("ステータス管理マネージャー")]
    private Status StatusManager = null;
    private float DefaultGameSpeed;
    // Start is called before the first frame update
    void Start()
    {
        HPGameOverObj.SetActive(false);
        ZankiGameOverObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //HPがゼロになった時の処理
    public void HPGameOverFunction()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(RebornStartTime))
            .Subscribe(_ => RebornFunction());

        
    }

    //復活処理
    private void RebornFunction()
    {
        HPGameOverObj.SetActive(true);

        //ゲーム進行速度を変更
        DefaultGameSpeed = Time.timeScale;
        Time.timeScale = GameSpeed_reborn;

        //指定秒後にゲーム進行速度を元に戻す
        Observable.Timer(System.TimeSpan.FromSeconds(GameSpeedResetTime * GameSpeed_reborn))
            .Subscribe(_ =>
            {
                Time.timeScale = DefaultGameSpeed;
                StatusManager.ResetHP();
                HPGameOverObj.SetActive(false);
            });

        //復活アニメーション再生


    }

    public void ZankiGameOverFunction()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(DeadStartTime))
            .Subscribe(_ => DeadFunction());

    }

    private void DeadFunction()
    {
        Time.timeScale = 0;
        ZankiGameOverObj.SetActive(true);
    }
}