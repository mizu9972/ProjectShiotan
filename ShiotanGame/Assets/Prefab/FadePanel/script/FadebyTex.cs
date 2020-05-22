using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadebyTex : MonoBehaviour
{
    [SerializeField, Header("時間取得マテリアル")]
    private Material InitMat = null;

    [SerializeField, Header("フェードインマテリアル")]
    private Material FadeInMat = null;

    [SerializeField, Header("フェードアウトマテリアル")]
    private Material FadeOutMat = null;

    [SerializeField, Header("トランジションルール画像")]
    private Texture TransitionTex = null;

    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 5.0f;

    [Header("フェードアウトかどうか")]
    public bool isFadeOut = true;

    [SerializeField, Header("有効無効")]
    private bool isActive = false;

    Action<RenderTexture, RenderTexture> m_FadeFunction;

    private RenderTexture m_SetState = null;
    private Texture2D m_SetInitState = null;
    // Start is called before the first frame update
    void Start()
    {

        m_SetState = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        m_SetInitState = new Texture2D(1, 1);
        m_SetInitState.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        m_SetInitState.Apply();

        if (isActive)
        {
            m_FadeFunction = Fade_InOut_Function;
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

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_FadeFunction.Invoke(source, destination);
    }

    //フェードさせる
    private void Fade_InOut_Function(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, isFadeOut ? FadeOutMat : FadeInMat);
    }

    //何も処理しない
    private void NoFunction(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
    }

    [ContextMenu("FeedInStart")]
    public void StartFadeIn()
    {
        Graphics.Blit(m_SetInitState, m_SetState, InitMat);
        FadeInMat.SetTexture("_InitStateTex", m_SetState);
        FadeInMat.SetFloat("_isActive", 1);

        m_FadeFunction = Fade_InOut_Function;
    }

    [ContextMenu("FeedOutStart")]
    public void StartFadeOut()
    {
        Graphics.Blit(m_SetInitState, m_SetState, InitMat);
        FadeOutMat.SetTexture("_InitStateTex", m_SetState);
        FadeOutMat.SetFloat("_isActive", 1);

        m_FadeFunction = Fade_InOut_Function;
    }
}
