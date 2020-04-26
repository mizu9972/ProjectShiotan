using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaDestroy : MonoBehaviour
{
    [SerializeField,Header("エサ消えるまでの時間")]
    private float Destroytime=0;

    [SerializeField, Header("エサ浮上位置")]
    private float UpPos = 0;

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
        if(updawn)
        {
            pos.y += 1.0f* Time.deltaTime;
            if (UpPos+0.3f <= this.transform.position.y)
            {
                updawn = false;
            }
        }
        else
        {
            pos.y -= 1.0f * Time.deltaTime;
            if (UpPos - 0.3f >= this.transform.position.y)
            {
                updawn = true;
            }
        }

        //エサ位置更新
        this.transform.position = pos;
    }

    public void TimeCountOnOff()
    {
        if (timeOnOff == false)
        {
            timeOnOff = true;
        }
        else
        {
            timeOnOff = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Stage_Floor")
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
    }
}
