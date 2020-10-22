using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor: MonoBehaviour
{
    DataManager dataManager;

    [SerializeField, Header("コインの数")]
    private int m_Coin;

    [SerializeField, Header("残機")]
    private int m_Remain;
    public void SaveStatus()//コイン、残機を引き継ぎ
    {
        dataManager.Coin = m_Coin;
        dataManager.Remain = m_Remain;
    }
}
