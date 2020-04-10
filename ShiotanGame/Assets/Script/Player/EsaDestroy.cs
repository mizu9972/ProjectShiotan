using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaDestroy : MonoBehaviour
{
    [SerializeField,Header("エサ消えるまでの時間")]
    private float Destroytime=0;

    private float time;
    
    //消滅カウント　スイッチ
    private bool timeOnOff = true;

    //エサ浮き沈み　スイッチ
    private bool updawn=true;

    private HumanoidBase HPcnt;

    // Start is called before the first frame update
    void Start()
    {
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

        if(Destroytime<=time|| HPcnt.NowHP <= 0)
        {
            Destroy(this.gameObject);
        }

        //エサ　浮き沈み
        if(updawn)
        {
            pos.y += 1.0f* Time.deltaTime;
            if (0.3f <= this.transform.position.y)
            {
                updawn = false;
            }
        }
        else
        {
            pos.y -= 1.0f * Time.deltaTime;
            if (-0.3f >= this.transform.position.y)
            {
                updawn = true;
            }
        }

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
}
