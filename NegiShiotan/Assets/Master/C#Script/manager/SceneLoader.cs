﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Header("読み込むシーン名"),HideInInspector]
    public string SceneName = "";

    [SerializeField, HideInInspector]
    public int SceneIndex = 0;//選択されているシーンのビルド設定でのインデックス番号

    [ContextMenu("シーン読み込み")]
    public void LoadScene()
    {
        //SceneManager.LoadScene(SceneName);
        SceneManager.LoadScene(SceneIndex);
    }
}

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
