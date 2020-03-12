using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要

public class FadeScript : MonoBehaviour
{

    float fadeSpeed = 0.005f;        //透明度が変わるスピードを管理
    float red, green, blue, alfa;   //パネルの色、不透明度を管理

    public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
    public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

    Image fadeImage;                //透明度を変更するパネルのイメージ

    private bool FadeSts = false;//trueでフェード終了 
    void Start()
    {
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    void Update()
    {
        if (isFadeIn)
        {
            FadeSts = StartFadeIn();
        }

        if (isFadeOut)
        {
            FadeSts = StartFadeOut();
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
        alfa = 0.0f;
        isFadeOut = true;
        FadeSts = false;
    }
    public void SetIsFeadIn()//フェードアウトスタートをセット
    {
        alfa = 1.0f;
        isFadeIn = true;
        FadeSts = false;
    }

    public bool GetFeadStatus()
    {
        return FadeSts;
    }
}