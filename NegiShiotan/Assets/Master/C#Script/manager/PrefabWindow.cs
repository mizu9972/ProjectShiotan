using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabWindow : MonoBehaviour
{
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

        for(int listIter = 0;listIter < m_PW.prefabWindowsList.Count; listIter++)
        {
            ViewPrefabData(m_PW.prefabWindowsList[listIter]);
        }

        addPrefab();
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

    //保存済みプレハブ表示
    private void ViewPrefabData(PrefabWindow.PrefabData data)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(data.obj, typeof(GameObject), false);
        EditorGUILayout.TextField(data.text);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("生成"))
        {
            m_PW.InstantiatePrefabbyList(data.obj);
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