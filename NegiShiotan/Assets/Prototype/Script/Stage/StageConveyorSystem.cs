using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageConveyorSystem : MonoBehaviour,IStageConveyorSystem
{
    [SerializeField,Header("ステージオブジェクト群")]
    private List<GameObject> StagePlaneList;

    [Header("ステージの挙動についての設定")]
    [Tooltip("ステージの基本移動速度")]
    public float ScrollBaseSpeed = 1.0f;
    [Space(10)]

    private GameObject ActiveStageObject = null;
    private float NowScrollSpeed;//ステージ移動速度

    // Start is called before the first frame update
    void Start()
    {
        NowScrollSpeed = ScrollBaseSpeed;

        StageInit();
    }

    // Update is called once per frame
    void Update()
    {

        ActiveStageObject.transform.Translate(-1.0f * NowScrollSpeed, 0, 0);
    }

    //ステージ処理初期化
    void StageInit()
    {
        ActiveStageObject = StagePlaneList[0];
    }

    public void OnEndLineSystem()
    {
        Debug.Log("EndLine到達");
        Debug.Break();
    }
}
