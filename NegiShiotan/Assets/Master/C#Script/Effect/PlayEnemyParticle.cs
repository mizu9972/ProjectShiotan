using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemyParticle : MonoBehaviour
{
    [SerializeField, Header("エフェクトのY座標")]
    private float m_EffectPosY = 2.8f;

    [SerializeField, Header("着水エフェクト")]
    List<GameObject> m_ParticleObj = new List<GameObject>();
    
    List<ParticleEffectScript> m_ParticleList = new List<ParticleEffectScript>();//着水エフェクトリスト本体

    [SerializeField, Header("着水エフェクトリスト使用状態")]
    List<bool> m_isUseParticle = new List<bool>();

    [Header("SE:着水時")]
    public SEPlayer SE;

    private Vector3 m_PopPosition;//パーティクルを表示させるポジション
    // Start is called before the first frame update
    void Awake()
    {
        for(int cnt=0;cnt<m_ParticleObj.Count;cnt++)//パーティクルのオブジェクトリストから再生スクリプトを取得
        {
            m_ParticleList.Add(m_ParticleObj[cnt].GetComponent<ParticleEffectScript>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int cnt = 0;cnt<m_ParticleList.Count;cnt++)
        {
            //再生が終わったパーティクルを未使用に
            if(m_ParticleList[cnt].isStopped())
            {
                m_isUseParticle[cnt] = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="RideFish"||other.tag=="RidePiranha")//魚介類が触れたらエフェクト再生
        {
            SE.PlaySound();

            m_PopPosition = new Vector3(other.transform.position.x,
                                        m_EffectPosY,
                                        other.transform.position.z);//衝突オブジェクトのX,Z座標を格納

            int usenum = CheckUseParticle();//使用できる番号を検索

            m_ParticleObj[usenum].transform.position = m_PopPosition;//格納した位置に未使用のパーティクルを移動
            m_isUseParticle[usenum] = true;//パーティクルを使用状態に

            m_ParticleList[usenum].StartEffect();//エフェクト再生
        }
        
    }

    private int CheckUseParticle()
    {
        int val = 0;
        for (int cnt = 0; cnt < m_ParticleList.Count; cnt++)
        {
            if(!m_isUseParticle[cnt])
            {
                val = cnt;
                return val;
            }
        }

        Debug.Log("未使用のパーティクルなし");
        return -1;
    }
}
