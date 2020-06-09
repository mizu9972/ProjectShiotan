using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;
using UniRx.Triggers;

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

    [SerializeField, Header("フェードイン速度")]
    private float FadeInSpeed = 1.0f;

    [SerializeField, Header("フェードアウト速度")]
    private float FadeOutSpeed = 1.0f;
    [SerializeField, Header("シーン開始時にいきなり処理するか")]
    private bool isActive = false;
    [SerializeField, Header("いきなり処理する場合、フェードインかどうか")]
    private bool isFadeIn = false;

    private bool isFade = false;//フェード中かを管理する
    //private float m_FunctionTimeCount = 0;//処理時間カウント用
    ReactiveProperty<float> m_FunctionTimeCount = new ReactiveProperty<float>();
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

        FadeOutMat.SetTexture("_TransitionTex", TransitionTex);

        //フェード終了通知
        m_FunctionTimeCount.Select(x => x * 0.01f >= 1.0).DistinctUntilChanged().Where(x => x).Subscribe(x =>
        {
            OnEndFadeInOut();
        });

    }

    //描画時処理
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_FadeFunction.Invoke(source, destination);
    }

    //フェードさせる
    private void FadeIn_Function(RenderTexture source, RenderTexture destination)
    {
        FadeInMat.SetFloat("_TimeCount", m_FunctionTimeCount.Value);
        Graphics.Blit(source, destination, FadeInMat);
        m_FunctionTimeCount.Value += FadeInSpeed;
    }

    private void FadeOut_Function(RenderTexture source, RenderTexture destination)
    {
        FadeOutMat.SetFloat("_TimeCount", m_FunctionTimeCount.Value);
        Graphics.Blit(source, destination, FadeOutMat);
        m_FunctionTimeCount.Value += FadeOutSpeed;
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
        m_FunctionTimeCount.Value = 0;

        ActiveMaterial = FadeInMat;
        m_FadeFunction = FadeIn_Function;

        isFade = true;
        GameManager.Instance.SetisFade(true);
        GameManager.Instance.SetPauseEnable(false);
    }

    //フェードアウト
    [ContextMenu("FeedOutStart")]
    public void StartFadeOut()
    {
        FadeOutMat.SetFloat("_isActive", 1);
        m_FunctionTimeCount.Value = 0;

        ActiveMaterial = FadeOutMat;
        m_FadeFunction = FadeOut_Function;

        isFade = true;
        GameManager.Instance.SetisFade(true);
        GameManager.Instance.SetPauseEnable(false);
    }

    //フェード終了時処理
    public void OnEndFadeInOut()
    {
        //Debug.Log("フェード終わり");
        if(isFadeIn)
        {
            GameManager.Instance.SetPauseEnable(true);//ポーズ画面の仕様を可能に
            GameManager.Instance.PlayerControlStart();//プレイヤーがいれば操作可能に
        }

        isFade = false;//フェード終了
        GameManager.Instance.SetisFade(false);
    }

    //現在フェード中かを返す
    public bool GetisFade()
    {
        return isFade;
    }
}
