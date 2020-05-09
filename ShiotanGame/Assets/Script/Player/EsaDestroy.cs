using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaDestroy : MonoBehaviour
{
    [SerializeField,Header("エサ消えるまでの時間")]
    private float Destroytime=0;

    [SerializeField, Header("エサ浮上位置")]
    private float UpPos;

    [SerializeField, Header("エサ最大沈む位置")]
    private float DawnPos;

    private float time;
    
    //消滅カウント　スイッチ
    private bool timeOnOff = true;

    //エサ浮き沈み　スイッチ
    private bool updawn=true;

    private HumanoidBase HPcnt;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();

        HPcnt = this.GetComponent<HumanoidBase>();
    }

    // Update is called once per frame
    void Update()
    {
        // 座標を取得
        Vector3 pos = this.transform.position;

        //消滅カウント
        if (timeOnOff==true)
        {
            time += Time.deltaTime;
        }

        //エサ消滅
        if(Destroytime<=time|| HPcnt.NowHP <= 0)
        {
            Destroy(this.gameObject);
        }

        //エサ　浮き沈み
        if (updawn)
        {
            pos.y += 1.0f* Time.deltaTime;
            if (UpPos <= this.transform.position.y)
            {
                updawn = false;
            }
        }
        else
        {
            pos.y -= 1.0f * Time.deltaTime;
            if (DawnPos >= this.transform.position.y)
            {
                updawn = true;
            }
        }

        //エサ位置更新
        this.transform.position = pos;
    }

    public void IsCountDown(bool value) 
    {
        timeOnOff = value;
    }

    void OnCollisionEnter(Collision other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Stage_Wall")
        {
            rb.velocity = Vector3.zero;
        }

        if (layerName == "SeaPlane")
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;

            //エサをステージとの当たり判定消して浮き沈みするようにする
            this.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
