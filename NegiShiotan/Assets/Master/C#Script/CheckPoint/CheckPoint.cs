using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    DataAccessor dataAccessor;

    public void SaveStatus(int coin,int remain)//コイン、残機を引き継ぎ
    {
        dataAccessor.Coin = coin;
        dataAccessor.Remain = remain;
    }
}
