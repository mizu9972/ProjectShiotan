using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class NumberScript : MonoBehaviour
{
    [Header("数　表示画像")]
    public RawImage SetNumber;

    [Header("桁数")]
    public int truss;

    void Start()
    {
        float x, y, w, h = 0;
        
        x = 0.0f;
        y = 0.5f;
        w = 0.2f;
        h = 0.5f;

        //0　表示
        SetNumber.uvRect = new Rect(x, y, w, h);
    }

    void Update()
    {

    }

    public void SetNumberDraw(int Number_num)
    {
        float x=0, y=0, w=0, h=0;

        if (truss == 0)
        {
            //2桁目　計算に含まない
            Number_num %= 10;

            //数値のUVのX座標
            x = Number_num % 5.0f / 5.0f;

            //数値のUVのY座標
            if (Number_num < 5)
            {
                y = 0.5f;
            }
            if (Number_num >= 5)
            {
                y = 0.0f;
            }
        }

        if(truss==10)
        {
            Number_num = Number_num / 10;

            //数値のUVのX座標
            x = Number_num % 5.0f / 5.0f;

            //数値のUVのY座標
            if (Number_num < 5)
            {
                y = 0.5f;
            }
            if (Number_num >= 5)
            {
                y = 0.0f;
            }
        }

        w = 0.2f;
        h = 0.5f;

        SetNumber.uvRect = new Rect(x, y, w, h);
    }
}
