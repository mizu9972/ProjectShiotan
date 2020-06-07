using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class ClearText : MonoBehaviour
{
    [Header("フェードインの秒数")]
    public float FadeInTime = 2f;

    [Header("フェードアウトの秒数")]
    public float FadeOutTime = 2f;

    [Header("表示しておく時間(秒)")]
    public double DrawTime = 2.0;

    [Header("指定するテキスト")]
    public Text MyText;

    private bool isFadeIn = false;//フェードイン状態
    private bool isDraw = false;//描画状態  
    private bool isFadeOut = false;//フェードアウト状態
    private float Alpha = 0f;//アルファ値



    private bool isEnd = false;//一連の動作が終了したか
    [SerializeField]
    private float ElapsedTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Alpha = 0f;//最初は透明状態から
        //描画状態になったら指定秒数後に描画終了させるメソッドの実行
        this.UpdateAsObservable().
            Where(_ => isDraw).Take(1).
            Subscribe(_ => DrawStageName());

    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else if (isFadeOut)
        {
            FadeOut();
        }
        //テキストのカラー値に現在の情報をセット
        MyText.color = new Color(MyText.color.r, MyText.color.g, MyText.color.b, Alpha);
    }

    private void FadeIn()//フェードイン
    {
        ElapsedTime += Time.deltaTime;//経過時間計測

        //経過時間と終了時間の割合をアルファに適用
        Alpha = ElapsedTime / FadeInTime;

        if (Alpha >= 1.0f)//フェードイン終了で描画状態で
        {
            Alpha = 1.0f;
            isFadeIn = false;
            isDraw = true;
            ElapsedTime = 0f;//経過時間リセット
        }
    }

    private void FadeOut()
    {
        ElapsedTime += Time.deltaTime;//経過時間計測

        //経過時間と終了時間の割合をアルファに適用
        Alpha = 1f - (ElapsedTime / FadeInTime);

        if (Alpha <= 0f)
        {
            Alpha = 0.0f;
            isFadeOut = false;
            ElapsedTime = 0f;
            isEnd = true;//一連の動作終了
            isFadeIn = true;
            //描画状態になったら指定秒数後に描画終了させるメソッドの実行
            this.UpdateAsObservable().
                Where(_ => isDraw).Take(1).
                Subscribe(_ => DrawStageName());
        }
    }

    private void DrawStageName()//指定秒数後まで描画しておく
    {
        Observable.Timer(System.TimeSpan.FromSeconds(DrawTime))
            .Take(1).Subscribe(_ => EndDraw());
    }

    private void EndDraw()//描画終了->フェードアウト
    {
        isDraw = false;
        isFadeOut = true;
        ElapsedTime = 0f;
    }

    public bool GetisEnd()//ステージ名描画の処理の状態を返す。
    {
        return isEnd;
    }

    public void StartFade()
    {
        isFadeIn = true;
    }
}
