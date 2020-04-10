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
        //TODO gageのinit処理を実装　GageScript.InitGage(HumanoidBaseの初期HP);//ゲージの初期化
    }

    private void Update()
    {
        //TODO ゲージのダメージ関数呼び出しGageScript.GageUpdate(HumanoidBaseの現在HP);
    }
}
