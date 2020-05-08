using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleConfirm : MonoBehaviour
{
    public enum MenuState//メニューの状態
    {
        START = 0,
        STAGESELECT,
        END,
    };

    InputStick inputStick;


    [Header("アンダーバーのオブジェクト")]
    public Image UnderLine;

    [Header("非セレクト状態時のテクスチャ")]
    public Sprite NotSelected;

    [Header("セレクト状態時のテクスチャ")]
    public Sprite Selected;

    [Header("メインメニューオブジェクト")]
    public GameObject MainMenuObj;

    [SerializeField]
    private int NowSelect = 0;//現在選択中のアイテム

    private int[] Items = new int[2];//選択項目

    private Vector3[] LinePos = new Vector3[2];//ポジションの固定値

    [SerializeField]
    private int State = -1;//選択されている状態

    [Header("選択描画フレーム")]
    public int DrawFlame = 5;

    private Image MyImage;
    private int AnimCnt = 0;
    private bool isDraw = false;
    // Start is called before the first frame update
    void Start()
    {
        inputStick = new InputStick();
        MyImage = this.GetComponent<Image>();
        UnderLine.sprite = NotSelected;//テクスチャを非選択状態に設定
        InitLinePos();//選択状態のラインのポジション初期化
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
    }

    private void OnEnable()
    {
        NowSelect = 0;
    }

    private void KeyInput()
    {
        if (!isDraw)
        {
            inputStick.StickUpdate();
            if (Input.GetKeyDown(KeyCode.UpArrow) || inputStick.GetUpStick())//上
            {
                NowSelect -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || inputStick.GetDownStick())//下
            {
                NowSelect += 1;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("MenuSelect"))//決定
            {
                UnderLine.sprite = Selected;//テクスチャを選択状態に設定
                isDraw = true;
            }
            NowSelect = Mathf.Clamp(NowSelect, 0, (Items.Length - 1));//選択範囲制限(配列の要素数-1まで)
            UnderLine.transform.localPosition = LinePos[NowSelect];//ポジションセット
        }

        if (isDraw)//選択状態のテクスチャ描画状態
        {
            AnimCnt++;
            //選択状態のテクスチャが指定フレーム分描画されたら選択した処理を実行
            if (AnimCnt >= DrawFlame)
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
        if (num == 0)//はいが選択されれば
        {
            switch (State)//選択番号で描画処理を変更する
            {
                case (int)MenuState.END:
                    GameManager.Instance.Quit();//終了
                    break;

                default:
                    break;
            }
        }

        else//いいえが選択されれば
        {
            MainMenuObj.GetComponent<TitleScene>().SetState(State);//最後に選択した状態からスタート
            this.gameObject.SetActive(false);
            MainMenuObj.SetActive(true);
        }
    }

    private void InitLinePos()
    {
        LinePos[0] = new Vector3(0, -30, 0);
        LinePos[1] = new Vector3(0, -80, 0);
    }

    public void SetState(int sts)//状態をセット
    {
        State = sts;
    }


    
}
