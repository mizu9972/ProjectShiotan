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

    [Header("イカダ端の壁の位置")]
    public Transform IkadaWidth;

    [Header("コイン数 表示画像")]
    public NumberScript[] CoinNumDraw;

    [Header("残機 表示画像")]
    public NumberScript[] ZankiNumDraw;

    private PlayerMove P_MoveScript;    //プレイヤー　倒れるアニメーション設定用

    [Header("SE:プレイヤーダメージ受けた時")]
    public SEPlayer SE;

    private DataManager m_SceneManager = null;
    // Start is called before the first frame update
    void Start()
    {
        m_SceneManager = GameObject.Find("SceneManager").GetComponent<DataManager>();//DataManager取得

        coin = m_SceneManager.GetComponent<DataManager>().Coin;//コイン引き継ぎ
        Zanki = m_SceneManager.GetComponent<DataManager>().Remain;

        HP = MAXHP;

        P_MoveScript= GameObject.FindWithTag("Human").GetComponent<PlayerMove>();

        //イカダ端　位置セット
        P_MoveScript.SetIkadaWidth(IkadaWidth.localPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        //UI表示
        CoinNumDraw[0].SetNumberDraw(coin);     //コイン
        CoinNumDraw[1].SetNumberDraw(coin);
        ZankiNumDraw[0].SetNumberDraw(Zanki);   //残機
        ZankiNumDraw[1].SetNumberDraw(Zanki);
        
        //無敵時間　計測
        if (MutekiTime > 0)
        {
            MutekiTime -= 0.1f;
        }

        m_SceneManager.Coin = coin;//コインを常に更新
        m_SceneManager.Remain = Zanki;
    }

    public float GetIkadaWidth()
    {
        return IkadaWidth.localPosition.x;
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

    //HP回復
    public void RecoveryHP(int HPUP)
    {
        HP += HPUP;
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }
    }

    //HP元に戻す
    public void ResetHP()
    {
        HP = MAXHP;
        MutekiTime = SetMutekiTime; //無敵時間　セット
    }

    //プレイヤー　ダメージ計算
    public bool DamageHP(int Damage,bool ac)
    {
        //無敵時間　以外　ダメージ受ける
        if (MutekiTime <= 0)
        {
            SE.PlaySound();
            HP -= Damage;
            MutekiTime = SetMutekiTime; //無敵時間　セット

            //体力・残機
            //０になった時
            if (HP <= 0)
            {
                //残機によって復活可能なら
                if (Zanki > 0)
                {
                    //HP0以下　倒れる＆起き上がる演出
                    P_MoveScript.SetLive(1);
                }

                //残機減少
                DownZanki(1);

                return true;
            }

            //すぐに倒れるか
            if (ac)
            {
                P_MoveScript.SetKokeru();
            }

            return true;
        }
        return false;
    }

    //コイン取得
    public void UpCoin(int s_coin)
    {
        coin += s_coin;
        if (coin > MAXcoin)
        {
            coin = 0;
            UpZanki(1);
        }
    }

    //残機増幅
    public void UpZanki(int Up_zanki)
    {
        Zanki += Up_zanki;
        if (Zanki > MAXZanki)
        {
            Zanki = MAXZanki;
        }
    }

    //残機減少
    public void DownZanki(int Down_zanki)
    {
        Zanki -= Down_zanki;

        //残機がゼロになったら
        if (Zanki < 0)
        {
            //HP0以下　倒れる演出
            P_MoveScript.SetLive(2);
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
