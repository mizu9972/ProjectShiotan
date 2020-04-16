using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove2 : MonoBehaviour
{
    [Header("スピード調整用"), SerializeField]private float waruspeed;

    [Header("十字キーでの移動スピード"), SerializeField] private float speed;
    [Header("最大移動スピード"), SerializeField] private float Maxspeed;

    [Header("加速値"), SerializeField] private float kasoku;
    [Header("最大加速スピード"),SerializeField] private float MaxKasoku;
    private float Nowkasoku = 0;    //現在の加速度

    [Header("減速の割合（%）"), SerializeField] private float gensoku;
    [Header("プレイヤーのブレーキ時の減速度"), SerializeField] private float brake;
    
    [Header("回転の度合い"), SerializeField] float ang;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;

    private PlayerAnimator Animation;

    //移動フラグ用変数
    private bool MoveOn;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();
        Animation= GetComponent<PlayerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        //現在の速度取得
        Vector3 Max = rb.velocity;

        //移動フラグ
        MoveOn = false;

        //コントローラー入力　取得
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //コントローラー入力しているとき
        if (h != 0 || v != 0)
        {
            MoveOn = true;
            speed = Maxspeed;

            //プレイヤーの位置に入力の値足す
            Vector3 targetPositon = new Vector3(transform.position.x + h, transform.position.y, transform.position.z + v);

            //進行方向に回転していく
            Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
        else
        {
            //左へ進む
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveOn = true;
                speed = Maxspeed;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);
            }

            //右へ進む
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveOn = true;
                speed = Maxspeed;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);
            }

            //上へ進む
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveOn = true;
                speed = Maxspeed;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), step);
            }

            //下へ進む
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveOn = true;
                speed = Maxspeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180.0f, 0), step);
            }
        }

        //加速
        if (Input.GetKeyDown("joystick button 1")
            || Input.GetKeyDown(KeyCode.B))
        {
            MoveOn = true;
            Nowkasoku += kasoku;

            //限界加速度制限
            if (MaxKasoku < Nowkasoku)
            {
                Nowkasoku = MaxKasoku;
            }
        }
        else
        {
            //減速処理（加速）
            if (Nowkasoku > 0.5f)
            {
                Nowkasoku *= gensoku;
            }
            else
            {
                Nowkasoku = 0;
            }
        }

        Animation.SetAnimation(MoveOn);

        //減速処理（移動）
        if (speed > 0.5f)
        {
            speed *= gensoku;
        }
        else
        {
            speed = 0;
        }

        //初速度速くするための変数
        float xspeed=1;
        float speedx = Mathf.Abs(Max.x);
        float speedz = Mathf.Abs(Max.z);

        //現在のスピードの大きいほう(X・Z方向)と最大スピードの差分を取得(初速度を速くする用)
        if (speedx < speedz)
        {
            if (speedz < 0.5f)
            {
                xspeed = Maxspeed - speedz;
            }
        }
        else
        {
            if (speedx < 0.5f)
            {
                xspeed = Maxspeed - speedx;
            }
        }

        //慣性での移動用
        rb.AddForce(this.gameObject.transform.forward * (speed + Nowkasoku) * xspeed, ForceMode.Acceleration);

        //速度制限 (十字キーのスピード＋現在の加速度　を超えない)
        if ((speed + Nowkasoku) / 2 < Mathf.Abs(rb.velocity.x))
        {
            Max.x = (speed + Nowkasoku) / 2 * Mathf.Sign(rb.velocity.x);
            rb.velocity = Max;
        }
        if ((speed + Nowkasoku) / 2 < Mathf.Abs(rb.velocity.z))
        {
            Max.z = (speed + Nowkasoku) / 2 * Mathf.Sign(rb.velocity.z);
            rb.velocity = Max;
        }
    }
}
