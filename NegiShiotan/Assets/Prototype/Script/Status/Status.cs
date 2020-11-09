using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("無敵時間")]
    public float SetMutekiTime;
    private float MutekiTime;

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

    //コイン取得　同時取得　防止
    private float coincooltime;

    //ゲームオーバー用オブジェクト
    private GameOverManager m_GameOverManager = null;
    // Start is called before the first frame update
    void Start()
    {
        coincooltime = 0;
        HP = MAXHP;

        m_GameOverManager = GameObject.FindWithTag("GameOverManager").GetComponent<GameOverManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CoinNumDraw[0].SetNumberDraw(coin);
        CoinNumDraw[1].SetNumberDraw(coin);

        ZankiNumDraw[0].SetNumberDraw(Zanki);
        ZankiNumDraw[1].SetNumberDraw(Zanki);

        //コイン取得　クールタイム
        if (coincooltime > 0)
        {
            coincooltime -= 0.1f;
        }

        //無敵時間　計測
        if (MutekiTime > 0)
        {
            MutekiTime -= 0.1f;
        }

        //体力・残機
        //０になった時
        if (HP <= 0)
        {
            m_GameOverManager.HPGameOverFunction();
        }
        if (Zanki <= 0)
        {
            m_GameOverManager.ZankiGameOverFunction();
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

    public void RecoveryHP(int HPUP)
    {
        HP += HPUP;
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }
    }

    public bool DamageHP(int Damage)
    {
        //無敵時間　以外
        if (MutekiTime <= 0)
        {
            HP -= Damage;
            MutekiTime = SetMutekiTime; //無敵時間　セット
            return true;
        }
        return false;
    }

    public int GetCoin()
    {
        return coin;
    }

    public void UpCoin(int s_coin)
    {
        if (coincooltime <= 0)
        {
            coin += s_coin;
            coincooltime += 0.1f;
            if (coin > MAXcoin)
            {
                coin = 0;
                UpZanki(1);
            }
        }
    }

    public void UpZanki(int Up_zanki)
    {
        Zanki += Up_zanki;
        if (Zanki > MAXZanki)
        {
            Zanki = MAXZanki;
        }
    }

    //デバッグ用
    //HPを０にする
    [ContextMenu("HP０")]
    public void HP_Zero()
    {
        HP = 0;
    }
    //残機を０にする
    [ContextMenu("残機０")]
    public void Zanki_Zero()
    {
        Zanki = 0;
    }
}
