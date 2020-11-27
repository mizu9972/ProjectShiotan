using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;

    [Header("移動速度")]
    public float Speed = 1f;

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

    //空中に吹っ飛んでいるか
    private bool Air;

    // Animator コンポーネント
    private Animator animator;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isAttack = "isAttack";
    

    // Start is called before the first frame update
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();

        BlowHigh = BlowHigh + this.transform.localPosition.y;

        MoveActive = true;      //操作　可能
        Savepos = transform.position.y;         //基本Y座標　保存
        rb = this.GetComponent<Rigidbody>();    //Rigidbody　取得
        AttackCollider.SetActive(false);        //攻撃コライダー　非アクティブ

        Air = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //移動可能
        if (MoveActive)
        {
            //移動処理
            MoveFunc();
        }

        //イカダに着地
        if (Savepos > transform.position.y)
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);

            Vector3 pos = this.transform.position;
            pos.y = Savepos;
            this.transform.position = pos;

            //移動可能
            MoveActive = true;
        }

        //設定した高さまで飛んだ
        if (BlowHigh < this.transform.position.y)
        {
            rb.useGravity = true;
            Air = false;
        }

        //空中
        if (Savepos < this.transform.localPosition.y)
        {
            //移動不可能
            MoveActive = false;
        }

        if(Air)
        {
            //上に吹き飛ばす
            Vector3 Throwpos = this.transform.forward;
            Throwpos.y = this.transform.position.y + 1;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos * 2, ForceMode.Force);
        }
    }


    private void MoveFunc()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        // RunからWaitに遷移する
        this.animator.SetBool(key_isRun, false);
        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (Speed * Time.deltaTime));
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0f, 0), step);

            // WaitからRunに遷移する
            this.animator.SetBool(key_isRun, true);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (-Speed * Time.deltaTime));
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180f, 0), step);

            // WaitからRunに遷移する
            this.animator.SetBool(key_isRun, true);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);

            // WaitからRunに遷移する
            this.animator.SetBool(key_isRun, true);

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x + (-Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);

            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);

            // WaitからRunに遷移する
            this.animator.SetBool(key_isRun, true);
        }

        //攻撃コライダー　アクティブ化
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Wait or RunからAttackに遷移する
            this.animator.SetBool(key_isAttack, true);
            this.animator.SetBool(key_isRun, false);

            AttackCollider.SetActive(true);

            // AttackからWait or Runに遷移する
            //this.animator.SetBool(key_isAttack, false);
        }
    }

    public void SetRaftPosition(Vector2 pos)
    {
        //イカダのどこにいるかをセット
        OnRaftPosition = pos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //移動不可能
            MoveActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "IkadaMoveLimit")
        {
            //rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

    }

    private void OnCollisionStay(Collision other)
    {
    }
}
