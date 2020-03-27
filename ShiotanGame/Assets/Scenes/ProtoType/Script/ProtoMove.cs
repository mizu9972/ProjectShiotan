using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove : MonoBehaviour
{
    [SerializeField]
    float waruspeed;    //スピード調整用

    private float speed;                    //十字キーでの移動スピード
    [SerializeField] float Maxspeed;        //最大移動スピード

    private float Nowkasoku = 0;   //現在の加速度
    [SerializeField] float kasoku;          //加速値
    [SerializeField] float MaxKasoku;       //最大加速スピード

    [SerializeField] float gensokuritu;     //減速の割合（%）
    public float brake;          //プレイヤーのブレーキ時の減速度

    [SerializeField] float ang;     //回転の度合い

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
        //ブレーキ判断用
        bool sts=false;

        //現在の速度取得
        Vector3 Max = rb.velocity;
        
        //左回転
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0.0f, -ang, 0.0f);
        }

        //右回転
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0.0f, ang, 0.0f);
        }

        //前進
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed = Maxspeed;
        }
        else
        {
            sts = true;
        }

        //加速
        if (Input.GetKeyDown(KeyCode.B))
        {
            Nowkasoku += kasoku;

            //限界加速度制限
            if (MaxKasoku<Nowkasoku)
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

        //ブレーキ
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // 物体のブレーキ
            speed *= brake; // 3Dの場合
            sts = false;
        }

        //減速するかどうか？（ブレーキしていない時＆＆十字キー↑押していないとき）
        if (sts == true)
        {
            //減速処理（移動）
            if (speed > 0.5f)
            {
                speed *= gensokuritu;
            }
            else
            {
                speed = 0;
            }
        }
        
        //初速度速くするための変数
        float xspeed;
        float speedx = Max.x;
        float speedz = Max.z;

        //現在のスピードの大きいほう(X・Z方向)と最大スピードの差分を取得(初速度を速くする用)
        if (speedx<speedz)
        {
            xspeed = Maxspeed - Mathf.Abs(speedz);
        }
        else
        {
            xspeed = Maxspeed - Mathf.Abs(speedx);
        }

        //スピード調整用
        xspeed /=waruspeed;

        //慣性での移動用
        rb.AddForce(this.gameObject.transform.forward * (speed+Nowkasoku)* xspeed, ForceMode.Acceleration);

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
