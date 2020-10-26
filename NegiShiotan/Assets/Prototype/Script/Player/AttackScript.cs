using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [Header("再生成するオブジェクト")]
    public GameObject obj;

    //攻撃開始時間
    private float StartTime;

    [Header("攻撃時間　何秒か")]
    public float EndTime;

    // Start is called before the first frame update
    void Start()
    {

    }
    void OnEnable()
    {
        //アクティブ化　開始時間
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //一定時間経過で攻撃コライダー　非アクティブ化
        if(Time.time-StartTime>EndTime)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Enemy")
        {
            //ぶつかった　魚削除
            Destroy(other.gameObject);

            //弾丸型の魚生成し　前方に飛ばす
            GameObject instance = (GameObject)Instantiate(obj,
                                                          this.transform.position,
                                                          this.transform.rotation);
        }
    }
}
