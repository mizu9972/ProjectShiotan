﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class RestFood : MonoBehaviour
{
    [SerializeField,Header("残りエサの数")]
    private float restFoods;

    [SerializeField, Header("HPを犠牲にするか")]
    private bool isSacrifi;

    [Header("1の位の描画オブジェクト")]
    public Image DigOne;

    [Header("10の位の描画オブジェクト")]
    public Image DigTen;

    private GameObject PlayerObj=null;//プレイヤー
    private bool isFast = true;//
    // Start is called before the first frame update
    void Start()
    {
        this.LateUpdateAsObservable().
            Where(_ => restFoods <= 0).
            Take(1).
            Subscribe(_ => isSacrifi = true);//残りエサ数が0になったらHP犠牲状態へ

        this.UpdateAsObservable().Where(_=>!PlayerObj).Subscribe(_ => Init());//プレイヤーポップ後に初期化
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerObj)//Playerのオブジェクトが空でなければ実行
        {
            if(!isSacrifi)
            {
                restFoods = PlayerObj.GetComponent<Player>().GetRestFood();
            }
            else
            {
                float nowHp = PlayerObj.GetComponent<HumanoidBase>().NowHP;
                float sacHp = PlayerObj.GetComponent<Player>().GetSacHp();
                restFoods = nowHp / sacHp + 1.0f;
                if(nowHp==1.0f)//HPが1の時はエサの数を0に
                {
                    restFoods = 0;
                }
            }
        }
        SetDigObjNum(); //描画する数値をセット
    }

    private void SetDigObjNum()
    {
        int setNum = (int)restFoods % 10;//1の位
        DigOne.GetComponent<DrawDig>().SetDrawNum(setNum);

        setNum = (int)restFoods / 10;//10の位
        DigTen.GetComponent<DrawDig>().SetDrawNum(setNum);
    }

    private void Init()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");//Playerを取得
    }
}