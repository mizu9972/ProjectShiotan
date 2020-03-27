using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEsa : MonoBehaviour
{
    [Header("エサ投げるクールタイム")]
    public float wait;

    //エサが消えるまでの時間　測る用の変数
    private float time;

    [Header("エサのオブジェクト")]
    public GameObject EsaPrefab;

    [Header("エサ投げる力")]
    public float throwrange;


    void Start()
    {
        //最初から投げれる状態
        time = wait;
    }


    void Update()
    {
        //エサ投げるまでのクールタイム
        if(time<=wait)
        {
            time += Time.deltaTime;
        }

        //えさ投げる
        if (Input.GetKeyDown("joystick button 0")|| Input.GetKey(KeyCode.Space))
        {
            //クールタイム超えたら投げれる
            if (time >=wait)
            {
                var bulletInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
                bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);

                //エサ消える時間
                Destroy(bulletInstance, 5);

                //エサ再び投げるまでのクールタイム
                time = 0;
            }
        }
    }
}
