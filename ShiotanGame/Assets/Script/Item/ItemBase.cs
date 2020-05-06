﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum e_ItemType {
    HPRecover,
    EsaRecover,
    End
}

public class ItemBase : MonoBehaviour
{
    [SerializeField,Header("アイテムの種類")]
    private e_ItemType ItemType;

    private delegate void Item();
    private Item ItemScript;    // Itemの効果を入れる

    [SerializeField, Header("プレイヤーが回復するHP")]
    private float HealHP;
    [SerializeField, Header("プレイヤーが回復するエサ")]
    private float HealFood;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // ItemTypeによって実行するスクリプトを変化させる
        switch (ItemType) {
            case e_ItemType.HPRecover:
                ItemScript = HPRecover;
                break;

            case e_ItemType.EsaRecover:
                ItemScript = EsaRecover;
                break;

            case e_ItemType.End:
            default:
                ItemScript = null;
                break;
        }
    }

    // アイテムを取得した際にこれを実行する
    public void UseItem() 
    {
        if (!Player) {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        ItemScript.Invoke();
        Destroy(gameObject);
    }

    #region Item効果　基本的にprivate
    // HP回復
    private void HPRecover() 
    {
        Player.GetComponent<HumanoidBase>().NowHP += HealHP;
        if(Player.GetComponent<HumanoidBase>().InitHP < Player.GetComponent<HumanoidBase>().NowHP) {
            Player.GetComponent<HumanoidBase>().NowHP = Player.GetComponent<HumanoidBase>().InitHP;
        }
    }

    // 餌回復
    private void EsaRecover() 
    {
        Player.GetComponent<Player>().AddFoods(HealFood);
    }
    #endregion
}
