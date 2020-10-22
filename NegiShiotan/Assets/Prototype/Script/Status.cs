using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("MAXHP")]
    public int MAXHP;
    public int HP;      //現在のHP

    [Header("コイン数")]
    public int coin;
    [Header("最大コイン数")]
    public int MAXcoin;

    [Header("残機数")]
    public int Zanki;
    [Header("最大残機数")]
    public int MAXZanki;

    [Header("コイン数 表示画像")]
    public NumberScript[] CoinNumDraw;

    [Header("残機 表示画像")]
    public NumberScript[] ZankiNumDraw;

    // Start is called before the first frame update
    void Start()
    {
        HP = MAXHP;
    }

    // Update is called once per frame
    void Update()
    {
        CoinNumDraw[0].SetNumberDraw(coin);
        CoinNumDraw[1].SetNumberDraw(coin);

        ZankiNumDraw[0].SetNumberDraw(Zanki);
        ZankiNumDraw[1].SetNumberDraw(Zanki);

        if (coin >= MAXcoin)
        {
            coin = 0;
            SetZanki(1);
        }
    }

    public int GetHP()
    {
        return HP;
    }
    public int GetMAXHP()
    {
        return MAXHP;
    }

    public int GetCoin()
    {
        return coin;
    }

    public void UpCoin(int s_coin)
    {
        coin += s_coin;
        if(coin>MAXcoin)
        {
            coin = 0;
            SetZanki(1);
        }
        CoinNumDraw[0].SetNumberDraw(coin);
        CoinNumDraw[1].SetNumberDraw(coin);
    }

    public void SetZanki(int s_zanki)
    {
        Zanki += s_zanki;
        if(Zanki>MAXZanki)
        {
            Zanki = MAXZanki;
        }
        ZankiNumDraw[0].SetNumberDraw(Zanki);
        ZankiNumDraw[1].SetNumberDraw(Zanki);
    }
}
