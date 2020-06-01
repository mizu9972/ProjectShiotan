using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("移動する力"), SerializeField]
    public float Speed;

    [Header("速度制限"), SerializeField]
    public float MaxSpeed;

    [Header("進み始める角度"), SerializeField]
    public float GoAng;

    [Header("向く方向　設定"), SerializeField]
    public float Angle;

    [Header("向くまでの速さ"), SerializeField]
    public float AngSpeed;

    //チェックポイントの壁の当たり判定　消す用
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
            //チェックポイントの壁　すり抜けON
            Box.isTrigger = true;

            //プレイヤー動きとめる
            other.GetComponentInParent<ProtoMove2>().Stop();
            other.GetComponentInParent<ProtoMove2>().enabled = false;
            rb = other.GetComponentInParent<Rigidbody>();
            rb.AddForce(other.gameObject.transform.forward * Speed, ForceMode.Force);

            AudioManager.Instance.PlaySE("SE_CHECKPOINT");
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
            Save.x = MaxSpeed;

            rb.velocity = Save;
        }
        if (MaxSpeed < rb.velocity.z)
        {
            Vector3 Save = rb.velocity;
            Save.z = MaxSpeed;

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
        }
    }
}
