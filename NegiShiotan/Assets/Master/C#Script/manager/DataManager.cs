using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField, Header("コインの数")]
    private int m_Coin;

    [SerializeField, Header("残機")]
    private int m_Remain = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_Remain = 1;
    }
    #region Getter&Setter
    public int Coin
    {
        get { return m_Coin; }
        set { m_Coin = value; }
    }
    public int Remain
    {
        get { return m_Remain; }
        set { m_Remain = value; }
    }
    #endregion

    #region Add&SubFunction
    //コイン
    public void AddCoin()
    {
        m_Coin++;
    }
    public void SubCoin()
    {
        m_Coin--;
    }
    //残機
    public void AddRemain()
    {
        m_Remain++;
    }
    public void SubRemain()
    {
        m_Remain--;
    }
    #endregion
}
