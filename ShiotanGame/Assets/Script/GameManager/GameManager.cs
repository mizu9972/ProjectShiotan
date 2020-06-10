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

    [Header("フェード用パネル")]
    public GameObject FadePanel;

    private GameObject isStageObj;//ゲームメインかを確認するオブジェクト
    //HPとエサ引き継ぎ用
    private bool isCarryover = false;
    private float WorkFoods = 0f;
    private float WorkHp = 0f;
    private bool PauseEnable = false;
    private bool isTakeover = false;
    private bool workTakeover = false;
    private int WorkKey = 0;//引き継ぐ鍵

    private GameObject PlayerObj = null;

    private bool isFade;//フェード中か
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
        if (isStageObj == null)
        {
            isStageObj = GameObject.Find("isStage");
        }

        //ステージ中ならプレイヤーの取得
        if (isStageObj.GetComponent<isGameMain>().GetisGameMain())
        {
            if(PlayerObj==null)
            {
                PlayerObj = GameObject.FindGameObjectWithTag("Player");
            }
        }

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
        workTakeover = istakeover;
    }
    public void SceneReload()//シーン再読み込み
    {
        AudioManager.Instance.StopLoopSeAll();//ループSEを全て停止
        if(isStageObj.GetComponent<isGameMain>().GetisGameMain())//ゲームメイン中は円形フェード
        {
            MainCamera.GetComponent<FadebyTex>().StartFadeOut();
            this.UpdateAsObservable().
                Where(_ => !GetisFade()).Take(1).
                Subscribe(_ => SceneTransition(SceneManager.GetActiveScene().name));
        }
        else
        {
            MainCamera.GetComponent<SceneTransition>().SetTransitionRun(SceneManager.GetActiveScene().name);
        }
    }

    public void SetworkPosition(Vector3 pos)
    {
        work_Position = pos;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//シーンが読み込まれた時
    {
        PlayerObj = null;

        isStageObj = null;
        //シーンが読み込まれた時にカメラを取得
        MainCamera = Camera.main;

        if(isTakeover)//リスポーン地点引き継ぎ
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
        if(PlayerObj != null)
        {
            PlayerObj.GetComponent<Player>().SetPlayerMove(true);//プレイヤーの操作可能に
            PlayerObj.GetComponent<Player>().SetThrowFoodEnable(true);//プレイヤーのエサ投げ可能に
        }
    }
    public void PlayerControlStop()
    {
        if (PlayerObj != null)
        {
            PlayerObj.GetComponent<Player>().SetPlayerMove(false);//プレイヤーの操作不可能に
            PlayerObj.GetComponent<Player>().SetThrowFoodEnable(false);//プレイヤーのエサ投げ不可能に
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

    public void SetWorkStatus(float workho,float workfoods,int workkey)
    {
        WorkHp = workho;
        WorkFoods = workfoods;
        WorkKey = workkey;
    }

    private void InitPlayerStatus()//引き継ぎステータスのセット
    {
        PlayerObj.GetComponent<Player>().SetPlayerStatus(WorkHp, WorkFoods,WorkKey);
    }

    public GameObject GetPlayer()
    {
        if(!PlayerObj)//プレイヤーオブジェクトを取得
        {
            return null;
        }
        return PlayerObj;
    }

    public bool GetisStage()//現在がゲームメイン中であるかを返す
    {
        return isStageObj.GetComponent<isGameMain>().GetisGameMain();
    }

    public void SceneTransition(string SceneName)
    {
        AudioManager.Instance.StopLoopSeAll();//ループSEを全て停止
        SceneManager.LoadScene(SceneName);
    }

    public void SetisFade(bool isfade)
    {
        isFade = isfade;
    }
    public bool GetisFade()
    {
        return isFade;
    }

    public void SetPanelAlpha(float value)
    {
        FadePanel.GetComponent<FadeScript>().SetPanelAlpha(value);
    }

    public bool GetisTakeOver()//リスポーン地点引き継ぐかを取得
    {
        return workTakeover;
    }

    public void SetWorkTakeOver(bool istakeover)//チェックポイントを通ったら保存
    {
        workTakeover = istakeover;
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
