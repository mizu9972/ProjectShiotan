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
    
    [Header("回転の度合い"), SerializeField] private float ang;

    //オール漕ぐアニメーション
    Animator _animator;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;

    //移動フラグ用変数
    private bool MoveOn;

    //エサ投げている状態か？
    public bool EsaTrow;

    [Header("エサ投げるときにスピードが与える影響力"), SerializeField] private float s_powerPercent;
    //エサ投げるときにスピードが与える力
    public float speedpower;

    [Header("オール漕ぐ時間の間隔"), SerializeField] private float animetime;
    private float animecount;

    [Header("オール漕ぐ間隔　移動でのカウント"), SerializeField] private float olltimemove;
    [Header("オール漕ぐ間隔　加速でのカウント"), SerializeField] private float olltimekasoku;
    [Header("移動で足すアニメーション再生速度"), SerializeField] private float animespeedmove;
    [Header("加速で足すアニメーション再生速度"), SerializeField] private float animespeedkasoku;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();

        EsaTrow = false;
        
        _animator = GetComponent<Animator>();
        animecount = animetime;
    }

    // Update is called once per frame
    void Update()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        if (EsaTrow == true)
        {
            step = 0;
        }

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


        //アニメーション再生スピード　変更用
        float animespeed = animespeedmove / (Maxspeed / speed) + animespeedkasoku / (MaxKasoku / Nowkasoku);

        //アニメーションがエサ投げている状態か
        if (EsaTrow == false)
        {
            //アニメーション　再生スピード　変更
            _animator.speed = animespeed;
        }
        else
        {
            //アニメーション　再生スピード　変更
            _animator.speed = 1;

            //エサ投げ終了時　すぐにオール漕ぐ
            animecount = animetime;
        }


        //オール漕ぐ間隔　カウント用
        float ollspeed = olltimemove / (Maxspeed / speed) + olltimekasoku / (MaxKasoku / Nowkasoku);

        speedpower = (speed + Nowkasoku)* s_powerPercent;

        //移動しているか
        if (MoveOn == true && EsaTrow == false)
        {
            //スピードの値でオール漕ぐ間隔変化
            animecount += ollspeed;

            //設定した値超えるとオール漕ぐ
            if (animecount > animetime)
            {
                //オール漕ぐ　カウント初期化
                animecount = 0;

                //アニメーション最初から再生
                _animator.Play("Move", 0, 0.0f);
            }
        }


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
        float xspeed = 1;
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

    //移動を止める関数
    public void Stop()
    {
        speed = 0;
        Nowkasoku = 0;
        rb.velocity = Vector3.zero;
    }
}
