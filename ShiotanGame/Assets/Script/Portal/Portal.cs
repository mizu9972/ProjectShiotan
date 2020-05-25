using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private MeshRenderer MyMesh;//メッシュレンダラー
    private Camera SceneManager;

    [Header("遷移先シーン名")]
    public string NextScene = null;

    [Header("ゴールにする")]
    public bool isGoal;

    [Header("HPとエサを引き継ぐか")]
    public bool isCarryOver = true;

    [Header("ステージ移動後にもらえるエサ")]
    public float BonusFood = 0f;

    [Header("ステージ移動後にもらえるHP")]
    public float BonusHp = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        MyMesh = this.GetComponent<MeshRenderer>();
        MyMesh.enabled = false;//ゲーム開始時に描画オフに
    }

    private void Start()
    {
        SceneManager = Camera.main;//シーンマネージャをアタッチしてるカメラを取得
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")//衝突相手がプレイヤーなら処理の実行
        {
            other.GetComponent<ProtoMove2>().enabled = false;//プレイヤーの操作無効
            if(NextScene!=null && !isGoal)//次のシーンへ
            {
                if(isCarryOver)
                {
                    //ポータル通過時のみHPとエサの個数の引き継ぎ
                    GameManager.Instance.SetCarryOver(true);
                    float workHP = other.GetComponent<HumanoidBase>().NowHP + BonusHp;
                    float workFoods = other.GetComponent<Player>().GetRestFood() + BonusFood;
                    GameManager.Instance.SetWorkStatus(workHP, workFoods);
                }
                
                SceneManager.GetComponent<SceneTransition>().SetTransitionRun(NextScene);
            }
            else if(isGoal)//ゴール
            {
                SceneManager.GetComponent<SceneTransition>().SetTransitionRun("ClearScene");
            }
        }
    }
}
