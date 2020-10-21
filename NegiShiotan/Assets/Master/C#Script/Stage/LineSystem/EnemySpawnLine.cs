using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EnemySpawnLine : MonoBehaviour
{
    [SerializeField, Header("出現させる敵オブジェクト")]
    private GameObject EnemyObject = null;

    private GameObject m_EnemySpawnErea = null;
    //public List<bool> Erea;

    [HideInInspector]
    public bool E_1, E_2, E_3, E_4, E_5, E_6, E_7, E_8, E_9;

    // Start is called before the first frame update
    void Start()
    {
        m_EnemySpawnErea = GameObject.FindGameObjectWithTag("EnemySpawnErea");

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {

        }
    }

    private void SpawnLine()
    {
        if(EnemyObject != null)
        {

        }
    }
}

//エディター拡張
[CustomEditor(typeof(EnemySpawnLine))]
public class SpawnEreaSelecter : Editor
{
    EnemySpawnLine m_Erea;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Erea = target as EnemySpawnLine;

        //敵スポーン位置の設定項目
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("敵スポーン位置");
        EditorGUILayout.BeginHorizontal();
        m_Erea.E_1 = ToggleCheck(m_Erea.E_1);
        m_Erea.E_2 = ToggleCheck(m_Erea.E_2);
        m_Erea.E_3 = ToggleCheck(m_Erea.E_3);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_Erea.E_4 = ToggleCheck(m_Erea.E_4);
        m_Erea.E_5 = ToggleCheck(m_Erea.E_5);
        m_Erea.E_6 = ToggleCheck(m_Erea.E_6);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_Erea.E_7 = ToggleCheck(m_Erea.E_7);
        m_Erea.E_8 = ToggleCheck(m_Erea.E_8);
        m_Erea.E_9 = ToggleCheck(m_Erea.E_9);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    //トグルが変更されたかどうかを検知する
    private bool ToggleCheck(bool Erea)
    {
        EditorGUI.BeginChangeCheck();

        Erea = EditorGUILayout.Toggle("", Erea);

        if (EditorGUI.EndChangeCheck())
        {
            Erea = ToggleChange(Erea);
        }

        return Erea;
    }

    //トグルの内容を変更する
    private bool ToggleChange(bool ChangeErea)
    {
        if(ChangeErea == false)
        {
            return false;
        }

        m_Erea.E_1 = false;
        m_Erea.E_2 = false;
        m_Erea.E_3 = false;
        m_Erea.E_4 = false;
        m_Erea.E_5 = false;
        m_Erea.E_6 = false;
        m_Erea.E_7 = false;
        m_Erea.E_8 = false;
        m_Erea.E_9 = false;

        return true;
    }
}