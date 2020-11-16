using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    [SerializeField, Header("リザルト画面キャンバス")]
    private GameObject ResultCanvas = null;

    private void Awake()
    {
        if(ResultCanvas == null)
        {
            ResultCanvas = GameObject.FindGameObjectWithTag("ClearCanvas");
        }
        ResultCanvas.SetActive(false);
        
    }

    //クリア演出
    public void ClearFunction()
    {
        ResultCanvas.SetActive(true);

    }
}
