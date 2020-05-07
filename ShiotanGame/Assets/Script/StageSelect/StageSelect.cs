﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelect : MonoBehaviour
{
    [Header("オブジェクトの間隔")]
    public float Space;

    [Header("選択オブジェクトのサイズ")]
    public Vector2 SelectedSize;

    [Header("非選択オブジェクトのサイズ")]
    public Vector2 NotSelectedSize;

    [Header("最初に選択状態になるステージ")]
    public Image FirstSelectStage;

    [Header("一回で動く量")]
    public float MoveDistance;

    [Header("線形補間のスピード")]
    public float Spead = 0.1f;

    [Header("長押しカウントのマックス値")]
    public float LongPushMax = 30.0f;
    private float work_LongPushMax;

    private float NowXPos;


    private Image NowSelectObj;//現在選択中のオブジェクト



    private RectTransform MyRectTrans;

    private float MoveTimer = 0f;//Lerpに使用するタイマー

    private bool isMoveLeft = false;//左右移動判定フラグ
    private bool isMoveRight = false;

    private float LongPushTime = 0f;//長押しタイマー
    private bool isLeftPushing = false;
    private bool isRightPushing = false;
    // Start is called before the first frame update
    void Start()
    {
        work_LongPushMax = LongPushMax;
        MyRectTrans = this.GetComponent<RectTransform>();
        NowXPos = MyRectTrans.localPosition.x;
        this.GetComponent<HorizontalLayoutGroup>().spacing = Space;//並べる間隔をセット
        NowSelectObj = FirstSelectStage;
        FirstSelectStage.GetComponent<StageImage>().SetisSelect(true);
        
        var childObject = this.GetComponentsInChildren<RectTransform>();
        foreach(var item in childObject)//すべての子オブジェクトをデフォルトサイズに
        {
            item.sizeDelta = NotSelectedSize;
        }
        FirstSelectStage.GetComponent<StageImage>().SetSize(SelectedSize);//最初に選択するオブジェクトのサイズを大きく
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
    }

    private void KeyInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)&&!isMoveRight)//ひとつ前のオブジェクトを選択
        {
            isLeftPushing = true;
            if(NowSelectObj.GetComponent<StageImage>().GetPrevExist())//一つ前にオブジェクトがあれば
            {
                //非選択状態へ
                NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                //選択オブジェクトの変更
                NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoPerv();
                NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                //NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
                //オブジェクトの移動
                MoveTimer = 0f;
                NowXPos += MoveDistance;
                isMoveLeft = true;
            }
        }
        


        if(Input.GetKeyDown(KeyCode.RightArrow)&&!isMoveLeft)//一つ後にオブジェクトがあれば
        {
            isRightPushing = true;
            if(NowSelectObj.GetComponent<StageImage>().GetNextExist())
            {
                //非選択状態へ
                NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                //選択オブジェクトの変更
                NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoNext();
                NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                //NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
                //オブジェクトの移動
                MoveTimer = 0f;
                NowXPos -= MoveDistance;
                isMoveRight = true;
            }   
        }

        if (isMoveLeft)
        {
            MoveLeft();
        }
        else if (isMoveRight)
        {
            MoveRight();
        }


        if(isLeftPushing)
        {
            LongPushTime += 1.0f;
            if(LongPushTime>=LongPushMax&& !isMoveLeft)
            {
                if (NowSelectObj.GetComponent<StageImage>().GetPrevExist())//一つ前にオブジェクトがあれば
                {
                    //非選択状態へ
                    NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                    NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                    //選択オブジェクトの変更
                    NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoPerv();
                    NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                    //NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
                    //オブジェクトの移動
                    MoveTimer = 0f;
                    NowXPos += MoveDistance;
                    isMoveLeft = true;
                    LongPushMax /= 2.0f;
                    CountReset();
                }
            }
        }
        else if(isRightPushing)
        {
            LongPushTime += 1.0f;
            if (LongPushTime >= LongPushMax&& !isMoveRight)
            {
                if (NowSelectObj.GetComponent<StageImage>().GetNextExist())
                {
                    //非選択状態へ
                    NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                    NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                    //選択オブジェクトの変更
                    NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoNext();
                    NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                    //NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
                    //オブジェクトの移動
                    MoveTimer = 0f;
                    NowXPos -= MoveDistance;
                    isMoveRight = true;
                    LongPushMax /= 2.0f;
                    CountReset();
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeftPushing = false;
            CountReset();
            LongPushMax = work_LongPushMax;
        }
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRightPushing = false;
            CountReset();
            LongPushMax = work_LongPushMax;
        }

        if (Input.GetKeyDown(KeyCode.Return))//ステージを決定
        {
            NowSelectObj.GetComponent<StageImage>().SelectStage();
        }
    }

    [ContextMenu("間隔をセット")]
    private void  SetSpace()
    {
        MyRectTrans = this.GetComponent<RectTransform>();
        NowXPos = MyRectTrans.localPosition.x;
        this.GetComponent<HorizontalLayoutGroup>().spacing = Space;//並べる間隔をセット
        NowSelectObj = FirstSelectStage;
        FirstSelectStage.GetComponent<StageImage>().SetisSelect(true);

        var childObject = this.GetComponentsInChildren<RectTransform>();
        foreach (var item in childObject)//すべての子オブジェクトをデフォルトサイズに
        {
            item.sizeDelta = NotSelectedSize;
        }
    }

    private void MoveLeft()//左へ移動
    {
        Vector3 moveposition= new Vector3(NowXPos, -75.0f, 0f);
        Vector2 SetSize = SelectedSize;
        SetSize = Vector2.Lerp(NotSelectedSize, SelectedSize, MoveTimer);
        NowSelectObj.GetComponent<StageImage>().SetSize(SetSize);
        MyRectTrans.localPosition = Vector3.Lerp(MyRectTrans.localPosition, moveposition, MoveTimer);
        MoveTimer += Spead;
        if (MoveTimer>=1.0f)
        {
            isMoveLeft = false;
        }
        //MyRectTrans.localPosition = new Vector3(NowXPos, -75.0f, 0f);
    }

    private void MoveRight()//右へ移動
    {
        Vector3 moveposition = new Vector3(NowXPos, -75.0f, 0f);
        Vector2 SetSize = SelectedSize;
        SetSize = Vector2.Lerp(NotSelectedSize, SelectedSize, MoveTimer);
        NowSelectObj.GetComponent<StageImage>().SetSize(SetSize);
        MyRectTrans.localPosition = Vector3.Lerp(MyRectTrans.localPosition, moveposition, MoveTimer);
        MoveTimer += Spead;
        if (MoveTimer >= 1.0f)
        {
            isMoveRight = false;
        }
    }

    

    private void CountReset()
    {
        LongPushTime = 0.0f;
    }
}