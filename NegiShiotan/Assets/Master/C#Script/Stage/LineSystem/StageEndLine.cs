using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージクリアを判定し、リザルト画面を呼び出すライン用クラス
public class StageEndLine : MonoBehaviour
{
    //ゲームマネージャー
    private ClearManager m_ClearManager;

    //コンベアシステムオブジェクト
    //private GameObject m_StageConveyor = null;

    void Start()
    {
        m_ClearManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ClearManager>();
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            EndLine();
        }
    }

    //魚　削除
    private void OnTriggerStay(Collider other)
    {
        //クリアライン超えたとき
        if (other.gameObject.tag == "Piranha" || other.gameObject.tag == "Pillarc" || other.gameObject.tag == "RidePiranha"|| other.gameObject.tag == "RideFish")
        {
            Destroy(other.gameObject);
        }

        //プレイヤー（イカダ）　ゴールライン通過時
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<RaftMove>().SetGoal();
        }
    }

    //ステージエンドラインにPlayerPointが到達したらゲームマネージャーへ通知する
    private void EndLine()
    {
        m_ClearManager.ClearFunction();
    }
}
