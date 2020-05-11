using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("入力検知機能を使う")]
    public bool isDebugKey = true;

    private Vector3 work_Position;//シーン再読み込みする時のリスポーン地点の座標
    private Camera MainCamera;//アクティブシーンのメインカメラ
    [Header("ポーズ画面オブジェクト")]
    public GameObject PauseMenu;

    private GameObject isStageObj;//ゲームメインかを確認するオブジェクト
    //HPとエサ引き継ぎ用
    private bool isCarryover = false;
    private float WorkFoods = 0f;
    private float WorkHp = 0f;
    private bool PauseEnable = false;
    private bool isTakeover = false;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainCamera = Camera.main;
        isStageObj = GameObject.Find("isStage");
        PauseMenu.SetActive(false);//ポーズ画面のDontDestroyが最初から有効化されてないと呼ばれないので
    }

    // Update is called once per frame
    void Update()
    {
        

        if (PauseEnable)
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

    public void SceneReload(bool istakeover)//シーンを再読み込み(リスポーン地点を引き継ぐか)
    {
        work_Position = GameObject.Find("Respawn").transform.position;
        SceneReload();
        isTakeover = istakeover;
    }
    public void SceneReload()//シーン再読み込み
    {
        MainCamera.GetComponent<SceneTransition>().SetTransitionRun(SceneManager.GetActiveScene().name);
    }

    public void SetworkPosition(Vector3 pos)
    {
        work_Position = pos;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//シーンが読み込まれた時
    {
        //シーンが読み込まれた時にカメラを取得
        MainCamera = Camera.main;
        isStageObj = GameObject.Find("isStage");
        if(isTakeover)
        {
            isTakeover = false;
            GameObject.Find("Respawn").transform.position = work_Position;
        }
        if (isCarryover)//HPとエサを引き継ぐ場合
        {
            this.UpdateAsObservable().Take(1).Subscribe(_ => InitPlayerStatus());
            isCarryover = false;
        }
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

    public void SetCarryOver(bool iscarry)
    {
        isCarryover = iscarry;
    }

    public void SetWorkStatus(float workho,float workfoods)
    {
        WorkHp = workho;
        WorkFoods = workfoods;
    }

    private void InitPlayerStatus()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<Player>().SetPlayerStatus(WorkHp, WorkFoods);
    }

    public void Quit()//終了処理
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
