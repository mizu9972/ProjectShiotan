using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum e_ItemType {
    HPRecover,
    EsaRecover,
    KeyDrops,
    End
}

public class ItemBase : MonoBehaviour
{
    [SerializeField,Header("アイテムの種類")]
    private e_ItemType ItemType;
    [SerializeField, Header("アイテムホップアップ")]
    private List<GameObject> ItemHopup;
    private int UseHopUpNum;

    private delegate void Item();
    private Item ItemScript;    // Itemの効果を入れる

    [SerializeField, Header("プレイヤーが回復するHP")]
    private float HealHP;
    [SerializeField, Header("プレイヤーが回復するエサ")]
    private float HealFood;
    [SerializeField, Header("プレイヤーが取得する鍵")]
    private int DropKey;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // ItemTypeによって実行するスクリプトを変化させる
        switch (ItemType) {
            case e_ItemType.HPRecover:
                ItemScript = HPRecover;
                UseHopUpNum = (int)e_ItemType.HPRecover;
                break;

            case e_ItemType.EsaRecover:
                ItemScript = EsaRecover;
                UseHopUpNum = (int)e_ItemType.EsaRecover;
                break;

            case e_ItemType.KeyDrops:
                ItemScript = KeyDrop;
                UseHopUpNum = (int)e_ItemType.KeyDrops;
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
        // ホップアップ作成
        GameObject CreateHopUp = Instantiate(ItemHopup[UseHopUpNum], transform.position, Quaternion.identity);
        //CreateHopUp.GetComponent<LookCamera>().parentTransform = CreateHopUp.transform;
        Destroy(gameObject);
    }

    #region Item効果　基本的にprivate
    // HP回復
    private void HPRecover() 
    {
        if (Player.GetComponent<HumanoidBase>().NowHP > 0) {
            Player.GetComponent<HumanoidBase>().NowHP += HealHP;
            if (Player.GetComponent<HumanoidBase>().InitHP < Player.GetComponent<HumanoidBase>().NowHP) {
                Player.GetComponent<HumanoidBase>().NowHP = Player.GetComponent<HumanoidBase>().InitHP;
            }
        }
    }

    // 餌回復
    private void EsaRecover() 
    {
        Player.GetComponent<Player>().AddFoods(HealFood);
    }

    // 鍵ドロップ
    private void KeyDrop() {
        Player.GetComponent<Player>().KeyCount += DropKey;
    }
    #endregion
}
