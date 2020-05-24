using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove2 : MonoBehaviour
{
    [Header("初速度の割合（%）"), SerializeField] private float StartDashPower;
    [Header("十字キーでの移動スピード"), SerializeField] private float speed;
    [Header("最大移動スピード"), SerializeField] private float Maxspeed;

    [Header("加速値"), SerializeField] private float kasoku;
    [Header("最大加速スピード"),SerializeField] private float MaxKasoku;
    private float Nowkasoku = 0;    //現在の加速度

    [Header("慣性の減速の割合　通常移動時（%）"), SerializeField] private float InertialDawn;
    [Header("慣性の減速の割合　加速移動時（%）"), SerializeField] private float InertialAcceleDawn;
    
    [Header("回転の度合い"), SerializeField] private float ang;

    //オール漕ぐアニメーション
    Animator _animator;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;

    //移動フラグ用変数
    private bool MoveOn;

    //スタートダッシュフラグ用変数
    private bool OnStartDash=false;

    //エサ投げている状態か？
    public bool EsaThrow;

    [Header("エサ投げるときにスピードが与える影響力"), SerializeField] private float s_powerPercent;
    //エサ投げるときにスピードが与える力
    public float speedpower;

    [Header("オール漕ぐ時間の間隔"), SerializeField] private float animetime;
    private float animecount;

    [Header("オール漕ぐ間隔　移動でのカウント"), SerializeField] private float olltimemove;
    [Header("オール漕ぐ間隔　加速でのカウント"), SerializeField] private float olltimekasoku;
    [Header("移動で足すアニメーション再生速度"), SerializeField] private float animespeedmove;
    [Header("加速で足すアニメーション再生速度"), SerializeField] private float animespeedkasoku;

    public float NagareSpeed;
    public float Movespd;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得する
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        EsaThrow = false;
        
        animecount = animetime;     //最初動く時からオール漕ぐ
    }

    // Update is called once per frame
    void Update()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        //エサ投げてるとき　回転しない
        if (EsaThrow == true)
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
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);
            }

            //右へ進む
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveOn = true;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);
            }

            //上へ進む
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveOn = true;
                //指定した方向にゆっくり回転する場合
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), step);
            }

            //下へ進む
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveOn = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180.0f, 0), step);
            }
        }

        //加速(移動キー押していない場合　加速しない)
        if ((Input.GetKeyDown("joystick button 1")|| Input.GetKeyDown(KeyCode.B))
            &&MoveOn==true)
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
            if (Nowkasoku > 0.1f)
            {
                Nowkasoku *= InertialDawn/100;
            }
            else
            {
                Nowkasoku = 0;
            }
        }
        
        //十字キー押したときの移動
        if (MoveOn)
        {
            speed = Maxspeed;

            //初速度　補正　　キー押した最初だけスタートダッシュ
            if (5 > rb.velocity.magnitude&& OnStartDash==false)
            {
                rb.AddForce(this.gameObject.transform.forward * speed* StartDashPower/100, ForceMode.VelocityChange);
            }

            rb.AddForce(this.gameObject.transform.forward * (speed + Nowkasoku), ForceMode.Acceleration);

            //十字キー押した最初だけ　スタートダッシュ
            OnStartDash = true;
        }
        else
        {
            //スタートダッシュ初期化
            OnStartDash = false;

            //慣性での移動
            rb.AddForce(Max*1.5f);

            //減速
            rb.velocity *= InertialDawn / 100;
        }

        //速度制限
        if ((speed + Nowkasoku+ NagareSpeed) < rb.velocity.magnitude)
        {
            rb.velocity *= 0.9f;

            //加速時　減速大きく
            rb.velocity *= 1 - (Nowkasoku / MaxKasoku) * (1 - InertialAcceleDawn / 100);
        }

        if(NagareSpeed>1)
        {
            NagareSpeed *= 0.98f;
        }
        else
        {
            NagareSpeed = 0;
        }

        Movespd = rb.velocity.magnitude;

        //アニメーション再生スピード変更用
        if (speed > 0.5f)
        {
            speed *= InertialDawn/100;
        }
        else
        {
            speed = 0;
        }

        //アニメーション再生スピード　変更用
        float animespeed = animespeedmove / (Maxspeed / speed) + animespeedkasoku / (MaxKasoku / Nowkasoku);

        //アニメーションがエサ投げている状態か
        if (EsaThrow == false)
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

        //スピードによるエサ投げる距離の変化
        speedpower = (speed + Nowkasoku)* s_powerPercent;

        //スピードの値でオール漕ぐ間隔変化
        animecount += ollspeed;

        //オール漕ぐ関連（漕ぐ速度・再生）
        if (MoveOn == true && EsaThrow == false)
        {
            //設定した値超えるとオール漕ぐ
            if (animecount > animetime)
            {
                //オール漕ぐ　カウント初期化
                animecount = 0;

                //アニメーション最初から再生
                _animator.Play("Move", 0, 0.0f);

                AudioManager.Instance.PlaySE("SE_EOW");
            }
        }
        
    }

    //移動を止める関数
    public void Stop()
    {
        speed = 0;
        Nowkasoku = 0;
        rb.velocity = Vector3.zero;
    }
    
    public void SetNagare(float spd)
    {
        NagareSpeed = spd;
        speed = Maxspeed;
    }
}
