using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gage : MonoBehaviour
{
    [Header("アニメーション時間")]
    public float AnimationTime;
    RectTransform MyRectTrans;

    [SerializeField]
    float MaxHP;

    [SerializeField]
    float NowHP;

    void Start()
    {
        MyRectTrans = this.GetComponent<RectTransform>();  
    }

    public void InitGage(float Hp)//ヒットポイントの初期化
    {
        MaxHP = Hp;
        NowHP = Hp;
    }

    public void Damage(float Hp)//ダメージ受ける
    {
        NowHP = Hp;//現在のHPを引数から取得
        //現在のHPと最大HPの割合でゲージのアニメーションを行う
        MyRectTrans.DOScale(new Vector3((NowHP/MaxHP),1,1), AnimationTime).SetEase(Ease.Linear);
    }
}
