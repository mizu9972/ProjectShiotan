using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class RaftMove : MonoBehaviour
{
    [Header("プレイヤーの位置による移動の割合")]
    public float MoveRate;

    [Header("Z軸方向のスピード(左が+、右が-)")]
    public float RaftSpead;

    [Header("プレイヤーの中心からのあそび")]
    public float Range;

    [SerializeField, Header("Z座標")]
    private float ZPos;

    [SerializeField, Header("イカダ　幅")]
    public float IkadaWidth;

    [SerializeField, Header("ステージ右壁")]
    public GameObject RightWall;

    [SerializeField, Header("ステージ左壁")]
    public GameObject LeftWall;

    [SerializeField, Header("プレイヤーがイカダのどこにいるか")]
    private Vector2 OnPlayerPos;
    
    [SerializeField, Header("ピラルク格納")]
    private List<GameObject> _Piraruku;

    [SerializeField, Header("プレイヤー")]
    private GameObject PlayerObj = null;

    //イカダのレイヤー番号
    const int PlayerRaftLayer = 17;

    private Ray ray;//Ray本体

    RaycastHit hit;//Rayが当たったオブジェクトいれる

    int distance = 10;//Rayの飛ばす距離

    // Start is called before the first frame update
    void Start()
    {
        //リスト初期化
        _Piraruku = new List<GameObject>();

        //プレイヤーオブジェクト　取得
        PlayerObj= GameObject.FindGameObjectWithTag("Human");
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー　イカダの上の位置　取得
        GetNowPlayerPosition(PlayerObj);

        //リストに格納されているピラルクの数だけ検索
        for (int i=1;i<_Piraruku.Count;i++)
        {
            //ピラルク　位置　イカダより下か
            if (_Piraruku[i].transform.localPosition.y < -1.0f)
            {
                //ピラルク削除
                _Piraruku.RemoveAt(i);
                i--;
                continue;
            }

            //ピラルク　イカダの上の位置　取得
            GetNowPirarukuPosition(_Piraruku[i]);
        }

        //イカダ移動処理
        MoveMain();
    }

    private void MoveMain()
    {
        //イカダ　速度制限
        if (RaftSpead > 2 * MoveRate)
        {
            RaftSpead = 2 * MoveRate;
        }
        if (RaftSpead < -2 * MoveRate)
        {
            RaftSpead = -2 * MoveRate;
        }

        //Z座標用変数に代入
        ZPos -= RaftSpead * Time.deltaTime;

        //イカダ　移動制限（ステージの端より進まない）
        if (ZPos + IkadaWidth > LeftWall.transform.position.z)
        {
            ZPos = LeftWall.transform.position.z - IkadaWidth;
        }
        if (ZPos - IkadaWidth < RightWall.transform.position.z)
        {
            ZPos = RightWall.transform.position.z + IkadaWidth;
        }

        Debug.Log(ZPos);

        //実際の座標に代入
        transform.position = new Vector3(transform.position.x, transform.position.y, ZPos);
    }

    //画面奥へ移動させる
    public void moveFar()
    {
        Tween moveTween;

        moveTween = transform.DOMoveX(1000f, 100.0f)
            .SetRelative();

        moveTween.Play();
    }


    //現在のプレイヤーがイカダのどこにいるかを取得
    void GetNowPlayerPosition(GameObject obj)
    {
        //Rayを飛ばす原点と方向を設定
        ray = new Ray(PlayerObj.transform.position, new Vector3(0, -1, 0));

        //Rayの可視化
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        //Rayの衝突検知
        if (Physics.Raycast(ray, out hit, distance, 1 << PlayerRaftLayer))
        {
            //イカダにプレイヤーの現在位置を送信
            SetOnPlayerPos(hit.textureCoord);
        }
    }

    //現在のピラルクがイカダのどこにいるかを取得
    void GetNowPirarukuPosition(GameObject obj)
    {
        //レイ　位置調整
        Vector3 save = obj.transform.position;
        save.y += 0.5f;

        //Rayを飛ばす原点と方向を設定
        ray = new Ray(save, new Vector3(0, -1, 0));

        //Rayの可視化
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
       
        //Rayの衝突検知
        if (Physics.Raycast(ray, out hit, distance, 1 << PlayerRaftLayer))
        {
            //イカダにピラルクの現在位置を送信
            SetOnPlusPos(hit.textureCoord);
        }
    }

    public void SetOnPlayerPos(Vector2 pos)
    {
        OnPlayerPos = pos;

        //あそびの部分の範囲を超えていたら
        if ((OnPlayerPos.y - 0.5f) <= -Range || (OnPlayerPos.y - 0.5f) >= Range)
        {
            //プレイヤーの位置によってスピードを決定
            RaftSpead = (OnPlayerPos.y - 0.5f) * MoveRate;
        }
    }

    public void SetOnPlusPos(Vector2 pos)
    {
        OnPlayerPos = pos;

        //あそびの部分の範囲を超えていたら
        if ((OnPlayerPos.y - 0.5f) <= -Range || (OnPlayerPos.y - 0.5f) >= Range)
        {
            //プレイヤーの位置によってスピードを決定
            RaftSpead += (OnPlayerPos.y - 0.5f) * MoveRate;
        }
    }

    public void SetOnPirarukuPos(GameObject obj)
    {
        _Piraruku.Add(obj);
    }
}
