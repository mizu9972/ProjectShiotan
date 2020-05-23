using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//トランシジョンルールテクスチャの情報をもとにフェードイン・アウト合成を行うカメラ用スクリプト
public class FadebyTex : MonoBehaviour
{

    [SerializeField, Header("フェードインマテリアル")]
    private Material FadeInMat = null;

    [SerializeField, Header("フェードアウトマテリアル")]
    private Material FadeOutMat = null;

    private Material ActiveMaterial = null;

    [SerializeField, Header("トランジションルール画像")]
    private Texture TransitionTex = null;

    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 1.0f;

    [SerializeField, Header("シーン開始時にいきなり処理するか")]
    private bool isActive = false;
    [SerializeField, Header("いきなり処理する場合、フェードインかどうか")]
    private bool isFadeIn = false;

    private float m_FunctionTimeCount = 0;//処理時間カウント用

    Action<RenderTexture, RenderTexture> m_FadeFunction;//処理切り替え用
    
    // Start is called before the first frame update
    void Start()
    {
        //シーン開始時にいきなり処理するか
        if (isActive)
        {
            //フェードインアウト判定
            if (isFadeIn)
            {
                StartFadeIn();
            }
            else
            {
                StartFadeOut();
            }
        }
        else
        {
            m_FadeFunction = NoFunction;
        }

        //シェーダー初期化
        FadeInMat.SetTexture("_TransitionTex", TransitionTex);
        FadeInMat.SetFloat("_FadeSpeed", FadeSpeed);

        FadeOutMat.SetTexture("_TransitionTex", TransitionTex);
        FadeOutMat.SetFloat("_FadeSpeed", FadeSpeed);
    }

    //描画時処理
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_FadeFunction.Invoke(source, destination);
    }

    //フェードさせる
    private void Fade_InOut_Function(RenderTexture source, RenderTexture destination)
    {
        ActiveMaterial.SetFloat("_TimeCount", m_FunctionTimeCount);
        Graphics.Blit(source, destination, ActiveMaterial);
        m_FunctionTimeCount += FadeSpeed;
    }

    //描画するだけ(フェードインアウト処理を行わない)
    private void NoFunction(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
    }

    //フェードイン
    [ContextMenu("FeedInStart")]
    public void StartFadeIn()
    {
        FadeInMat.SetFloat("_isActive", 1);
        m_FunctionTimeCount = 0;

        ActiveMaterial = FadeInMat;
        m_FadeFunction = Fade_InOut_Function;
    }

    //フェードアウト
    [ContextMenu("FeedOutStart")]
    public void StartFadeOut()
    {
        FadeOutMat.SetFloat("_isActive", 1);
        m_FunctionTimeCount = 0;

        ActiveMaterial = FadeOutMat;
        m_FadeFunction = Fade_InOut_Function;
    }
}
