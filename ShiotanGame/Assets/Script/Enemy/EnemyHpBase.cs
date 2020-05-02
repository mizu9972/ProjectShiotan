using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// GageScriptには子オブジェクトの
/// EnemyHpCanvas->GageParent->EnemyGage->Barを持っていってください
/// </summary>
public class EnemyHpBase : MonoBehaviour
{
    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;
    // Start is called before the first frame update
    void Start()
    {
        GageScript.InitGage(this.GetComponent<HumanoidBase>().InitHP);//ゲージの初期化
    }

    // Update is called once per frame
    void Update()
    {
        //現在のHPをゲージに反映
        GageScript.GageUpdate(this.GetComponent<HumanoidBase>().NowHP);
    }
}
