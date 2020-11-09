using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StageStarter : MonoBehaviour
{

    [SerializeField, Header("ステージ作成で最低限必要なオブジェクト群")]
    private List<GameObject> StageObjects = new List<GameObject>();

    private List<GameObject> m_PlacedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //オブジェクト配置
    public void ObjectSet()
    {
        Vector3 InitPosition = new Vector3(0, 0, 0);
        Quaternion InitRotate = new Quaternion(0, 0, 0, 0);

        DeletePreObject();
        foreach (GameObject setObject in StageObjects)
        {
            m_PlacedObjects.Add(Instantiate(setObject, setObject.transform.position, setObject.transform.rotation));

        }
    }

    //Scene作成時等で既に作成されているオブジェクトで不要なものを無効化する
    private void DeletePreObject()
    {
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
        }
        foreach(GameObject deleteObject in m_PlacedObjects)
        {
            m_PlacedObjects.Remove(deleteObject);
            Destroy(deleteObject);
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageStarter))]
public class StageStarterEditor : Editor
{
    StageStarter m_StageStarter;

    public void OnEnable()
    {
        m_StageStarter = target as StageStarter;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        base.OnInspectorGUI();



        serializedObject.Update();



        EditorGUILayout.EndVertical();
        if (GUILayout.Button("配置開始"))
        {
            m_StageStarter.ObjectSet();
        }

        serializedObject.ApplyModifiedProperties();

    }
}

#endif