using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using DG.Tweening;

public enum StageSelectAction
{
    Next,
    Prev,
    Load
}
public class StageSelectManager : MonoBehaviour
{
    [SerializeField, Header("ステージ個数")]
    private const int STAGE_NUM = 10;

    [SerializeField, Header("ステージ選択マスオブジェクト群")]
    private List<GameObject> StageSquares = new List<GameObject>();

    [SerializeField, Header("ステージ間親オブジェクト群")]
    private List<GameObject> StageWhiles = new List<GameObject>();

    //前後ステージ間オブジェクト
    private class PreNexStageWhile
    {
        public GameObject PreWhile = null;
        public GameObject NexWhile = null;
    }
    private PreNexStageWhile[] m_PreNexStageWhile = new PreNexStageWhile[STAGE_NUM];

    [SerializeField, Header("ステージ選択シーケンスコレクション"), HideInInspector]
    private Dictionary<GameObject, PreNexStageWhile> StageSeqCollection = new Dictionary<GameObject, PreNexStageWhile>();


    [SerializeField, Header("ステージ選択で動かすオブジェクト")]
    private GameObject StageSelectPlayer = null;

    //現在選択中のステージ添字
    private int m_nowSelectStageIter = 0;

    [HideInInspector]//移動速度
    public float StageMoveTime = 1.0f;

    private Sequence m_moveStageSequence = null;
    // Start is called before the first frame update
    void Start()
    { 
        
        m_nowSelectStageIter = 0;
        SetStageCollections();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("SelectNext"))
        {
            //前進
            MoveSelectStage(StageSelectAction.Next);
        }
        else if (Input.GetButtonDown("SelectPreview"))
        {
            //後退
            MoveSelectStage(StageSelectAction.Prev);
        }
    }

    //StageSeqCollectionの設定
    //ステージ選択のオブジェクトを紐づけていく
    private void SetStageCollections()
    {
        for(int InitPNtStageWhileIter = 0; InitPNtStageWhileIter < STAGE_NUM; InitPNtStageWhileIter++)
        {
            m_PreNexStageWhile[InitPNtStageWhileIter] = new PreNexStageWhile(); 
        }
        //ステージ１
        {
            m_PreNexStageWhile[0].PreWhile = null;
            m_PreNexStageWhile[0].NexWhile = StageWhiles[0];

            StageSeqCollection[StageSquares[0]] = m_PreNexStageWhile[0];
        }

        //ステージ２～９
        for (int StageNum = 1; StageNum < STAGE_NUM - 1; StageNum++)
        {
            m_PreNexStageWhile[StageNum].PreWhile = StageWhiles[StageNum - 1];
            m_PreNexStageWhile[StageNum].NexWhile = StageWhiles[StageNum];

            StageSeqCollection[StageSquares[StageNum]] = m_PreNexStageWhile[StageNum];
        }

        //ステージ10
        {
            m_PreNexStageWhile[9].PreWhile = StageWhiles[8];
            m_PreNexStageWhile[9].NexWhile = null;

            StageSeqCollection[StageSquares[9]] = m_PreNexStageWhile[9];
        }
    }

    
    private void MoveSelectStage(StageSelectAction stageSelectAct)
    {
        //ステージ選択行動
        switch (stageSelectAct)
        {
            //次のステージへ
            case StageSelectAction.Next:
                MoveStage(StageSeqCollection[StageSquares[m_nowSelectStageIter]].NexWhile, StageSelectAction.Next);
                break;

            //前のステージへ
            case StageSelectAction.Prev:
                MoveStage(StageSeqCollection[StageSquares[m_nowSelectStageIter]].PreWhile, StageSelectAction.Prev);
                break;

            //選択中のステージ読み込み
            case StageSelectAction.Load:
                break;
        }
    }

    //ステージ間移動処理       //ステージ間オブジェクト移動順リスト　//移動の種類(前進か後退か)
    private void MoveStage(GameObject movedWhileData, StageSelectAction selectAction)
    {
        //移動先が無ければなにもしない
        if (movedWhileData == null)
        {
            return;
        }

        if (selectAction == StageSelectAction.Next)
        {
            m_nowSelectStageIter += 1;
        }else if(selectAction == StageSelectAction.Prev)
        {
            m_nowSelectStageIter -= 1;
        }

        //挙動開始
        MoveStageTween(movedWhileData.GetComponent<StageWhileParent>().getStageWhileList(selectAction), StageSquares[m_nowSelectStageIter]);
    }

    //DOTweenによる実際の挙動      //ステージ間移動順リスト　　　　　　　　//移動先オブジェクト
    private void MoveStageTween(List<GameObject> stageWhileList, GameObject movedStageSquare)
    {

        m_moveStageSequence = DOTween.Sequence();

        //移動するオブジェクト数を計算　ステージ間　＋　移動先ステージマス
        float ObjectCount = stageWhileList.Count + 1;

        //一点ずつの移動時間
        float moveTimeperPoint = 0;
        moveTimeperPoint = StageMoveTime / ObjectCount;

        //ステージ間
        for (int StageWhilePointIter = 0; StageWhilePointIter < stageWhileList.Count; StageWhilePointIter++)
        {
            m_moveStageSequence.Append(
                StageSelectPlayer.transform.DOMove(stageWhileList[StageWhilePointIter].transform.position, moveTimeperPoint)
            );
        }

        //移動先ステージマス
        m_moveStageSequence.Append(
            StageSelectPlayer.transform.DOMove(movedStageSquare.transform.position, moveTimeperPoint)
            );
    }

    //符号を取得
    //正は１　０は０　負はー１
    private float judgeSign(double value)
    {
        if (value > 0) { return 1; }
        else if (value < 0) { return -1; }
        else return 0;
    }
}

//ステージ間移動詳細設定用エディター拡張
//TODO DOTween設定項目実装
[CustomEditor(typeof(StageSelectManager))]
public class StageSelectManagerCustom : Editor
{
    StageSelectManager m_SSManager;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        m_SSManager = target as StageSelectManager;

        EditorGUI.BeginChangeCheck();


        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("ステージ間の挙動に関する設定");
        {

        }
        EditorGUILayout.EndVertical();

        //変更されたら
        if (EditorGUI.EndChangeCheck())
        {
            //Undoに保存
            Undo.RecordObject(m_SSManager, "StageSelectManager");

            EditorUtility.SetDirty(m_SSManager);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        serializedObject.ApplyModifiedProperties();
    }
}