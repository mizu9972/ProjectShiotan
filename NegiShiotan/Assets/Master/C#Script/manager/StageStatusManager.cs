using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StageStatus
{
    public enum Stages{
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10,
        Ending
    }
}

public class StageStatusManager : MonoBehaviour
{
    
    [HideInInspector]
    public static StageStatus.Stages m_NowStage;

    //各ステージの状態
    [HideInInspector]
    public Dictionary<StageStatus.Stages, bool> m_stageStatusDictionary = new Dictionary<StageStatus.Stages, bool>() {
        { StageStatus.Stages.Stage1, false },
        { StageStatus.Stages.Stage2, false },
        { StageStatus.Stages.Stage3, false },
        { StageStatus.Stages.Stage4, false },
        { StageStatus.Stages.Stage5, false },
        { StageStatus.Stages.Stage6, false },
        { StageStatus.Stages.Stage7, false },
        { StageStatus.Stages.Stage8, false },
        { StageStatus.Stages.Stage9, false },
        { StageStatus.Stages.Stage10, false },
        { StageStatus.Stages.Ending, false },
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //ステージクリア時
    public void clearedStage()
    {
        m_stageStatusDictionary[m_NowStage] = true;
    }

    //選択中ステージ保存
    public void setNowStage(StageStatus.Stages set)
    {
        m_NowStage = set;
    }

    //クリア情報取得
    public bool getStageStatus(StageStatus.Stages stage)
    {
        return m_stageStatusDictionary[stage];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageStatusManager))]
class StageStatusManagerEditor : Editor
{
    private StageStatusManager m_SSM;
    public override void OnInspectorGUI()
    {
        m_SSM = target as StageStatusManager;
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("ステージクリア状況");
        {
            ViewStageStatus("Stage1", StageStatus.Stages.Stage1);
            ViewStageStatus("Stage2", StageStatus.Stages.Stage2);
            ViewStageStatus("Stage3", StageStatus.Stages.Stage3);
            ViewStageStatus("Stage4", StageStatus.Stages.Stage4);
            ViewStageStatus("Stage5", StageStatus.Stages.Stage5);
            ViewStageStatus("Stage6", StageStatus.Stages.Stage6);
            ViewStageStatus("Stage7", StageStatus.Stages.Stage7);
            ViewStageStatus("Stage8", StageStatus.Stages.Stage8);
            ViewStageStatus("Stage9", StageStatus.Stages.Stage9);
            ViewStageStatus("Stage10", StageStatus.Stages.Stage10);
            ViewStageStatus("Ending", StageStatus.Stages.Ending);

        }
        EditorGUILayout.EndVertical();
    }

    //ステージクリア状況表示
    private void ViewStageStatus(string label,StageStatus.Stages stage)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(label);
        isClearView(m_SSM.m_stageStatusDictionary[stage]);
        EditorGUILayout.EndHorizontal();
    }

    //クリアしているか判定して表示を変える
    private void isClearView(bool isClear)
    {
        if (isClear)
        {
            EditorGUILayout.TextField("Clear");
        }else if(isClear == false)
        {
            EditorGUILayout.TextField("");
        }
    }
}
#endif