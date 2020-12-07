using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class RockScript : MonoBehaviour
{
    [Header("HP減る量")]
    public int Damage;

    [Header("エフェクト再生スクリプト")]
    public ParticleEffectScript m_playPart = null;

    [Header("モデルのメッシュ")]
    public MeshRenderer m_ModelMesh = null;

    //HP管理マネージャー
    private Status HPStatus;

    private EffectCamera effectCamera;

    // Start is called before the first frame update
    void Start()
    {
        HPStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
        effectCamera = Camera.main.GetComponent<EffectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_ModelMesh.enabled = false;//岩モデルの描画を消す

            m_playPart.StartEffect();//Effectを再生

            HPStatus.DamageHP(Damage,true);//プレイヤーのHPを減少

            //パーティクル再生が終わったら削除
            this.UpdateAsObservable().
                Where(_ => m_playPart.isStopped()).Take(1).
                Subscribe(_ => Destroy(this.gameObject));
            effectCamera.Shake();
            HPStatus.DamageHP(Damage,true);
            Destroy(this.gameObject);
        }
    }
}
