using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Player : MonoBehaviour
{
    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;

    private float restFood;
    

    [Header("エサ管理オブジェクト")]
    public GameObject FoodManager;

    [Header("所持鍵")]
    public int KeyCount;

    private void Start()
    {
        //スクリプトを取得
        GageScript = GameObject.Find("PlayerGage").GetComponentInChildren<Gage>();
        
        GageScript.InitGage(this.GetComponent<HumanoidBase>().InitHP);//ゲージの初期化
    }

    private void Update()
    {
        //現在のHPをゲージに反映
        GageScript.GageUpdate(this.GetComponent<HumanoidBase>().NowHP);
        restFood = FoodManager.GetComponent<ThrowEsa>().GetCount();//残りエサ数を表示
    }

    public float GetRestFood()
    {
        return restFood;
    }

    public GameObject GetFoodManager() {
        return FoodManager;
    }

    public void AddHp(float addvalue)//HP回復
    {
        this.GetComponent<HumanoidBase>().NowHP += addvalue;
        if(this.GetComponent<HumanoidBase>().NowHP>= this.GetComponent<HumanoidBase>().InitHP)
        {
            this.GetComponent<HumanoidBase>().NowHP = this.GetComponent<HumanoidBase>().InitHP;
        }
    }

    public void AddFoods(float addvalue)//エサの回復
    {
        this.GetComponentInChildren<ThrowEsa>().count += addvalue;
    }

    public void SetPlayerMove(bool isActive)//プレイヤーの操作可否を設定
    {
        this.GetComponent<ProtoMove2>().enabled = isActive;
    }

    public void SetThrowFoodEnable(bool isActive)//プレイヤーのエサ投げ可否を設定
    {
        this.GetComponentInChildren<ThrowEsa>().enabled = isActive;
    }

    public float GetSacHp()//犠牲にするHPを取得
    {
        return this.GetComponentInChildren<ThrowEsa>().GetSacrificeHP();
    }

    public void SetPlayerStatus(float hp,float foods,int key)//エサとHPの数値セット
    {
        this.GetComponent<HumanoidBase>().NowHP = hp;
        this.GetComponentInChildren<ThrowEsa>().count = foods;
        KeyCount = key;
    }
}
