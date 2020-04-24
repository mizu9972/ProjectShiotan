using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDig : MonoBehaviour
{
    [SerializeField, Header("描画する数値")]
    private int drawNum;

    public Text textObj;

    [Header("数字の画像")]
    public Sprite[] Number = new Sprite[10];

    [Header("描画状態切り替えを使用するか")]
    public bool isUseDrawChange = false;

    private Image MyImage;
    // Start is called before the first frame update
    void Start()
    {
        MyImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //textObj.text = drawNum.ToString();
        SetTex();//数値に基づいたテクスチャをセット

        //10の位の数値が0なら描画しない
        if (isUseDrawChange && drawNum <= 0)
        {
            MyImage.enabled = false;
        }
        else
        {
            MyImage.enabled = true;
        }
    }

    public void SetDrawNum(int num)//描画する数値をセット
    {
        drawNum = num;
    }

    private void SetTex()
    {
        MyImage.sprite = Number[drawNum];
    }
}
