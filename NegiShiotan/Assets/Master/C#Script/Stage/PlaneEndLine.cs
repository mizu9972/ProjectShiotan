using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEndLine : MonoBehaviour
{
    //コンベアシステムオブジェクト
    private GameObject m_StageConveyor = null;
    // Start is called before the first frame update
    void Start()
    {
        m_StageConveyor = GameObject.FindGameObjectWithTag("StageConveyor");
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
        var EndLineSystem_ = m_StageConveyor.GetComponent<IStageConveyorSystem>();
        if(EndLineSystem_ != null)
        {
            m_StageConveyor.GetComponent<IStageConveyorSystem>().OnEndLineSystem(this.gameObject.transform.root.gameObject);

        }
    }
}
