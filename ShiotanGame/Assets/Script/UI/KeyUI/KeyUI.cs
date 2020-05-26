using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    private int KeyCount = 0;
    [Header("鍵の数字イメージ")]
    public Image KeyNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        KeyCount = GameManager.Instance.GetPlayer().GetComponent<Player>().KeyCount;
        KeyNumber.GetComponent<DrawDig>().SetDrawNum(KeyCount);//数字を描画クラスにセット
    }

    public void AddKey(int val)//キー加算
    {
        KeyCount += val;
    }

    public void SubKey(int val)//キー減算
    {
        KeyCount -= val;
    }

    public void SetKeyCount(int val)//キーに数値をセット
    {
        KeyCount = val;
    }
}
