using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIScript : MonoBehaviour
{
    //HPUI　で使用
    private int MAXHP;
    private int HPnumber = 0;
    private int BeforeHP = 0;

    [Header("プレイヤーのステータス")]
    public Status PlayerStatus;

    [Header("HP　リスト")]
    public List<GameObject> HeartDrawNum;

    [Header("コピーするハートの画像")]
    public GameObject copy;

    [Header("canvas"), SerializeField]
    GameObject canvas;

    [Header("ハート隙間　減らす量")]
    public float DawnWidth;

    // Start is called before the first frame update
    void Start()
    {
        //HP取得
        MAXHP = PlayerStatus.GetMAXHP();
        HPnumber = MAXHP;
        BeforeHP = HPnumber;

        //MAXHPまでハート画像　複製
        for (int i = 0; i < MAXHP-1; i++)
        {
            //ハート生成
            GameObject prefab = (GameObject)Instantiate(copy);

            //canvasと親子関係
            prefab.transform.SetParent(canvas.transform, false);

            //ハート　位置設定
            Vector3 pos;
            RectTransform save = HeartDrawNum[i].GetComponent<RectTransform>();
            pos.x = save.localPosition.x + save.sizeDelta.x - DawnWidth;
            pos.y = save.localPosition.y;
            pos.z = 0;

            //ハート　位置反映
            prefab.GetComponent<RectTransform>().localPosition = pos;

            //リストに生成したハートデータ　追加　
            HeartDrawNum.Add(prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HPnumber = PlayerStatus.GetHP();

        //HP減少
        if (HPnumber<BeforeHP)
        {
            while (BeforeHP != HPnumber)
            {
                BeforeHP--;
                if (BeforeHP < 0)
                {
                    BeforeHP = 0;
                    Debug.Log("break");
                    break;
                }
                else
                {
                    //ハートUI　非表示化
                    HeartDrawNum[BeforeHP].SetActive(false);
                }
            }
        }

        //HP増加
        if(HPnumber>BeforeHP)
        {
            while (BeforeHP != HPnumber)
            {
                BeforeHP++;
                if (BeforeHP > MAXHP)
                {
                    BeforeHP = MAXHP;
                    Debug.Log("break");
                    break;
                }
                else
                {
                    //ハートUI　表示化
                    HeartDrawNum[BeforeHP - 1].SetActive(true);
                }
            }
        }
    }
}
