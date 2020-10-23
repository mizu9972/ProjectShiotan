using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor: MonoBehaviour
{
    DataManager dataManager = new DataManager();

    [SerializeField, Header("コインの数")]
    private int m_Coin = 0;

    [SerializeField, Header("残機")]
    private int m_Remain = 0;
    public void SaveStatus()//コイン、残機を引き継ぎ
    {
        dataManager.Coin = m_Coin;
        dataManager.Remain = m_Remain;
    }
}
