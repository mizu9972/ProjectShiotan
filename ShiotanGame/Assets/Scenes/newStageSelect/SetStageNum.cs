using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetStageNum : MonoBehaviour
{
    [Header("スプライトリスト")]
    public Sprite[] sprites = new Sprite[15];
    public Kart kart;

    private int NowCount = 0;
    private Image MyImg;
    
    // Start is called before the first frame update
    void Start()
    {
        MyImg = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCount(kart.count);
        MyImg.sprite = sprites[NowCount];
    }

    public void SetCount(int value)
    {
        NowCount = value;
    }
}
