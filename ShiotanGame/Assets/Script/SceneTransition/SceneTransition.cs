using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
public class SceneTransition : MonoBehaviour
{
    [SerializeField, Header("シーン遷移実行フラグ")]
    bool TransitionRun = false;

    [Header("遷移先シーン名")]
    public string NextSceneName;

    [Header("1つ前のシーン")]
    public string BeforSceneName;

    private bool FadeStart;//フェード開始フラグ
    
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable().Where(_ => FadeStart).Take(1).Subscribe(_ => GameObject.Find("FadePanel").GetComponent<FadeScript>().SetIsFeadOut());
    }

    // Update is called once per frame
    void Update()
    {
        if(TransitionRun)
        {
            FadeStart = true;//フェード開始
        }
        if(FadeStart)
        {
            if(FeadOut())//フェードアウト->遷移
            {
                SceneTrans(NextSceneName);
            }
        }
    }

    private void SceneTrans(string scenename)//シーン遷移
    {
        SceneManager.LoadScene(scenename);
    }

    private bool FeadOut()//フェード終了ならtrue,終了していなければfalseを返す
    {
        //フェードアウト実行
        return GameObject.Find("FadePanel").GetComponent<FadeScript>().GetFeadStatus();
    }

    public void SetTransitionRun()
    {
        //遷移実行フラグをtrueに
        TransitionRun = true;
    }
}
