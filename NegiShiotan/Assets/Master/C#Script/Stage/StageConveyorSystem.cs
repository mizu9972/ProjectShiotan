using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

//ベルトコンベア式にステージをスクロールさせるシステムクラス
//X座標を減少させて移動させる
public class StageConveyorSystem : MonoBehaviour,IStageConveyorSystem
{
    [SerializeField,Header("ステージオブジェクト群")]
    private List<GameObject> StagePlaneList;

    private List<GameObject> ActiveStagePlaneList = new List<GameObject>();//実際に操作するオブジェクト群

    [Header("描画ステージプレーン数")]
    [Header("ステージについての設定")]
    public int ViewStageNum = 2;
    [Header("ステージの基本移動速度")]
    public float ScrollBaseSpeed = 1.0f;


    private GameObject ActiveStageObject = null;
    private float NowScrollSpeed;//ステージ移動速度
    private int StagePlaneIter;
    //private int StageObjectIter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        NowScrollSpeed = ScrollBaseSpeed;

        StageInit();
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

            //配置位置計算
            newStagePlane.transform.position = new Vector3(
                //新しく配置するPlaneのサイズの半分 +  既に配置されている最後尾のPlaneのサイズの半分 + 既に配置されている最後尾のPlaneの座標
                newStagePlane.transform.lossyScale.x / 2.0f * 10  + OldStageScaleX / 2.0f * 10 + OldStagePosition.x,
                newStagePlane.transform.position.y,
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

        //新しくステージプレーン追加
        StageAdd(1);

    }

    //ステージプレーンを追加する
    void StageAdd(int num)
    {
        //配置したい番号が設定されているplane数を超えていたらリストの先頭から配置する
        if (StagePlaneIter >= StagePlaneList.Count)
        {
            StagePlaneIter = StagePlaneIter % StagePlaneList.Count;
        }
        GameObject newStageObject = Object.Instantiate(StagePlaneList[StagePlaneIter]);//設定されているStagePlaneを複製
        StageLineUpAtIter(num, newStageObject);//配置
        ActiveStagePlaneList.Add(newStageObject);//配列へ追加
        StagePlaneIter++;
    }

    //滝の落ちるラインの処理
    public void OnFallLineSystem(float FallEndPositionY_)
    {

    }
}
