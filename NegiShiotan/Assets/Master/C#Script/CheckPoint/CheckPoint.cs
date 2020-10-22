using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    DataAccessor dataAccessor;

    public void SaveStatusFunc()//コイン、残機を引き継ぎ
    {
        dataAccessor.SaveStatus();
    }
}
