using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private Vector3 work_Position;//シーン再読み込みする時のリスポーン地点の座標
    private Camera MainCamera;//アクティブシーンのメインカメラ
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void PlayerControlStart()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(Player!=null)
        {
            Player.GetComponent<Player>().SetPlayerMove(true);//プレイヤーの操作可能に
        }
    }
}
