using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEndLine : MonoBehaviour
{
    //コンベアシステムオブジェクト
    private GameObject StageConveyor = null;
    // Start is called before the first frame update
    void Start()
    {
        StageConveyor = GameObject.FindGameObjectWithTag("StageConveyor");
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        //PlayerPointと触れたら
        if(other.gameObject.layer == 11)
        {
            EndLine();
        }
    }

    //EndLineにPlayerPointが到達したらコンベアシステムへ通知する
    private void EndLine()
    {
        var EndLineSystem_ = StageConveyor.GetComponent<IStageConveyorSystem>();
        if(EndLineSystem_ != null)
        {
            StageConveyor.GetComponent<IStageConveyorSystem>().OnEndLineSystem();
        }
    }
}
