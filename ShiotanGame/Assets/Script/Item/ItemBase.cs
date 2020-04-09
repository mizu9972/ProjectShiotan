using System.Collections;
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
        ItemScript.Invoke();
        Destroy(gameObject);
    }

    #region Item効果　基本的にprivate
    // HP回復
    private void HPRecover() 
    {

    }

    // 餌回復
    private void EsaRecover() 
    {

    }
    #endregion
}
