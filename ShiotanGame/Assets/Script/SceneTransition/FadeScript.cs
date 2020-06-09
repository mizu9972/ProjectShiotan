using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
public class FadeScript : MonoBehaviour
{

    float fadeSpeed = 0.016f;        //透明度が変わるスピードを管理
    float red, green, blue, alfa;   //パネルの色、不透明度を管理

    public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
    public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

    Image fadeImage;                //透明度を変更するパネルのイメージ

    private bool FadeOutSts = false;//trueでフェード終了 
    private bool FadeInSts = false;//trueでフェード終了 
    
    void Start()
    {
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += SceneLoaded;
        SceneLoadFead();
    }

    void Update()
    {
        if (isFadeIn)
        {
            FadeInSts = StartFadeIn();
        }

        if (isFadeOut)
        {
            FadeOutSts = StartFadeOut();
        }
    }

    bool StartFadeIn()
    {
        alfa -= Mathf.Sin(fadeSpeed);               //a)不透明度を徐々に下げる
        SetAlpha();                      //b)変更した不透明度パネルに反映する
        if (alfa <= 0)
        {                    //c)完全に透明になったら処理を抜ける
            isFadeIn = false;
            fadeImage.enabled = false;    //d)パネルの表示をオフにする
            GameManager.Instance.SetisFade(false);
            return true;
        }
        return false;
    }

    bool StartFadeOut()
    {
        fadeImage.enabled = true;  // a)パネルの表示をオンにする
        alfa += Mathf.Sin(fadeSpeed);         // b)不透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (alfa >= 1)
        {             // d)完全に不透明になったら処理を抜ける
            isFadeOut = false;
            GameManager.Instance.SetisFade(false);
            return true;
        }
        return false;
    }

    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }


    public void SetIsFeadOut()//フェードアウトスタートをセット
    {
        GameManager.Instance.SetisFade(true);
        alfa = 0.0f;
        isFadeOut = true;
        FadeOutSts = false;
        GameManager.Instance.SetPauseEnable(false);//ポーズ画面の使用を不可能に
        GameManager.Instance.PlayerControlStop();//プレイヤーがいれば操作可能に

    }
    public void SetIsFeadIn()//フェードアウトスタートをセット
    {
        GameManager.Instance.SetisFade(true);
        alfa = 1.0f;
        isFadeIn = true;
        FadeInSts = false;
        GameManager.Instance.SetPauseEnable(false);//ポーズ画面の使用を不可能に
        GameManager.Instance.PlayerControlStop();//プレイヤーがいれば操作可能に
    }

    public bool GetFeadOutStatus()
    {
        return FadeOutSts;
    }

    public bool GetFeadInStatus()
    {
        return FadeInSts;
    }

    private void ResetFlag()//全てのフラグをリセット
    {
        isFadeOut = false;
        isFadeIn = false;
        FadeOutSts = false;//trueでフェード終了 
        FadeInSts = false;//trueでフェード終了 
    }

    // イベントハンドラー（イベント発生時に動かしたい処理）
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)//シーンが切り替わった時の処理
    {
        this.UpdateAsObservable().Take(1).Subscribe(_ => SceneLoadFead());
    }

    private void SceneLoadFead()
    {

        if (!GameManager.Instance.GetisStage())
        {
            SetIsFeadIn();//シーンスタート時にフェードインを実行
                          //フェードイン終了後に全てのフラグをリセット
            this.UpdateAsObservable().
                Where(_ => !GameManager.Instance.GetisFade()).Take(1).
                Subscribe(_ => ResetFlag());
        }
        else
        {
            ResetFlag();
            alfa = 0f;
            SetAlpha();
            GameManager.Instance.SetPauseEnable(false);//ポーズ画面の使用を可能に
        }
    }

    public void SetPanelAlpha(float alpha)
    {
        alfa = alpha;
        SetAlpha();
    }
}