﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScript : MonoBehaviour
{
    //移動可能か
    [Header("移動可能か")]
    public bool MoveActive;

    //停止時間
    private float cooltime;
    private bool Air;

    //Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    private Rigidbody rb;

    private GameObject PlayerObj;   //プレイヤーの位置
    private Rigidbody PlayerRb;
    private Status PlayerStatus;    //プレイヤーのステータス

    [Header("与えるダメージ量")]
    public int ATK;

    [Header("回転の度合い")]
    public float ang;

    [Header("移動速度")]
    public float Speed;

    [Header("プレイヤー　吹き飛ばす力")]
    public float BlowPower;

    [Header("ピラニア　吹き飛ぶ高さ")]
    public float BlowHigh;

    [Header("Effect")]
    public ParticleEffectScript m_Effect;

    //イカダY座標　保存
    private float Savepos=0;

    //自身（ピラニア）の当たり判定
    private BoxCollider ThisBox;

    //イカダ乗り込み時の処理　一度だけ行いたい
    bool onePlay;

    public float IkadaSize;


    //Start is called before the first frame update
    void Start()
    {
        Air = false;
        onePlay = false;
        MoveActive = false;     //動かない
        cooltime = 1;       //動かない時間
        PlayerObj = GameObject.FindGameObjectWithTag("Human");
        PlayerRb = GameObject.FindGameObjectWithTag("Human").GetComponent<Rigidbody>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
        rb = this.GetComponent<Rigidbody>();

        //親子関係したとき　メッシュがずれるバグ解消のための一文
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

        //イカダを親オブジェクトに設定
        this.transform.SetParent(PlayerObj.transform.parent, true);

        //自身の当たり判定　取得
        ThisBox = this.GetComponent<BoxCollider>();

        IkadaSize = PlayerStatus.GetIkadaSize();
    }

    //Update is called once per frame
    void Update()
    {
        if(cooltime>0)
        {
            cooltime -= 0.1f;
        }

        //XZ回転　初期化
        Quaternion XYrot = this.transform.rotation;
        XYrot.x = 0;
        XYrot.z = 0;

        //現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = XYrot;

        //設定した高さまで飛んだ
        if (BlowHigh < this.transform.position.y)
        {
            rb.useGravity = true;
            ThisBox.isTrigger = false;
            Air = false;
        }

        if (MoveActive==false&&Air)
        {
            //吹き飛ぶ方向
            Vector3 Throwpos2 = -this.transform.forward;
            Throwpos2.y = BlowHigh;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos2 * BlowPower, ForceMode.Force);
        }

        //移動可能か（イカダの上）
        if (MoveActive && cooltime < 0)
        {
            //プレイヤー方向のベクトル　取得
            Vector3 relativePos = PlayerObj.transform.position - this.transform.position;

            //プレイヤーの方　向く
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = 0;
            rotation.z = 0;

            //現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 0.1f);
            
            //移動処理（前方へ）
            Vector3 velocity = gameObject.transform.rotation * new Vector3(0, 0, Speed);
            gameObject.transform.position += velocity * Time.deltaTime;
        }

        if(onePlay==true)
        {
            //イカダから落ちないようにする処理
            Vector3 Pos = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);

            //X軸の端
            if (IkadaSize < this.transform.localPosition.x)
            {
                Pos.x = IkadaSize;
            }
            if (-IkadaSize > this.transform.localPosition.x)
            {
                Pos.x = -IkadaSize;
            }

            //Z軸の端
            if (IkadaSize < this.transform.localPosition.z)
            {
                Pos.z = IkadaSize;
            }
            if (-IkadaSize > this.transform.localPosition.z)
            {
                Pos.z = -IkadaSize;
            }

            //位置修正
            transform.localPosition = Pos;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //人間とぶつかる
        if (other.tag == "Human" && MoveActive)
        {
            //プレイヤーのHP減少
            bool sts = PlayerStatus.DamageHP(ATK,false);

            //  無敵時間外にプレイヤーに当たったら吹っ飛ばす
            if (sts)
            {
                ThisBox.isTrigger = true;

                //プレイヤー向きをピラニアに(Z正面)
                other.transform.LookAt(this.transform.position);
                other.transform.rotation = new Quaternion(0, other.transform.rotation.y, 0, other.transform.rotation.w);

                //上に吹き飛ばす
                Vector3 Throwpos = this.transform.forward;
                Throwpos.y = this.transform.position.y + 3;

                //プレイヤー速度　初期化
                PlayerRb.velocity *= 0;

                //吹き飛ぶ力　追加
                PlayerRb.AddForce(Throwpos * BlowPower, ForceMode.Impulse);
            }

            //吹き飛ぶ方向
            Vector3 Throwpos2 = -this.transform.forward;
            Throwpos2.y = BlowHigh;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos2 * BlowPower, ForceMode.Impulse);

            //移動不可能
            cooltime = 3;
            MoveActive = false;
            Air = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //イカダとぶつかる
        if (other.gameObject.tag == "Player")
        {
            //一度だけ
            if(onePlay == false)
            {
                onePlay = true;

                //親子関係したとき　メッシュがずれるバグ解消のための一文
                this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

                //イカダを親オブジェクトに設定
                this.transform.SetParent(PlayerObj.transform.parent, true);

                //イカダの上の座標　取得
                Savepos = this.transform.position.y;
                BlowHigh += this.transform.localPosition.y;

                gameObject.layer = LayerMask.NameToLayer("Default");
            }

            cooltime = 1.0f;

            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);

            Vector3 pos = this.transform.position;
            pos.y = Savepos;
            this.transform.position = pos;

            //移動可能
            MoveActive = true;
        }
    }

    public void EffectPlay()//Effect再生
    {
        m_Effect.StartEffect();
    }
}