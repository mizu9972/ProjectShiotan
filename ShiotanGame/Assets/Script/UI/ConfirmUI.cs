using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class ConfirmUI : MonoBehaviour
{
    enum MenuState//メニューの状態
    {
        RESTART = 0,
        STAGESELECT,
        BACKTITLE
    };

    InputStick inputStick;


    [Header("アンダーバーのオブジェクト")]
    public Image UnderLine;

    [Header("ポーズ画面の親オブジェクト")]
    public GameObject PauseParent;

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
            if (Input.GetButtonDown("Pause"))
            {
                this.gameObject.SetActive(false);
                MainMenuObj.SetActive(true);
                AudioManager.Instance.PlaySE("SE_CANCEL");
                GameManager.Instance.SetActivePause(false);//ポーズ画面の終了
            }
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
        if(num==0)//はいが選択されれば
        {
            switch (State)//選択番号で描画処理を変更する
            {
                case (int)MenuState.RESTART://リスタート
                    GameManager.Instance.SceneReload(true);
                    this.gameObject.SetActive(false);
                    AudioManager.Instance.PlaySE("SE_ENTER");
                    GameManager.Instance.SetActivePause(false);//ポーズ画面の描画を終了
                    break;

                case (int)MenuState.STAGESELECT://ステージセレクトへ
                    this.gameObject.SetActive(false);
                    Camera.main.GetComponent<FadebyTex>().StartFadeOut();
                    ReservSceneLoad("StageSelect");
                    AudioManager.Instance.PlaySE("SE_ENTER");
                    GameManager.Instance.StageCountReset();
                    GameManager.Instance.SetActivePause(false);//ポーズ画面の描画を終了
                    break;

                case (int)MenuState.BACKTITLE://タイトルへ
                    Camera.main.GetComponent<FadebyTex>().StartFadeOut();
                    ReservSceneLoad("TitleScene");
                    this.gameObject.SetActive(false);
                    AudioManager.Instance.PlaySE("SE_ENTER");
                    GameManager.Instance.StageCountReset();
                    GameManager.Instance.SetActivePause(false);//ポーズ画面の描画を終了
                    break;
            }
        }
        
        else//いいえが選択されれば
        {
            MainMenuObj.GetComponent<SelectItem>().SetState(State);//最後に選択した状態からスタート
            this.gameObject.SetActive(false);
            MainMenuObj.SetActive(true);
            AudioManager.Instance.PlaySE("SE_CANCEL");
        }
    }

    private void InitLinePos()
    {
        LinePos[0] = new Vector3(15, -50, 0);
        LinePos[1] = new Vector3(15, -125, 0);
    }

    public void SetState(int sts)//状態をセット
    {
        State = sts;
    }

    private void ReservSceneLoad(string NextScene)//シーン遷移を予約
    {
        Camera.main.UpdateAsObservable().
            Where(_ => !GameManager.Instance.GetisFade()).Take(1).
            Subscribe(_ => ReservFunc(NextScene));
    }

    private void ReservFunc(string NextScene)
    {
        GameManager.Instance.SetPanelAlpha(1f);
        GameManager.Instance.SceneTransition(NextScene);
    }
}
