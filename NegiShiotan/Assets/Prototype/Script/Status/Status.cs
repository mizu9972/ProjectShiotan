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
    }

    public int GetHP()
    {
        return HP;
    }
    public int GetMAXHP()
    {
        return MAXHP;
    }

    public void RecoveryHP(int HPUP)
    {
        HP += HPUP;
        if(HP>MAXHP)
        {
            HP = MAXHP;
        }
    }

    public void DamageHP(int Damage)
    {
        HP -= Damage;
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
            UpZanki(1);
        }
    }

    public void UpZanki(int Up_zanki)
    {
        Zanki += Up_zanki;
        if(Zanki>MAXZanki)
        {
            Zanki = MAXZanki;
        }
    }
}
