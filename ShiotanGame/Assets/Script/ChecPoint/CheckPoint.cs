using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheckPoint : MonoBehaviour
{
    // <使い方>
    // 1.インスペクタ上の歯車から配列の初期化を実行
    // 2.チェックポイントをステージの手前側のオブジェクトから入れていく
    [Header("チェックポイントの数")]
    public uint CheckPointNum;

    [Header("チェックポイント")]
    public GameObject[] CheckPointList;

    [Header("リスポーン地点オブジェクト")]
    public GameObject RespawnObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckList();
    }

    [ContextMenu("配列の初期化")]
    private void CheckPointInit()
    {
        CheckPointList = new GameObject[CheckPointNum];
    }

    public GameObject GetRespawnObj()
    {
        return RespawnObj;
    }

    private void CheckList()//リストをチェックしてチェックポイントを通過していればリスポーン地点を変更
    {
        for (int cnt = 0; cnt < CheckPointNum; cnt++)
        {
            if (CheckPointList[cnt].GetComponent<CheckPointChild>().GetisChecked())
            {
                CheckPointList[cnt].GetComponent<CheckPointChild>().SetRespawnPosition();//リスポーン地点移動
                CheckPointList[cnt].SetActive(false);//オブジェクトを無効化
                //自分より手前のチェックポイントを全て無効化(チェックポイントを戻されないように)
                for (int element = cnt-1; element>=0;element--)
                {
                    CheckPointList[element].SetActive(false);
                }
            }
        }

    }
}
