using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleScene : MonoBehaviour
{
    public enum MenuState//メニューの状態
    {
        START = 0,
        STAGESELECT,
        END,
    };
    InputStick inputStick;

    [Header("非セレクト状態時のテクスチャ")]
    public Sprite NotSelected;

    [Header("セレクト状態時のテクスチャ")]
    public Sprite Selected;

    [Header("確認オブジェクト")]
    public GameObject ConfirmObj;

    [Header("アンダーバーのオブジェクト")]
    public Image UnderLine;

    private int[] Items = new int[3];

    [SerializeField]
    private int NowSelect = 0;//現在選択中のアイテム

    [Header("選択描画フレーム")]
    public int DrawFlame = 5;

    private Vector3[] LinePos = new Vector3[3];//ポジションの固定値

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
        KeyInput();//キー入力処理
    }

    private void KeyInput()//キー入力関数
    {
        if (!isDraw)
        {
            inputStick.StickUpdate();
            if (Input.GetKeyDown(KeyCode.UpArrow) || inputStick.GetUpStick())//上
            {
                NowSelect -= 1;
                if (NowSelect >= 0)
                {
                    AudioManager.Instance.PlaySE("SE_SHIFT");
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || inputStick.GetDownStick())//下
            {
                NowSelect += 1;
                if (NowSelect <= (Items.Length - 1))
                {
                    AudioManager.Instance.PlaySE("SE_SHIFT");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("MenuSelect"))//決定
            {
                UnderLine.sprite = Selected;//テクスチャを選択状態に設定
                AudioManager.Instance.PlaySE("SE_ENTER");
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
        switch (num)//選択番号で描画処理を変更する
        {
            case (int)MenuState.START:
                Camera.main.GetComponent<SceneTransition>().SetTransitionRun("stage1");
                this.GetComponent<TitleScene>().enabled = false;
                break;

            case (int)MenuState.STAGESELECT:
                Camera.main.GetComponent<SceneTransition>().SetTransitionRun("MenuScene");
                this.GetComponent<TitleScene>().enabled = false;
                break;

            //ゲーム終了は確認画面へ移行
            case (int)MenuState.END:
                ConfirmObj.SetActive(true);
                ConfirmObj.GetComponent<TitleConfirm>().SetState(num);
                this.gameObject.SetActive(false);
                break;

        }
    }

    private void InitLinePos()
    {
        LinePos[0] = new Vector3(10, 30, 0);
        LinePos[1] = new Vector3(10, -20, 0);
        LinePos[2] = new Vector3(10, -70, 0);
    }

    public void SetState(int sts)
    {
        NowSelect = sts;
    }
    //SceneTransition TransitionScript;
    //[Header("遷移先シーン名")]
    //public string NextSceneName = null;
    //private bool TransitionFlg = false;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    TransitionScript = this.GetComponent<SceneTransition>();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if (Input.anyKeyDown && !TransitionFlg)
    //    {
    //        AudioManager.Instance.PlaySE("SE_ENTER");//決定音
    //        TransitionScript.SetTransitionRun(NextSceneName);
    //        TransitionFlg = true;
    //    }
    //}
}
