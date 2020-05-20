using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("移動する力")]
    public float Speed;

    [Header("速度制限")]
    public float MaxSpeed;

    [Header("進み始める角度")]
    public float GoAng;

    [Header("向く方向　設定")]
    public float Angle;

    [Header("向くまでの速さ")]
    public float AngSpeed;

    public BoxCollider Box;
    private Rigidbody rb;
    

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //すり抜け　ON
            Box.isTrigger = true;

            //プレイヤー動きとめる
            other.GetComponentInParent<ProtoMove2>().Stop();
            other.GetComponentInParent<ProtoMove2>().enabled = false;
            rb = other.GetComponentInParent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //回転の度合い
        float step = AngSpeed * Time.deltaTime;

        //プレイヤー　回転
        other.transform.rotation = Quaternion.Slerp(other.transform.rotation, Quaternion.Euler(0, Angle, 0), step);

        //プレイヤー　移動
        rb.AddForce(other.gameObject.transform.forward * Speed, ForceMode.Force);

        //スピード制限
        if (MaxSpeed < rb.velocity.x)
        {
            Vector3 Save = rb.velocity;
            Save.x = 5;
            
            rb.velocity =Save;
        }
        if (MaxSpeed < rb.velocity.z)
        {
            Vector3 Save = rb.velocity;
            Save.z = 5;

            rb.velocity = Save;
        }
    }

    //一方通行にする
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //すり抜け　OFF
            Box.isTrigger = false;
            other.GetComponentInParent<ProtoMove2>().enabled = true;    //プレイヤー動く処理　ON

            //出たとき動きとめる
            other.GetComponentInParent<ProtoMove2>().Stop();

            //チェックポイント　一度だけ
            GetComponent<Checkpoint>().enabled = false;
        }
    }
}
