using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UniRx;

//ベルトコンベア式にステージをスクロールさせるシステムクラス
//X座標を減少させて移動させる
public class StageConveyorSystem : MonoBehaviour,IStageConveyorSystem
{
    [SerializeField,Header("ステージオブジェクト群")]
    private List<GameObject> StagePlaneList = new List<GameObject>();

    private List<GameObject> ActiveStagePlaneList = new List<GameObject>();//実際に操作するオブジェクト群

    [SerializeField, Header("描画カメラ")]
    private GameObject MainCamera = null;

    [Header("描画ステージプレーン数")]
    [Header("ステージについての設定")]
    public int ViewStageNum = 2;
    [Header("ステージの基本移動速度")]
    public float ScrollBaseSpeed = 1.0f;
    [Header("ループさせるか")]
    public bool isLooping = false;
    [Header("ステージを超えてから消えるまでの時間")]
    public float DestroyTime = 10.0f;

    [Header("落下速度")]
    [Header("滝を落ちる時の設定")]
    public float FallSpeed = 10.0f;

    [Header("減速速度")]
    [SerializeField, Header("ステージリア時の処理")]
    private float SpeedDownTime = 1.0f;
    
    private float NowScrollSpeed;//ステージ移動速度
    private int StagePlaneIter;
    private FallCamera m_FallCamera;
    
    //Y座標保存用
    private float planeYpos = 0.0f;

    //滝落下用Tween
    private Sequence m_FallTween;

    // Start is called before the first frame update
    void Awake()
    {
        NowScrollSpeed = ScrollBaseSpeed;

        StageInit();

        //0にならないように(0割り算回避のため)
        if(FallSpeed == 0)
        {
            FallSpeed = 1.0f;
        }
    }

    void Start()
    {
        if(MainCamera == null)
        {
            Debug.LogWarning("カメラが設定されていません\n 自動で設定しました。");
            MainCamera = Camera.main.gameObject;
        }
        
        
            m_FallCamera = MainCamera.GetComponent<FallCamera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var StageObject in ActiveStagePlaneList)
        {
            StageObject.transform.Translate(-1.0f * NowScrollSpeed, 0, 0);
        }
    }

    //ステージ処理初期化
    [ContextMenu("ステージ配置")]
    void StageInit()
    {
        StagePlaneIter = 0;
        //描画指定数分配置する
        for (int num = 0; num < ViewStageNum; num++)
        {
            StagePlaneIter = num;

            StageAdd(num);

            //if (num >= StagePlaneList.Count)
            //{
            //    StagePlaneIter = num % StagePlaneList.Count;
            //}

            //GameObject newStageObject = Object.Instantiate(StagePlaneList[StagePlaneIter]);//設定されているStagePlaneを複製
            //StageLineUpAtIter(num, newStageObject);//配置
            //ActiveStagePlaneList.Add(newStageObject);//配列へ追加
        }
        
    }

    //ステージプレーンを配置する
    private void StageLineUpAtIter(int Iter,GameObject newStagePlane)
    {
        if(Iter != 0)
        {
            //先頭以外のPlane
            //操作しているオブジェクト群の最後尾の座標を参照
            var OldStageScaleX = ActiveStagePlaneList.Last().transform.lossyScale.x;
            var OldStagePosition = ActiveStagePlaneList.Last().transform.position;

            //滝の落下後のY座標保存
            if(newStagePlane.tag == "FallBottom")
            {
                planeYpos = newStagePlane.transform.position.y;
            }

            //配置位置計算
            newStagePlane.transform.position = new Vector3(
                //新しく配置するPlaneのサイズの半分 +  既に配置されている最後尾のPlaneのサイズの半分 + 既に配置されている最後尾のPlaneの座標
                newStagePlane.transform.lossyScale.x / 2.0f * 10  + OldStageScaleX / 2.0f * 10 + OldStagePosition.x,
                planeYpos,
                OldStagePosition.z
                );

        }else if(Iter == 0)
        {
            //先頭のPlane
            //StageConveyor(自オブジェクト)の位置に配置
            newStagePlane.transform.position = this.gameObject.transform.position;
        }
    }
    //PlaneのEndLineに到達したときの処理
    public void OnEndLineSystem(GameObject obj)
    {
        Observable
            .Timer(System.TimeSpan.FromSeconds(DestroyTime))
            .Subscribe(_ =>
            {
                int ObjectInstanceID = obj.GetInstanceID();
                //オブジェクトを削除して操作配列から削除
                //インスタンスIDで判定
                foreach (var StageObject in ActiveStagePlaneList)
                {
                    if (ObjectInstanceID == StageObject.GetInstanceID())
                    {
                        ActiveStagePlaneList.Remove(StageObject);
                        break;
                    }
                }
                Destroy(obj);//削除

            });

        //新しくステージプレーン追加
        StageAdd(1);

    }

    //ステージプレーンを追加する
    void StageAdd(int num)
    {
        
        //配置したい番号が設定されているplane数を超えていたらリストの先頭から配置する
        if (StagePlaneIter >= StagePlaneList.Count)
        {
            if(isLooping == false)
            {
                return;
            }
            StagePlaneIter = StagePlaneIter % StagePlaneList.Count;
        }
        GameObject newStageObject = Object.Instantiate(StagePlaneList[StagePlaneIter]/*, Vector3.zero, Quaternion.identity, this.transform*/);//設定されているStagePlaneを複製
        newStageObject.transform.parent = this.gameObject.transform;
        StageLineUpAtIter(num, newStageObject);//配置
        ActiveStagePlaneList.Add(newStageObject);//配列へ追加
        StagePlaneIter++;
    }

    private void FallInit()
    {
        m_FallTween.SetRelative();
        m_FallTween.SetEase(Ease.InQuart);
    }

    //滝の落ちるラインの処理
    public void OnFallLineSystem(float FallEndPositionY_)
    {
        float defaultSpeed = NowScrollSpeed;
        m_FallTween = DOTween.Sequence().Append(transform.DOMoveY(0.0f - FallEndPositionY_, Mathf.Abs(FallEndPositionY_) / (FallSpeed * 10.0f)))
            .AppendCallback(() => ChangeSpeed(defaultSpeed));
        FallInit();
        m_FallTween.Play();

        ChangeSpeed(0);

        m_FallCamera.StartRotateTween(Mathf.Abs(FallEndPositionY_));
    }

    //クリア時の処理
    public void OnClearSystem()
    {
        //float DefalutSpeed = NowScrollSpeed;

        ////停止条件
        //var MoveStopStream = Observable.Timer(System.TimeSpan.FromSeconds(SpeedDownTime));

        ////減速させる
        //var ClearMoveStream = Observable.EveryUpdate()
        //.TakeUntil(MoveStopStream)
        //.Subscribe(_ => NowScrollSpeed -= DefalutSpeed / (SpeedDownTime * 60.0f));

        //var SpeedZeroStream = Observable.Timer(System.TimeSpan.FromSeconds(SpeedDownTime))
        //    .Subscribe(_ => NowScrollSpeed = 0.0f);

        ChangeSpeedbyTime(0, SpeedDownTime);
    }

    //段々速度変化
    public void ChangeSpeedbyTime(float toSpeed,float time)
    {
        float DefalutSpeed = NowScrollSpeed;

        //終了条件
        var ChangeStopStream = Observable.Timer(System.TimeSpan.FromSeconds(time));

        //速度変化
        var ChangeSpeedStream = Observable.EveryUpdate()
            .TakeUntil(ChangeStopStream)
            .Subscribe(_ => NowScrollSpeed += (toSpeed -  DefalutSpeed) / (time * 60.0f));

        var adjustSpeedStream = Observable.Timer(System.TimeSpan.FromSeconds(time))
            .Subscribe(_ => NowScrollSpeed = toSpeed);
    }

    //速度変更
    public void ChangeSpeed(float speed)
    {
        NowScrollSpeed = speed;
    }
}
