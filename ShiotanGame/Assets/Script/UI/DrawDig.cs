using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDig : MonoBehaviour
{
    [SerializeField, Header("描画する数値")]
    private int drawNum;

    public Text textObj;

    private float U = 0f;
    private float V = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textObj.text = drawNum.ToString();
    }

    public void SetDrawNum(int num)//描画する数値をセット
    {
        drawNum = num;
    }

    private void SetUVPosition()
    {
        //this.GetComponent<>
    }
}
