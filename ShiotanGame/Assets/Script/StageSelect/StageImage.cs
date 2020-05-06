using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageImage : MonoBehaviour
{
    [Header("一つ前のオブジェクト")]
    public Image PrevStage;

    [Header("一つ後のオブジェクト")]
    public Image NextStage;

    [Header("遷移先シーン名")]
    public string NextScene;

    private bool isSelect = false;//現在選択状態か

    //前後のオブジェクトがあるかの判定用
    private bool isNextExist = false;
    private bool isPrevExist = false;
    private RectTransform MyRectTrans;
    // Start is called before the first frame update
    void Awake()
    {
        //前後にオブジェクトがあるかを判定
        if(NextStage)
        {
            isNextExist = true;
        }
        if(PrevStage)
        {
            isPrevExist = true;
        }

        MyRectTrans = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetisSelect(bool select)//選択状態の設定
    {
        isSelect = select;
    }

    public void SetSize(Vector2 sizeselta)
    {
        MyRectTrans.sizeDelta = sizeselta;
    }

    public void SelectStage()//ステージを選択
    {
        Camera.main.GetComponent<SceneTransition>().SetTransitionRun(NextScene);
    }

    public Image GoNext()//次のオブジェクトを選択
    {
        NextStage.GetComponent<StageImage>().SetisSelect(true);
        isSelect = false;
        return NextStage;//次のステージオブジェクトを返す
    }
    public Image GoPerv()//前のオブジェクトを選択
    {
        PrevStage.GetComponent<StageImage>().SetisSelect(true);
        isSelect = false;
        return PrevStage;//前のステージオブジェクトを返す
    }

    public bool GetNextExist()
    {
        return isNextExist;
    }
    public bool GetPrevExist()
    {
        return isPrevExist;
    }
}
