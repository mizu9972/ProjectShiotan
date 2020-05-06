using System.Collections;
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

    private Image NowSelectObj;//現在選択中のオブジェクト
    // Start is called before the first frame update
    void Start()
    {
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
        if(Input.GetKeyDown(KeyCode.LeftArrow))//ひとつ前のオブジェクトを選択
        {
            if(NowSelectObj.GetComponent<StageImage>().GetPrevExist())//一つ前にオブジェクトがあれば
            {
                //非選択状態へ
                NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                //選択オブジェクトの変更
                NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoPerv();
                NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
            }
            
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))//一つ後にオブジェクトがあれば
        {
            if(NowSelectObj.GetComponent<StageImage>().GetNextExist())
            {
                //非選択状態へ
                NowSelectObj.GetComponent<StageImage>().SetisSelect(false);
                NowSelectObj.GetComponent<StageImage>().SetSize(NotSelectedSize);
                //選択オブジェクトの変更
                NowSelectObj = NowSelectObj.GetComponent<StageImage>().GoNext();
                NowSelectObj.GetComponent<StageImage>().SetisSelect(true);
                NowSelectObj.GetComponent<StageImage>().SetSize(SelectedSize);
            }   
        }
        if(Input.GetKeyDown(KeyCode.Return))//ステージを決定
        {
            NowSelectObj.GetComponent<StageImage>().SelectStage();
        }
    }

    [ContextMenu("間隔をセット")]
    private void  SetSpace()
    {
        this.GetComponent<HorizontalLayoutGroup>().spacing = Space;//並べる間隔をセット
    }
}
