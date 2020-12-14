﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;

    [Header("移動速度")]
    public float Speed = 1f;

    [Header("攻撃のためのコライダー")]
    public GameObject AttackCollider;

    [Header("攻撃時のエフェクト")]
    public GameObject AttackEffect;

    [SerializeField, Header("イカダのどの位置にいるか")]
    private Vector2 OnRaftPosition;//イカダのどの位置にいるか

    [Header("回転の度合い")]
    public float ang;

    [Header("吹っ飛ぶ速さ")]
    public float BlowSpeed;

    [Header("吹き飛ぶ高さ")]
    public float BlowHigh;

    [SerializeField, Header("アニメーション基本の再生スピード")]
    private float AnimeSpeed = 1;

    [SerializeField, Header("攻撃アニメーション再生スピード")]
    private float AttackSpeed = 1;

    [SerializeField, Header("こけるアニメーション再生スピード")]
    private float KokeruSpeed = 1;

    [SerializeField, Header("起き上がるアニメーション再生スピード")]
    private float StandUpSpeed = 1;

    [Header("攻撃アニメーション　開始時間指定(0.0～1.0)")]
    public float Atk_StartTime;

    [Header("攻撃アニメーション　終了時間指定(0.0～1.0)")]
    public float Atk_EndTime;

    //基本Y座標　保存
    private Vector3 Savepos;
    
    private bool _Attack;   //攻撃状態
    private bool _Kokeru;   //ダメージ受けてこけるアニメーション状態か
    private bool _JumpKoke;
    private bool _Blow;     //吹き飛び中か

    // Animator コンポーネント
    private Animator _animator;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isAttack = "isAttack";
    private const string key_isKokeru = "isKokeru";

    //イカダ端　位置
    private float IkadaWidth;

    //立ち上がったか？
    private bool _Stand;
    
    private bool _Goal;     //ゴールしたか
    private bool _Ikada;    //イカダ中心に移動したか
    private bool _ZoomC;   //カメラズーム完了したか
    
    [SerializeField, Header("中心に戻る速度")]
    public float GoalSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを習得する
        this._animator = GetComponent<Animator>();

        //プレイヤー　最高高度　設定
        BlowHigh = BlowHigh + this.transform.localPosition.y;
        
        Savepos = transform.localPosition;         //基本Y座標　保存
        rb = this.GetComponent<Rigidbody>();    //Rigidbody　取得
        AttackCollider.SetActive(false);        //攻撃コライダー　非アクティブ
        AttackEffect.SetActive(false);        //攻撃エフェクト　非アクティブ

        _Attack = false;
        _Kokeru = false;
        _JumpKoke = false;
        _Stand = true;
        _Goal = false;
        _Ikada = false;
        _ZoomC = false;
        _Blow = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //ゴールしていない時　操作可能
        if (_Goal == false)
        {
            //倒れている状態か
            if (_Kokeru == false)
            {
                //攻撃していない状態か
                if (_Attack == false)
                {
                    //吹き飛び中でない
                    if(_Blow)
                    {
                        //移動・アクティブ処理
                        MoveFunc();
                    }
                }
                else if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > Atk_EndTime)
                {
                    //アニメーション終了
                    _Kokeru = false;
                    _Attack = false;
                    this._animator.SetBool(key_isAttack, false);
                    this._animator.SetBool(key_isRun, false);
                    this._animator.SetBool(key_isKokeru, false);

                    AttackCollider.SetActive(false); //攻撃用コライダー　非アクティブ化
                    AttackEffect.SetActive(false);        //攻撃エフェクト　非アクティブ化
                }
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //立ち上がりしているか
                if (_Stand)
                {
                    _Stand = false;

                    //アニメーション　再生スピード　変更
                    _animator.speed = StandUpSpeed;
                }
                else
                {
                    //アニメーション終了
                    _Kokeru = false;
                    _Attack = false;
                    _Stand = true;
                    _Blow = true;
                    this._animator.SetBool(key_isAttack, false);
                    this._animator.SetBool(key_isRun, false);
                    this._animator.SetBool(key_isKokeru, false);
                }
            }

            //イカダに着地
            if (Savepos.y > transform.localPosition.y)
            {
                //重力　停止
                rb.useGravity = false;
                rb.velocity = new Vector3(0, 0, 0);

                //イカダにめりこまない
                Vector3 pos = this.transform.localPosition;
                pos.y = Savepos.y;
                this.transform.localPosition = pos;

                //攻撃くらったか
                if (_JumpKoke)
                {
                    _JumpKoke = false;
                    SetKokeru();
                }
            }

            //設定した高さまで飛んだ
            if (BlowHigh < this.transform.localPosition.y)
            {
                rb.useGravity = true;
                _JumpKoke = true;   //イカダ着地時　こける
            }

            //イカダからはみ出さない処理
            MoveLimit();
        }
        else
        {
            float step = GoalSpeed * Time.deltaTime;
            //プレイヤーの移動の値足す
            Vector3 targetPositon = Vector3.MoveTowards(transform.localPosition, Savepos, step);
            
            //進行方向に回転していく
            Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step*3);

            transform.localPosition = targetPositon;

            //中心に移動
            if ( transform.localPosition == Savepos)
            {
                this._animator.SetBool(key_isAttack, false);
                this._animator.SetBool(key_isRun, false);
                this._animator.SetBool(key_isKokeru, false);

                //カメラズーム終了　＆　イカダ中心に移動
                if(_ZoomC&&_Ikada)
                {
                    _animator.Play("Clear", 0, 0.0f);
                    this.enabled = false;
                }
            }
        }
    }

    //移動・アクティブ処理
    private void MoveFunc()
    {
        //回転の度合い
        float step = ang * Time.deltaTime;

        // RunからWaitに遷移する
        this._animator.SetBool(key_isRun, false);

        //コントローラー入力　取得
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //アニメーション　再生スピード　変更
        _animator.speed = AnimeSpeed;

        //コントローラー入力しているとき
        if (h != 0 || v != 0)
        {
            //プレイヤーの位置に入力の値足す
            Vector3 targetPositon = new Vector3(transform.position.x + (v * Speed * Time.deltaTime), transform.position.y, transform.position.z - (h * Speed * Time.deltaTime));

            //進行方向に回転していく
            Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);

            transform.position = targetPositon;

            // WaitからRunに遷移する
            this._animator.SetBool(key_isRun, true);
        }
        else
        {
            //十字キー　移動
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
        }

        //攻撃コライダー　アクティブ化
        if (Input.GetKeyDown(KeyCode.Space) && _Attack == false)
        {
            // Wait or RunからAttackに遷移する
            this._animator.SetBool(key_isAttack, true);
            this._animator.SetBool(key_isRun, false);
            this._animator.SetBool(key_isKokeru, false);

            //アニメーション最初から再生
            _animator.Play("Attack", 0, Atk_StartTime);

            //アニメーション　再生スピード　変更
            _animator.speed = AttackSpeed;

            AttackCollider.SetActive(true); //攻撃用コライダー　アクティブ化
            AttackEffect.SetActive(true);        //攻撃エフェクト　アクティブ化
            _Attack = true;
        }
    }

    //イカダからはみ出さない処理
    private void MoveLimit()
    {
        Vector3 Pos = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);

        //X軸の端　超えない
        if (IkadaWidth < this.transform.localPosition.x)
        {
            Pos.x = IkadaWidth;
        }
        if (-IkadaWidth > this.transform.localPosition.x)
        {
            Pos.x = -IkadaWidth;
        }

        //Z軸の端　超えない
        if (IkadaWidth < this.transform.localPosition.z)
        {
            Pos.z = IkadaWidth;
        }
        if (-IkadaWidth > this.transform.localPosition.z)
        {
            Pos.z = -IkadaWidth;
        }

        //位置修正
        transform.localPosition = Pos;
    }

    //こけるアニメーション　セット
    public void SetKokeru()
    {
        //アニメーション　再生スピード　変更
        _animator.speed = KokeruSpeed;

        //アニメーション最初から再生
        _animator.Play("Kokeru", 0, 0.0f);
        this._animator.SetBool(key_isAttack, false);
        this._animator.SetBool(key_isRun, false);
        this._animator.SetBool(key_isKokeru, true);
        AttackCollider.SetActive(false); //攻撃用コライダー　非アクティブ化
        _Attack = false;
        _Kokeru = true;
    }

    //イカダの幅　取得
    public void SetIkadaWidth(float hasi)
    {
        IkadaWidth = hasi;
    }

    public void SetRaftPosition(Vector2 pos)
    {
        //イカダのどこにいるかをセット
        OnRaftPosition = pos;
    }

    public void SetBlow()
    {
        _Blow = false;
    }

    public void SetGoal()
    {
        _Goal= true;
    }

    public void SetZoomC()
    {
        _ZoomC= true;
    }

    public void SetIkada()
    {
        _Ikada= true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

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
