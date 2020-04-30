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
    private void Start()
    {
        //スクリプトを取得
        GageScript = GameObject.Find("Gage").GetComponentInChildren<Gage>();
        
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

    public void SetPlayerMove(bool isActive)//プレイヤーの操作可否を設定
    {
        this.GetComponent<ProtoMove2>().enabled = isActive;
    }

    public float GetSacHp()//犠牲にするHPを取得
    {
        return this.GetComponentInChildren<ThrowEsa>().GetSacrificeHP();
    }
}
