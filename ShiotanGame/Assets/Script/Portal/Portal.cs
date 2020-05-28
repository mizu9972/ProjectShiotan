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

    private FadebyTex m_FadebyTex;
    // Start is called before the first frame update
    void Awake()
    {
        MyMesh = this.GetComponent<MeshRenderer>();
        MyMesh.enabled = false;//ゲーム開始時に描画オフに
    }

    private void Start()
    {
        SceneManager = Camera.main;//シーンマネージャをアタッチしてるカメラを取得
        m_FadebyTex = SceneManager.GetComponent<FadebyTex>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")//衝突相手がプレイヤーなら処理の実行
        {
            other.GetComponent<ProtoMove2>().enabled = false;//プレイヤーの操作無効
            m_FadebyTex.StartFadeOut();
            if(NextScene!=null && !isGoal)//次のシーンへ
            {
                if(isCarryOver)
                {
                    //ポータル通過時のみHPとエサの個数の引き継ぎ
                    //キーの引き継ぎも追加
                    GameManager.Instance.SetCarryOver(true);
                    float workHP = other.GetComponent<HumanoidBase>().NowHP + BonusHp;
                    float workFoods = other.GetComponent<Player>().GetRestFood() + BonusFood;
                    int workkey = GameManager.Instance.GetPlayer().GetComponent<Player>().KeyCount;
                    GameManager.Instance.SetWorkStatus(workHP, workFoods,workkey);
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
