using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージクリアを判定し、リザルト画面を呼び出すライン用クラス
public class StageEndLine : MonoBehaviour
{
    //ゲームマネージャー
    private ClearManager m_ClearManager;

    //コンベアシステムオブジェクト
    private GameObject m_StageConveyor = null;

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

    //ステージエンドラインにPlayerPointが到達したらゲームマネージャーへ通知する
    private void EndLine()
    {
        m_ClearManager.ClearFunction();
    }
}
