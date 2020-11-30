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


    [Header("攻撃アニメーション　開始時間指定(0.0～1.0)")]
    public float Atk_StartTime;

    [Header("攻撃アニメーション　終了時間指定(0.0～1.0)")]
    public float Atk_EndTime;

    //基本Y座標　保存
    private float Savepos;

    //移動できるか
    private bool MoveActive;
    
    private bool Air;       //空中に吹っ飛んでいるか
    private bool _Attack;   //攻撃状態
    private bool _Kokeru;   //ダメージ受けてこけるアニメーション状態か
    private bool _JumpKoke;

    // Animator コンポーネント
    private Animator _animator;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isAttack = "isAttack";
    private const string key_isKokeru = "isKokeru";

    //イカダ端　位置
    private float IkadaHasi;



    // Start is called before the first frame update
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを習得する
        this._animator = GetComponent<Animator>();

        //プレイヤー　最高高度　設定
        BlowHigh = BlowHigh + this.transform.localPosition.y;

        MoveActive = true;      //操作　可能
        Savepos = transform.position.y;         //基本Y座標　保存
        rb = this.GetComponent<Rigidbody>();    //Rigidbody　取得
        AttackCollider.SetActive(false);        //攻撃コライダー　非アクティブ

        Air = false;
        _Attack = false;
        _Kokeru = false;
        _JumpKoke = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_Kokeru == false)
        {
            //移動可能
            if (MoveActive)
            {
                if (_Attack == false)
                {
                    //移動・アクティブ処理
                    MoveFunc();
                }
                else if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > Atk_EndTime)
                {
                    //アニメーション終了
                    _Attack = false;
                    this._animator.SetBool(key_isAttack, false);

                    AttackCollider.SetActive(false); //攻撃用コライダー　非アクティブ化
                }
            }
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            //アニメーション終了
            _Kokeru = false;
            this._animator.SetBool(key_isKokeru, false);
        }

        //イカダに着地
        if (Savepos > transform.position.y)
        {
            //重力　停止
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);

            //イカダにめりこまない
            Vector3 pos = this.transform.position;
            pos.y = Savepos;
            this.transform.position = pos;

            //移動可能
            MoveActive = true;

            //攻撃くらったか
            if(_JumpKoke)
            {
                _JumpKoke = false;
                SetKokeru();
            }
        }

        //設定した高さまで飛んだ
        if (BlowHigh < this.transform.position.y)
        {
            rb.useGravity = true;
            Air = false;
            _JumpKoke = true;   //イカダ着地時　こける
        }

        //空中
        if (Savepos < this.transform.localPosition.y)
        {
            //移動不可能
            MoveActive = false;
        }

        //吹き飛び
        if(Air)
        {
            //上に吹き飛ばす
            Vector3 Throwpos = this.transform.forward;
            Throwpos.y = this.transform.position.y + 1;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos * 2, ForceMode.Force);
        }
        
        //イカダからはみ出さない処理
        MoveLimit();
    }

    //移動・アクティブ処理
    private void MoveFunc()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        // RunからWaitに遷移する
        this._animator.SetBool(key_isRun, false);
        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (Speed * Time.deltaTime));
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0f, 0), step);

            // WaitからRunに遷移する
            this._animator.SetBool(key_isRun, true);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             transform.position.z + (-Speed * Time.deltaTime));
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180f, 0), step);

            // WaitからRunに遷移する
            this._animator.SetBool(key_isRun, true);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);
            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), step);

            // WaitからRunに遷移する
            this._animator.SetBool(key_isRun, true);

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x + (-Speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);

            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), step);

            // WaitからRunに遷移する
            this._animator.SetBool(key_isRun, true);
        }


        //攻撃コライダー　アクティブ化
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Wait or RunからAttackに遷移する
            this._animator.SetBool(key_isAttack, true);
            this._animator.SetBool(key_isRun, false);

            //アニメーション最初から再生
            _animator.Play("Attack", 0, Atk_StartTime);

            AttackCollider.SetActive(true); //攻撃用コライダー　アクティブ化
            _Attack = true;
        }
    }

    //イカダからはみ出さない処理
    private void MoveLimit()
    {
        Vector3 Pos = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);

        //X軸の端
        if (IkadaHasi < this.transform.localPosition.x)
        {
            Pos.x = IkadaHasi;
        }
        if (-IkadaHasi > this.transform.localPosition.x)
        {
            Pos.x = -IkadaHasi;
        }

        //Z軸の端
        if (IkadaHasi < this.transform.localPosition.z)
        {
            Pos.z = IkadaHasi;
        }
        if (-IkadaHasi > this.transform.localPosition.z)
        {
            Pos.z = -IkadaHasi;
        }

        //位置修正
        transform.localPosition = Pos;
    }

    //こけるアニメーション　セット
    public void SetKokeru()
    {
        //アニメーション最初から再生
        _animator.Play("Kokeru", 0, 0.0f);
        this._animator.SetBool(key_isKokeru, true);
        _Kokeru = true;
    }


    public void SetIkadaHasi(float hasi)
    {
        IkadaHasi = hasi;
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
