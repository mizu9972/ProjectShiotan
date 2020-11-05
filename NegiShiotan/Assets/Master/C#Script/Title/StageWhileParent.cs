using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWhileParent : MonoBehaviour
{
    [SerializeField, Header("ステージ間ポイント(前進する順で)")]
    private List<GameObject> StageWhilePoints = new List<GameObject>();


    //移動する順番に並べたリスト
    private List<GameObject> AdvanceList = new List<GameObject>();//前進順
    private List<GameObject> RecessionList = new List<GameObject>();//後退順
    // Start is called before the first frame update
    void Start()
    {
        //移動順リストを設定
        {
            int addStageWhilePointIter = 0;
            int StageWhilePointNum = StageWhilePoints.Count;
            AdvanceList.Clear();
            for (addStageWhilePointIter = 0; addStageWhilePointIter < StageWhilePointNum; addStageWhilePointIter++)
            {
                AdvanceList.Add(StageWhilePoints[addStageWhilePointIter]);
            }

            RecessionList.Clear();
            for (addStageWhilePointIter = StageWhilePointNum - 1; addStageWhilePointIter >= 0; addStageWhilePointIter--)
            {
                RecessionList.Add(StageWhilePoints[addStageWhilePointIter]);
            }
        }
    }

    //移動順に整列したリストを返す
    public List<GameObject> getStageWhileList(StageSelectAction selectAction_)
    {
        switch (selectAction_)
        {
            case StageSelectAction.Next:
                return AdvanceList;

            case StageSelectAction.Prev:
                return RecessionList;

            default:
                return null;
        }
        
    }
    
}
