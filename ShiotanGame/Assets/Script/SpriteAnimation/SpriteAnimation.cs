using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteAnimation : MonoBehaviour
{
    [Header("アニメーション画像")]
    public Sprite[] Sprites = new Sprite[2];

    [Header("1枚あたりの描画フレーム")]
    public float AnimFrame = 3.0f;

    [Header("アニメーションするか")]
    public bool isAnim = true;

    private int ArraySize;
    private int AnimCnt = 0;
    private int ArrayNum = 0;
    
    private Image MyImg;
    // Start is called before the first frame update
    void Start()
    {
        if (!Sprites[0])
        {
            this.enabled = false;
        }
        else
        {
            ArraySize = Sprites.Length;
            MyImg = this.GetComponent<Image>();
            ArrayNum = 0;
            MyImg.sprite = Sprites[ArrayNum];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isAnim)
        {
            Animation();
        }
        else
        {
            if(MyImg.sprite!=Sprites[0])
            {
                ArrayNum = 0;
                MyImg.sprite = Sprites[ArrayNum];
            }
        }
    }

    void Animation()
    {
        //規定フレーム数描画が終われば次のスプライトへ移行
        AnimCnt++;
        if(AnimCnt>=AnimFrame)
        {
            ArrayNum++;
            if (ArrayNum >= ArraySize)
            {
                ArrayNum = 0;//配列のサイズをオーバーしたら1枚目の画像へ
            }
            MyImg.sprite = Sprites[ArrayNum];
            AnimCnt = 0;
        }
    }
    public void SetisAnim(bool isanim)
    {
        isAnim = isanim;
    }
}
