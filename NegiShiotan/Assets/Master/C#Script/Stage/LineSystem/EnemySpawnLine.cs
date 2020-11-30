using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EnemySpawnLine : MonoBehaviour
{
    [SerializeField, Header("出現させる敵オブジェクト")]
    private GameObject EnemyObject = null;

    [Header("ジャンプのためのコライダー")]
    private JumpScript JumpCollider;

    [Header("ジャンプする力")]
    public float JumpPower;

    [Header("ジャンプする高さ")]
    public float JumpHigh;

    private GameObject m_EnemySpawnErea = null;
    private EnemySpawnSystem m_ESS = null;

    [HideInInspector]
    public int EreaCount = 9;
    [HideInInspector]
    private const int EreaCountCst = 9;
    [/*HideInInspector,*/SerializeField]
    public bool[] Erea = new bool[EreaCountCst];

    // Start is called before the first frame update
    void Start()
    {
        m_EnemySpawnErea = GameObject.FindGameObjectWithTag("EnemySpawnErea");
        m_ESS = m_EnemySpawnErea.GetComponent<EnemySpawnSystem>();
        JumpCollider= GameObject.FindGameObjectWithTag("JumpArea").GetComponent<JumpScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            JumpCollider.SetJumpPower(JumpPower);
            JumpCollider.SetJumpRange(JumpHigh);
            SpawnEnemy();
        }
    }

    //指定したエリアに敵オブジェクトを生成する
    private void SpawnEnemy()
    {
        if (EnemyObject == null)
        {
            return;
        }

        //trueになっているエリアに生成
        //もし複数のエリアがtrueになっていたら複数体生成される
        for (int EreaNum = 0; EreaNum < EreaCountCst; EreaNum++)
        {
            if (Erea[EreaNum] == true)
            {
                Instantiate(EnemyObject, m_ESS.getEreaTrans(EreaNum).position, Quaternion.identity);
            }
        }
    }

}
#if UNITY_EDITOR
//エディター拡張
[CustomEditor(typeof(EnemySpawnLine))]
public class SpawnEreaSelecter : Editor
{
    EnemySpawnLine m_Erea;

    bool[] recordErea = new bool[9];
    
    private void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
 
        serializedObject.Update();
        base.OnInspectorGUI();


        m_Erea = (EnemySpawnLine)target;





        EditorGUI.BeginChangeCheck();


        //敵スポーン位置の設定項目

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("敵スポーン位置");
        {
            EditorGUILayout.BeginHorizontal();
            {
                recordErea[0] = EditorGUILayout.Toggle("", m_Erea.Erea[0]);
                recordErea[1] = EditorGUILayout.Toggle("", m_Erea.Erea[1]);
                recordErea[2] = EditorGUILayout.Toggle("", m_Erea.Erea[2]);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                recordErea[3] = EditorGUILayout.Toggle("", m_Erea.Erea[3]);
                recordErea[4] = EditorGUILayout.Toggle("", m_Erea.Erea[4]);
                recordErea[5] = EditorGUILayout.Toggle("", m_Erea.Erea[5]);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                recordErea[6] = EditorGUILayout.Toggle("", m_Erea.Erea[6]);
                recordErea[7] = EditorGUILayout.Toggle("", m_Erea.Erea[7]);
                recordErea[8] = EditorGUILayout.Toggle("", m_Erea.Erea[8]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        //変更されたら
        if (EditorGUI.EndChangeCheck())
        {
            //Undoに保存
            Undo.RecordObject(m_Erea, "ChangeErea");

            //反映
            for (int u = 0; u < 9; u++)
            {
                m_Erea.Erea[u] = recordErea[u];
            }

            EditorUtility.SetDirty(m_Erea);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }      

        serializedObject.ApplyModifiedProperties();

        
    }
  
    //一個だけ選べるようにする場合は下の処理を利用する

    ////トグルが変更されたかどうかを検知する
    //private bool ToggleCheck(bool Erea)
    //{
    //    EditorGUI.BeginChangeCheck();

    //    //表示
    //    Erea = EditorGUILayout.Toggle("", Erea);

    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        Erea = ToggleChange(Erea);
    //    }

    //    return Erea;
    //}

    ////トグルの内容を変更する
    //private bool ToggleChange(bool ChangeErea)
    //{
    //    if(ChangeErea == false)
    //    {
    //        return false;
    //    }

    //    //一旦全てfalseに
    //    for(int EreaNum = 0;EreaNum < m_Erea.EreaCount; EreaNum++)
    //    {
    //        m_Erea.Erea[EreaNum] = false;
    //    }

    //    return true;
    //}
}

#endif