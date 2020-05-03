using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("入力検知機能を使う")]
    public bool isDebugKey = true;

    private Vector3 work_Position;//シーン再読み込みする時のリスポーン地点の座標
    private Camera MainCamera;//アクティブシーンのメインカメラ
    [Header("ポーズ画面オブジェクト")]
    public GameObject PauseMenu;

    private GameObject isStageObj;//ゲームメインかを確認するオブジェクト

    private bool PauseEnable = false;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainCamera = Camera.main;
        isStageObj = GameObject.Find("isStage");
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseEnable)
        {
            if (Input.GetButtonDown("Pause"))
            {
                SetActivePause(true);
            }
        }
        
        if(isDebugKey)
        {
            DownKeyCheck();
        }
    }

    public void SceneReload(bool isTakeover)//シーンを再読み込み(リスポーン地点を引き継ぐか)
    {
        //TODO リスポーン地点の座標を取得
        SceneReload();
        //TODO work_Positionの座標をリスポーンオブジェクトに代入
    }
    public void SceneReload()//シーン再読み込み
    {
        MainCamera.GetComponent<SceneTransition>().SetTransitionRun(SceneManager.GetActiveScene().name);
    }

    public void SetworkPosition(Vector3 pos)
    {
        work_Position = pos;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//シーンが読み込まれた時にカメラを取得
    {
        MainCamera = Camera.main;
        isStageObj = GameObject.Find("isStage");
    }

    public void PlayerControlStart()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(Player!=null)
        {
            Player.GetComponent<Player>().SetPlayerMove(true);//プレイヤーの操作可能に
            Player.GetComponent<Player>().SetThrowFoodEnable(true);//プレイヤーのエサ投げ不可能に
        }
    }
    public void PlayerControlStop()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Player.GetComponent<Player>().SetPlayerMove(false);//プレイヤーの操作不可能に
            Player.GetComponent<Player>().SetThrowFoodEnable(false);//プレイヤーのエサ投げ不可能に
        }
    }

    public void SetActivePause(bool isActive)//ポーズ画面のアクティブ状態を変更
    {
        if(isStageObj.GetComponent<isGameMain>().GetisGameMain())
        {
            PauseMenu.SetActive(isActive);
        }
    }


    public void SetPauseEnable(bool enable)//ポーズ画面の受付をするか
    {
        PauseEnable = enable;
    }


    void DownKeyCheck()//入力検知関数
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    //処理を書く
                    Debug.Log(code);
                    break;
                }
            }
        }
    }
}
