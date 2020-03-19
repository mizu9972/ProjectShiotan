using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEsa : MonoBehaviour
{
    //エサが消えるまでの時間　測る用の変数
    private float wait;

    [Header("エサのオブジェクト")]
    public GameObject EsaPrefab;

    [Header("エサ消えるまでの時間")]
    public float Destroytime;

    [Header("エサ投げる力")]
    public float throwrange;


    void Start()
    {
        
    }


    void Update()
    {
        //エサ消えるまでの時間
        if(wait>0)
        {
            wait += Time.deltaTime;
            if(wait> Destroytime)
            {
                wait = 0;
            }
        }

        //えさ投げる
        if (Input.GetKey(KeyCode.Space))
        {
            if (wait == 0.0f)
            {
                var bulletInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
                bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);
                bulletInstance.tag = "Esa";     // ピラニアが追いかける対象の判別にタグが必要だったため追加　青木
                bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);
                Destroy(bulletInstance, 5.0f);
                wait += Time.deltaTime;
            }
        }
    }
}
