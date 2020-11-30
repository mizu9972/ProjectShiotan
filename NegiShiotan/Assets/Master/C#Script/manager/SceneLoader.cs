﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

using UniRx;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Header("読み込むシーン名"),HideInInspector]
    public string SceneName = "";

    [SerializeField, HideInInspector]
    public int SceneIndex = 0;//選択されているシーンのビルド設定でのインデックス番号

    [SerializeField, Header("クリア時のシーン遷移までの時間")]
    private float turnSceneTime = 3.0f;

    private int mySceneIndex;
    

    //事前読み込み用
    private AsyncOperation m_AsyncOperation = null;
    private bool isPreLoading = false;

    private void Start()
    {
        mySceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    [ContextMenu("シーン読み込み")]
    public void LoadScene()
    {
        if (isPreLoading)
        {
            m_AsyncOperation.allowSceneActivation = true;
        }
        else if(isPreLoading == false)
        {
            //SceneManager.LoadScene(SceneName);
            SceneManager.LoadScene(SceneIndex);
        }
    }

    //現在のシーン再読み込み
    public void LoadMyScene()
    {
        SceneManager.LoadScene(mySceneIndex);
    }

    //シーン事前読み込み
    public void PreLoadScene()
    {
        m_AsyncOperation = SceneManager.LoadSceneAsync(SceneIndex);
        m_AsyncOperation.allowSceneActivation = false;
        isPreLoading = true;
    }

    //事前読み込みシーン破棄
    public void UnloadScene()
    {
        if(m_AsyncOperation != null)
        {
            Debug.Break();
            SceneManager.UnloadSceneAsync(SceneIndex);
        }
    }

    //クリア時のシーン遷移
    public void LoadSceneByStageClear()
    {
        //カメラをもとの位置へ戻す
        var clearManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ClearManager>();
        clearManager.moveToDefaultCamera();

        SceneManager.sceneLoaded += ClearFunction;

        Observable.Timer(System.TimeSpan.FromSeconds(turnSceneTime))
            .Subscribe(_ => SceneManager.LoadScene(SceneIndex));

        //クリアキャンバスを非表示にする
        var clearCanvas = GameObject.FindGameObjectWithTag("ClearCanvas");
        clearCanvas.SetActive(false);

        //プレイヤーを画面奥へ移動させる
        RaftMove playerRaft = GameObject.FindGameObjectWithTag("Player").GetComponent<RaftMove>();
        playerRaft.moveFar();

        //ホワイトアウトさせる
        var mainCamera = Camera.main.GetComponent<FadebyTex>();
        mainCamera.StartWhiteOut();
    }

    private void ClearFunction(Scene next, LoadSceneMode mode)
    {
        var stageStausManager = GameObject.FindGameObjectWithTag("StageSelectManager").GetComponent<StageStatusManager>();

        //クリア処理
        stageStausManager.clearedStage();

        //イベントから削除
        SceneManager.sceneLoaded -= ClearFunction;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderCustom : Editor
{
    private SceneLoader m_SceneLoader = null;
    private Vector2 scroll;
    private int m_SceneCount;//ビルド設定されているシーン数
    string[] sceneNames;
    private void Awake()
    {
        m_SceneLoader = target as SceneLoader;
        m_SceneCount = SceneManager.sceneCountInBuildSettings;

        sceneNames = new string[m_SceneCount];

        for (int SceneIndex = 0; SceneIndex < m_SceneCount; SceneIndex++)
        {
            //シーン名を配列に設定
            sceneNames[SceneIndex] = getSceneName(SceneUtility.GetScenePathByBuildIndex(SceneIndex));
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.LabelField("読み込むシーン");
        //シーン名でプルダウンメニューを作成 ビルド設定のシーンのインデックス番号を保存
        m_SceneLoader.SceneIndex = EditorGUILayout.Popup(m_SceneLoader.SceneIndex, sceneNames);

        EditorGUILayout.EndScrollView();


        //変更されたら
        if (EditorGUI.EndChangeCheck())
        {
            //Undoに保存
            Undo.RecordObject(m_SceneLoader, "SceneLoader");

            EditorUtility.SetDirty(m_SceneLoader);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        serializedObject.ApplyModifiedProperties();
    }


    //シーンのパスからシーンの名前を取得
    private static string getSceneName(string ScenePath)
    {
        var NameStart = ScenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var NameEnd = ScenePath.LastIndexOf(".", StringComparison.Ordinal);
        var NameLength = NameEnd - NameStart;

        return ScenePath.Substring(NameStart, NameLength);
    }
}
#endif