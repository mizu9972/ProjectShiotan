using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Player : MonoBehaviour
{
    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;
    private void Start()
    {
        //スクリプトを取得
        GageScript = GameObject.Find("Gage").GetComponentInChildren<Gage>();
        
        GageScript.InitGage(this.GetComponent<HumanoidBase>().InitHP);//ゲージの初期化
    }

    private void Update()
    {
        
        GageScript.GageUpdate(this.GetComponent<HumanoidBase>().NowHP);
    }
}
