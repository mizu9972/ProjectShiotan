#pragma warning disable 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabWindow : MonoBehaviour
{
    [Header("フォルダのパス")]
    public string FolderPass = null;

    public class PrefabData
    {
        [HideInInspector]
        public GameObject obj = null;

        [HideInInspector]
        public string text = null;
    }

    [HideInInspector]
    public List<PrefabData> prefabWindowsList = new List<PrefabData>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addObject(PrefabData addData)
    {
        prefabWindowsList.Add(addData);
    }

    public void InstantiatePrefabbyList(GameObject obj)
    {
        Instantiate(obj, obj.transform.position, obj.transform.rotation);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PrefabWindow))]
class PrefabWindowEditor : Editor
{
    PrefabWindow m_PW = null;
    PrefabWindow.PrefabData addData = new PrefabWindow.PrefabData();
    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        base.OnInspectorGUI();

        m_PW = target as PrefabWindow;

        EditorGUI.BeginChangeCheck();

        //for(int listIter = 0;listIter < m_PW.prefabWindowsList.Count; listIter++)
        //{
        //    ViewPrefabData(m_PW.prefabWindowsList[listIter]);
        //}
        ViewPrefabData();

        //変更されたら
        if (EditorGUI.EndChangeCheck())
        {
            //Undoに保存
            Undo.RecordObject(m_PW, "PrefabWindow");

            EditorUtility.SetDirty(m_PW);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        serializedObject.ApplyModifiedProperties();

    }

    //フォルダー以下のプレハブ表示
    private void ViewPrefabData()
    {
        if(m_PW.FolderPass.Length == 0)
        {
            return;
        }
        string[] PrefabPasses = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new string[] { m_PW.FolderPass });

        for(int PrefabNumber = 0;PrefabNumber < PrefabPasses.Length; PrefabNumber++)
        {
            string prefabPass = AssetDatabase.GUIDToAssetPath(PrefabPasses[PrefabNumber]);
            var prefab_ = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPass);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.ObjectField(prefab_, typeof(GameObject), false);

            GenerateButton(prefab_);

            EditorGUILayout.EndVertical();
        }
    }

    //保存済みプレハブ表示
    private void ViewPrefabData(PrefabWindow.PrefabData data)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(data.obj, typeof(GameObject), false);
        EditorGUILayout.TextField(data.text);
        EditorGUILayout.EndHorizontal();

        GenerateButton(data.obj);
    }

    private void GenerateButton(GameObject obj)
    {
        if (GUILayout.Button("生成"))
        {
            m_PW.InstantiatePrefabbyList(obj);
        }
    }

    //プレハブ追加
    private void addPrefab()
    {

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("プレハブの追加");

        addData.obj = EditorGUILayout.ObjectField("追加するプレハブ", addData.obj, typeof(GameObject), true) as GameObject;
        EditorGUILayout.LabelField("備考"); 
        addData.text = EditorGUILayout.TextArea(addData.text);

        if (GUILayout.Button("追加"))
        {
            m_PW.addObject(addData);

            addData = new PrefabWindow.PrefabData();
        }

        EditorGUILayout.EndVertical();
    }

}
#endif