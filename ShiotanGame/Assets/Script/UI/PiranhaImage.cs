using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PiranhaImage : MonoBehaviour
{
    [Header("ピラニアのアニメーション画像")]
    public Sprite[] AnimImg = new Sprite[3];

    [Header("アニメーションフレーム数")]
    public int AnimFrame = 1;

    [SerializeField]
    private int AnimCnt = 0;//アニメーションカウント数
    [SerializeField]
    private bool isAnim = false;//アニメーション中か
    private Image MyImg;
    [SerializeField]
    private int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        MyImg = this.GetComponent<Image>();
        MyImg.sprite = AnimImg[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnim)
        {
            Animation();
        }
    }

    public void SetisAnim(bool isanim)
    {
        isAnim = isanim;
    }
    private void Animation()
    {
        cnt++;
        if(cnt>=AnimFrame)
        {
            cnt = 0;
            AnimCnt++;
        }
        if(AnimCnt>=3)
        {
            AnimCnt = 0;
            isAnim = false;
        }
        MyImg.sprite = AnimImg[AnimCnt];
    }

    public bool GetisAnim()
    {
        return isAnim;
    }
}
