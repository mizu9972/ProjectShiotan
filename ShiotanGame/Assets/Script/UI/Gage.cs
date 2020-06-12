using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
public class Gage : MonoBehaviour
{
    [Header("アニメーション時間")]
    public float AnimationTime;
    RectTransform MyRectTrans;

    [SerializeField]
    float MaxHP;

    [SerializeField]
    float NowHP;

    [Header("HPゲージの色を変えるHP割合ライン")]
    public float DangerZone = 0.2f;

    [Header("十分にHPがあるときのバー")]
    public Sprite GreenBar;

    [Header("危険域のバー")]
    public Sprite RedBar;

    [Header("ピラニアの画像")]
    public Image PiranhaImg = null;

    private Image MyImg;
    private Color MyCol;
    private bool isAnimation = false;
    void Start()
    {
        MyRectTrans = this.GetComponent<RectTransform>();
        MyImg = this.GetComponent<Image>();
        MyCol = this.GetComponent<Image>().color;

        if (PiranhaImg)
        {
            isAnimation = PiranhaImg.GetComponent<PiranhaImage>().GetisAnim();
            //HPが変動した時かつアニメーション中じゃなければアニメーション開始
            this.UpdateAsObservable().
                Subscribe(_ => AddPiranhaAnimFunc());
        }
    }

    public void InitGage(float Hp)//ヒットポイントの初期化
    {
        MaxHP = Hp;
        NowHP = Hp;
    }

    public void GageUpdate(float Hp)//ダメージ受ける
    {
        
        NowHP = Hp;//現在のHPを引数から取得

        if (NowHP<=1)//HPが1以下ならNowHPを1に
        {
            NowHP = 1f;
            MyImg.color = new Color(MyCol.r, MyCol.g, MyCol.b, 0f);//透明にして消滅させる
        }
        else
        {
            MyImg.color = new Color(MyCol.r, MyCol.g, MyCol.b, 1f);//HPが存在するなら表示状態に
        }
        //現在のHPと最大HPの割合でゲージのアニメーションを行う
        MyRectTrans.DOScale(new Vector3((NowHP/MaxHP),1,1), AnimationTime).SetEase(Ease.Linear);
        ChangeTexture();
    }

    private void ChangeTexture()//HPの割合でテクスチャを変更
    {
        if ((NowHP / MaxHP) <= DangerZone)
        {
            MyImg.sprite = RedBar;
        }
        else
        {
            MyImg.sprite = GreenBar;
        }
    }

    private void PiranhaAnimationStart()
    {
        if(!isAnimation)
        {
            PiranhaImg.GetComponent<PiranhaImage>().SetisAnim(true);
        }
    }

    public float GetProportion()
    {
        float proportion = NowHP / MaxHP;
        return proportion;
    }

    private void AddPiranhaAnimFunc()
    {
        //HPが変動した時かつアニメーション中じゃなければアニメーション開始
        this.UpdateAsObservable().
            Select(x => NowHP).
            DistinctUntilChanged(x => x).Skip(1).
            Subscribe(_ => PiranhaAnimationStart());
    }
}
