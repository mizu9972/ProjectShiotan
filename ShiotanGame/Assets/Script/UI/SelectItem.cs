using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectItem : MonoBehaviour
{
    enum MenuState//メニューの状態
    {
        RESTART=0,
        STAGESELECT,
        BACKTITLE,
        GAMEBACK
    };


    [Header("非セレクト状態時のテクスチャ")]
    public Sprite NotSelected;

    [Header("セレクト状態時のテクスチャ")]
    public Sprite Selected;

    [Header("確認オブジェクト")]
    public GameObject ConfirmObj;

    [Header("アンダーバーのオブジェクト")]
    public Image UnderLine;


    private int[] Items = new int[4];

    [SerializeField]
    private int NowSelect = 0;//現在選択中のアイテム

    [Header("選択描画フレーム")]
    public int DrawFlame = 5;

    private Vector3[] LinePos = new Vector3[4];//ポジションの固定値

    private Image MyImage;
    private int AnimCnt = 0;
    private bool isDraw = false;
    // Start is called before the first frame update
    void Start()
    {
        MyImage = this.GetComponent<Image>();
        UnderLine.sprite = NotSelected;//テクスチャを非選択状態に設定
        InitLinePos();//選択状態のラインのポジション初期化
    }

    // Update is called once per frame
    void Update()
    {
        
        KeyInput();//キー入力処理

    }

    void KeyInput()//キー入力関数
    {
        if (!isDraw)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))//上
            {
                NowSelect -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))//下
            {
                NowSelect += 1;
            }
            else if (Input.GetKeyDown(KeyCode.Return))//決定
            {
                UnderLine.sprite = Selected;//テクスチャを選択状態に設定
                isDraw = true;
            }
            NowSelect = Mathf.Clamp(NowSelect, 0, (Items.Length - 1));//選択範囲制限(配列の要素数-1まで)
            UnderLine.transform.localPosition = LinePos[NowSelect];//ポジションセット
        }
        
        if(isDraw)//選択状態のテクスチャ描画状態
        {
            AnimCnt++;
            //選択状態のテクスチャが指定フレーム分描画されたら選択した処理を実行
            if (AnimCnt>=DrawFlame)
            {
                isDraw = false;
                UnderLine.sprite = NotSelected;//テクスチャを非選択状態に設定
                AnimCnt = 0;
                Select(NowSelect);//選択番号によって処理を変更
            }
        }
    }

    private void Select(int num)
    {
        switch(num)//選択番号で描画処理を変更する
        {
            //ゲームに戻る以外は確認画面へ移行
            case (int)MenuState.GAMEBACK:
                //TODO ゲームマネージャにスクリプト全てを有効化する関数を実装
                GameManager.Instance.SetActivePause(false);//ポーズ画面の終了
                break;
            default:
                ConfirmObj.SetActive(true);
                ConfirmObj.GetComponent<ConfirmUI>().SetState(num);
                this.gameObject.SetActive(false);
                break;
        }
    }

    private void InitLinePos()
    {
        LinePos[0] = new Vector3(0, 45, 0);
        LinePos[1] = new Vector3(0, -30, 0);
        LinePos[2] = new Vector3(0, -105, 0);
        LinePos[3] = new Vector3(0, -180, 0);
    }

    public void SetState(int sts)
    {
        NowSelect = sts;
    }
}
