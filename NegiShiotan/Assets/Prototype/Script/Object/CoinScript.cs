using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class CoinScript : MonoBehaviour
{
    [Header("コイン増える数")]
    public int UpCoin;

    //コイン数管理マネージャー
    private Status CoinStatus;

    [Header("パーティクル再生スクリプト")]
    public PlayParticle m_playPart = null;

    [Header("コインのモデル")]
    public MeshRenderer m_Model = null;
    // Start is called before the first frame update
    void Start()
    {
        CoinStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Human"|| other.tag == "Bullet")
        {
            m_playPart.Play();//Effectを再生
            CoinStatus.UpCoin(UpCoin);
            m_Model.enabled = false;//モデルの描画を切る

            //パーティクル再生が終わったら削除
            this.UpdateAsObservable().
                Where(_ => m_playPart.isActive == false).
                Subscribe(_ => Destroy(this.gameObject));
            
        }
    }
}
