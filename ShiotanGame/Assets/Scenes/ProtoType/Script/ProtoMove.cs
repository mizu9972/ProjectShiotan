using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove : MonoBehaviour
{
    private float speed;         //プレイヤーの動くスピード
    public float MaxSpeed;      //最大スピード
    public float kasoku;         //加速スピード
    public float speedmainasu;   //プレイヤーの自然な減速度
    public float brake;          //プレイヤーのブレーキ時の減速度

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得

        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Max = rb.velocity;

        // transformを取得
        Transform myTransform = this.transform;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // ワールド座標基準で、現在の回転量へ加算する
            myTransform.Rotate(0.0f, -1.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // ワールド座標基準で、現在の回転量へ加算する
            myTransform.Rotate(0.0f, 1.0f, 0.0f, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed = kasoku;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // 物体のブレーキ
            speed *= brake; // 3Dの場合
        }

        if (MaxSpeed < Mathf.Abs(rb.velocity.x))
        {
            Max.x=MaxSpeed* Mathf.Sign(rb.velocity.x);
            rb.velocity = Max;
        }
        if (MaxSpeed < Mathf.Abs(rb.velocity.z))
        {
            Max.z = MaxSpeed * Mathf.Sign(rb.velocity.z);
            rb.velocity = Max;
        }
        rb.AddForce(this.gameObject.transform.forward * speed, ForceMode.Acceleration);

        if (speed > 0.5f)
        {
            speed *= speedmainasu;
        }
        else
        {
            speed = 0;
        }

        //rb.AddForce(force, ForceMode.Force);            // 力を加える(徐々)
        Debug.Log("速度: " + rb.velocity);

    }
}
