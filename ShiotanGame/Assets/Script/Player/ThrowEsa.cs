using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEsa : MonoBehaviour
{
    [Header("エサ投げるクールタイム")]
    public float wait;

    //エサが消えるまでの時間　測る用の変数
    private float time;

    //エサ投げるアニメーション　終了までの時間
    private float TrowEndCount;

    //エサ投げたかどうか
    private bool Trowbool;

    [Header("エサ投げるアニメーション　終了までの時間"), SerializeField]
    private float TrowEnd;

    [Header("エサ投げるタイミング"), SerializeField]
    private float TrowTime;

    [Header("エサのオブジェクト"), SerializeField]
    public GameObject EsaPrefab;

    [Header("回数超えた後に投げるオブジェクト"), SerializeField]
    public GameObject HPDawnEsa;

    [Header("エサ投げる力"), SerializeField]
    public float throwrange;

    [Header("エサ投げれる回数"), SerializeField]
    public float count;

    [Header("犠牲にするHP量"), SerializeField]
    private float SacrificeHP;

    private HumanoidBase HPcnt;
    private ProtoMove2 MoveScript;

    Animator _animator;


    void Start()
    {
        HPcnt = this.GetComponentInParent<HumanoidBase>();
        MoveScript= this.GetComponentInParent<ProtoMove2>();
        _animator = GetComponentInParent<Animator>();

        //最初から投げれる状態
        time = wait;
        Trowbool = false;
    }


    void Update()
    {
        //スピードによってエサ投げる距離変化
        float TrowPower = throwrange + MoveScript.speedpower;

        //エサ投げるまでのクールタイム
        if (time<=wait)
        {
            time += Time.deltaTime;
        }

        //エサ投げた状態の場合
        if (MoveScript.EsaThrow == true&& HPcnt.NowHP > 0)
        {
            TrowEndCount += Time.deltaTime;

            //エサ投げるアニメーション　終了処理
            if(TrowEndCount >= TrowEnd)
            {
                TrowEndCount = 0;
                MoveScript.EsaThrow = false;
                Trowbool = false;
            }

            //エサを設定したタイミングで投げる処理
            if(TrowEndCount >= TrowTime&&Trowbool==false)
            {
                //エサ投げた状態に
                Trowbool = true;

                //エサ残っているか
                if (count > 0)
                {
                    var bulletInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
                    bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * TrowPower, ForceMode.VelocityChange);

                    //エサ一個消費
                    count--;
                }
                else
                {
                    var bulletInstance = Instantiate<GameObject>(HPDawnEsa, this.transform.position, this.transform.rotation);
                    bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * TrowPower, ForceMode.VelocityChange);

                    float SaveHP = HPcnt.NowHP;
                    HPcnt.NowHP -= SacrificeHP;//犠牲にする分だけHPを減らす

                    //犠牲エサで死なないように設定
                    if (HPcnt.NowHP <= 0 && SaveHP != 1)
                    {
                        HPcnt.NowHP = 1;
                    }
                }
            }
        }

        //えさ投げる
        if (Input.GetKeyDown("joystick button 0")|| Input.GetKey(KeyCode.Space))
        {
            //クールタイム超えたら投げれる
            if (time >= wait && (count >0|| HPcnt.NowHP > 1))
            {
                //アニメーション最初から再生
                _animator.Play("Trow", 0, 0.0f);

                AudioManager.Instance.PlaySE("SE_THROW");
                MoveScript.EsaThrow = true;


                //エサ再び投げるまでのクールタイム
                time = 0;
            }
        }
    }

    public float GetCount()//残エサの数を返す
    {
        return count;
    }

    public float GetSacrificeHP()//犠牲にするHP量を返す
    {
        return SacrificeHP;
    }
}
