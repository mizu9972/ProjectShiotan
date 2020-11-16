using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSquare : MonoBehaviour
{
    [SerializeField, Header("ステージの種類")]
    private StageStatus.Stages thisStage = StageStatus.Stages.Stage1;

    [SerializeField, Header("ステージ選択管理オブジェクト")]
    private StageStatusManager stageSelectManager = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //選択中ステージ反映
    public void setStagetype()
    {
        stageSelectManager.setNowStage(thisStage);
    }

    StageStatus.Stages getStageType()
    {
        return thisStage;
    }
}
