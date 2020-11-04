using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;

    [Header("移動速度")]
    public float Speed = 1f;

    [Header("左側の移動制限用コライダ")]
    public HitCheck LeftCollider;

    [Header("右側の移動制限用コライダ")]
    public HitCheck RightCollider;

    [Header("前側の移動制限用コライダ")]
    public HitCheck FrontCollider;

    [Header("後ろ側の移動制限用コライダ")]
    public HitCheck BackCollider;

    [Header("攻撃のためのコライダー")]
    public GameObject AttackCollider;

    [SerializeField, Header("イカダのどの位置にいるか")]
    private Vector2 OnRaftPosition;//イカダのどの位置にいるか

    [Header("回転の度合い")]
    public float ang;

    [Header("吹っ飛ぶ速さ")]
    public float BlowSpeed;

    [Header("吹き飛ぶ高さ")]
    public float BlowHigh;

    //基本Y座標　保存
    private float Savepos;

    //移動できるか
    private bool MoveActive;



    // Start is called before the first frame update
    void Start()
    {
        MoveActive = true;      //操作　可能
        Savepos = transform.position.y;         //基本Y座標　保存
        rb = this.GetComponent<Rigidbody>();    //Rigidbody　取得
        AttackCollider.SetActive(false);        //攻撃コライダー　非アクティブ
    }

    // Update is called once per frame
    void Update()
    {
        //移動可能
        if (MoveActive)
        {
            //移動処理
            MoveFunc();
        }
        else
        {
            //プレイヤー　吹っ飛び中
            if(BackCollider.GetisHit())
            {
                transform.position = new Vector3(transform.position.x + (-BlowSpeed * Time.deltaTime),
                                                 transform.position.y,
                                                 transform.position.z);
            }
        }
    }


    private void MoveFunc()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) && LeftCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (Speed * Time.deltaTime));

            //指定した方向にゆっくり回転する場合
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);
        }
        if (Input.GetKey(KeyCode.RightArrow) && RightCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (-Speed * Time.deltaTime));
            //指定した方向にゆっくり回転する場合
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);
        }
        if (Input.GetKey(KeyCode.UpArrow) && FrontCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) && BackCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x + (-Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);
        }

        //攻撃コライダー　アクティブ化
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackCollider.SetActive(true);
        }
    }

    public void SetRaftPosition(Vector2 pos)
    {
        //イカダのどこにいるかをセット
        OnRaftPosition = pos;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RidePiranha")
        {
            //プレイヤー向きをピラニアに(Z正面)
            this.transform.LookAt(other.transform.position);
            this.transform.rotation = new Quaternion(0, other.transform.rotation.y, 0, this.transform.rotation.w);
            transform.Rotate(0.0f, 90, 0.0f, Space.World);

            
            //上に吹き飛ばす
            Vector3 Throwpos = other.transform.forward;
            Throwpos.y = other.transform.position.y + 1;

            //吹き飛ぶ力　追加
            //rb.AddForce(Throwpos * BlowPower, ForceMode.Impulse);
            rb.AddForce(this.transform.up * BlowHigh, ForceMode.Impulse);

            //重力　ON
            rb.useGravity = true;

            //移動不可（吹っ飛び中）
            MoveActive = false;
        }

        //イカダに着地時
        if (other.tag == "PlayerRaft")
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);

            Vector3 pos = this.transform.position;
            pos.y = Savepos;
            this.transform.position = pos;

            //移動不可（吹っ飛び中）
            MoveActive = true;
        }
    }
}
