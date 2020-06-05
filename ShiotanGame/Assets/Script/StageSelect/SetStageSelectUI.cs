using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStageSelectUI : MonoBehaviour
{
    public StageSelect stageselect;
    private bool isEnd = false;//処理が終わった時用
    // Start is called before the first frame update
    void Awake()
    {
        stageselect.GetComponent<StageSelect>().SetisControll(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GetisFade() && !isEnd)
        {
            stageselect.GetComponent<StageSelect>().SetisControll(true);
            isEnd = true;
        }
    }
}
