using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EnemySpawnLine : MonoBehaviour
{
    [SerializeField, Header("出現させる敵オブジェクト")]
    private GameObject EnemyObject = null;

    private GameObject m_EnemySpawnErea = null;


    [HideInInspector]
    public int EreaCount = 9;
    [HideInInspector]
    private const int EreaCountCst = 9;
    [HideInInspector]
    public bool[] Erea = new bool[EreaCountCst];

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

    private void SpawnEnemy()
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
        m_Erea.Erea[0] = ToggleCheck(m_Erea.Erea[0]);
        m_Erea.Erea[1] = ToggleCheck(m_Erea.Erea[1]);
        m_Erea.Erea[2] = ToggleCheck(m_Erea.Erea[2]);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_Erea.Erea[3] = ToggleCheck(m_Erea.Erea[3]);
        m_Erea.Erea[4] = ToggleCheck(m_Erea.Erea[4]);
        m_Erea.Erea[5] = ToggleCheck(m_Erea.Erea[5]);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_Erea.Erea[6] = ToggleCheck(m_Erea.Erea[6]);
        m_Erea.Erea[7] = ToggleCheck(m_Erea.Erea[7]);
        m_Erea.Erea[8] = ToggleCheck(m_Erea.Erea[8]);

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

        for(int EreaNum = 0;EreaNum < m_Erea.EreaCount; EreaNum++)
        {
            m_Erea.Erea[EreaNum] = false;
        }

        return true;
    }
}