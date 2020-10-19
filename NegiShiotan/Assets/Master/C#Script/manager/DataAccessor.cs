using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor: MonoBehaviour
{
    [SerializeField, Header("コインの数")]
    private int m_Coin;

    [SerializeField, Header("残機")]
    private int m_Remain;

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
}
