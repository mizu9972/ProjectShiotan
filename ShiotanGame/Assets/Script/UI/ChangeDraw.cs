﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeDraw : MonoBehaviour
{
    [SerializeField, Header("表示状態")]
    private bool isActive = true;

    [SerializeField, Header("子オブジェクトたち")]
    private List<Image> ChildObjects = new List<Image>();

    [SerializeField, Header("ゲージの透明度")]
    private Color col;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform childTransform in this.transform)//子オブジェクトを格納していく
        {
            ChildObjects.Add(childTransform.gameObject.GetComponent<Image>());
        }
    }

    public void ChangeDrawImage(bool sts)//子オブジェクトのImageコンポーネントをオフにする場合
    {
        int cnt = 0;
        isActive = sts;
        foreach(Image childImg in ChildObjects)
        {
            ChildObjects[cnt].enabled = isActive;
            cnt++;
        }
    }

    public void ChangeDrawState(bool sts)//SetActiveから親オブジェクトごとオフにする場合
    {
        isActive = sts;//引数で受け取ったステータスを代入
        this.gameObject.SetActive(isActive);
    }

    private void OnValidate()//インスペクタ上の数値が変更されたら
    {   
        //設定されたアルファ値のみをゲージに設定
        int cnt = 0;
        foreach (Image childImg in ChildObjects)
        {
            ChildObjects[cnt].color = new Color(ChildObjects[cnt].color.r, ChildObjects[cnt].color.g, ChildObjects[cnt].color.b, col.a); 
            cnt++;
        }
    }
}
