using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class RestFood : MonoBehaviour
{
    [SerializeField,Header("残りエサの数")]
    private float restFoods;

    [SerializeField, Header("HPを犠牲にするか")]
    private bool isSacrifi;

    [Header("1の位の描画オブジェクト")]
    public RawImage DigOne;

    [Header("10の位の描画オブジェクト")]
    public RawImage DigTen;

    private GameObject PlayerObj=null;//プレイヤー
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable().
            Where(_ => restFoods > 0).
            Take(1).
            Subscribe(_ => isSacrifi = true);//残りエサ数が0になったらHP犠牲状態へ

        this.UpdateAsObservable().Where(_=>!PlayerObj).Subscribe(_ => Init());//プレイヤーポップ後に初期化
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(PlayerObj)//Playerのオブジェクトが空でなければ実行
        {
            restFoods = PlayerObj.GetComponent<Player>().GetRestFood();
        }
        SetDigObjNum(); //描画する数値をセット
    }

    private void SetDigObjNum()
    {
        int setNum = (int)restFoods % 10;//1の位
        DigOne.GetComponent<DrawDig>().SetDrawNum(setNum);

        setNum = (int)restFoods / 10;//10の位
        DigTen.GetComponent<DrawDig>().SetDrawNum(setNum);
    }

    private void Init()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");//Playerを取得
    }
}
