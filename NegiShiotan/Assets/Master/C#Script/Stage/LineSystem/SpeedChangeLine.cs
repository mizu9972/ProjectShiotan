using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChangeLine : MonoBehaviour
{
    [SerializeField, Header("変化後の速度")]
    float ToSpeed = 0.0f;

    [SerializeField, Header("変化させる時間")]
    float Time = 0.0f;

    private StageConveyorSystem m_StageConveyorSystem = null;
    // Start is called before the first frame update
    void Start()
    {
        m_StageConveyorSystem = GameObject.FindGameObjectWithTag("StageConveyor").GetComponent<StageConveyorSystem>();
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        //PlayerPointと触れたら
        if (other.gameObject.layer == 11)
        {
            m_StageConveyorSystem.ChangeSpeedbyTime(ToSpeed, Time);
        }
    }
}
