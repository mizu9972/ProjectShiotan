using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove2 : MonoBehaviour
{
    [SerializeField]
    float waruspeed;    //スピード調整用

    private float speed;                    //十字キーでの移動スピード
    [SerializeField] float Maxspeed;        //最大移動スピード

    private float Nowkasoku = 0;   //現在の加速度
    [SerializeField] float kasoku;          //加速値
    [SerializeField] float MaxKasoku;       //最大加速スピード

    [Header("減速の割合（%）"), SerializeField]
    private float gensokuritu;

    public float brake;          //プレイヤーのブレーキ時の減速度

    [SerializeField] float ang;     //回転の度合い

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;

    //移動フラグ用変数
    private int MoveOn;

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
        //移動フラグ　初期化
        MoveOn = 0;

        //回転の度合い
        float step = ang * Time.deltaTime;

        //現在の速度取得
        Vector3 Max = rb.velocity;

        //コントローラー入力　取得
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //コントローラー入力しているとき
        if (h != 0 || v != 0)
        {
            MoveOn = 3;

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
                MoveOn = 1;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);
            }

            //右へ進む
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveOn = 1;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);
            }

            //上へ進む
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveOn = 2;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), step);
            }

            //下へ進む
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveOn = 2;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180.0f, 0), step);
            }
        }

        //移動の補正
        if(MoveOn!=0)
        {
            speed = Maxspeed;

            Max.z = Max.z * 1;
            Max.x = Max.x * 1;
            rb.velocity = Max;
        }

        //加速
        if (Input.GetKeyDown("joystick button 1")
            || Input.GetKeyDown(KeyCode.B))
        {
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
                Nowkasoku *= gensokuritu;
            }
            else
            {
                Nowkasoku = 0;
            }
        }
        //減速処理（移動）
        if (speed > 0.5f)
        {
            speed *= gensokuritu;
        }
        else
        {
            speed = 0;
        }

        //初速度速くするための変数
        float xspeed;
        float speedx = Max.x;
        float speedz = Max.z;

        //現在のスピードの大きいほう(X・Z方向)と最大スピードの差分を取得(初速度を速くする用)
        if (speedx < speedz)
        {
            xspeed = Maxspeed - Mathf.Abs(speedz);
        }
        else
        {
            xspeed = Maxspeed - Mathf.Abs(speedx);
        }

        //スピード調整用
        xspeed /= waruspeed;

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
