using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRaftPosition : MonoBehaviour
{
    //イカダのレイヤー番号
    const int PlayerRaftLayer = 17;

    private Ray ray;//Ray本体

    RaycastHit hit;//Rayが当たったオブジェクトいれる

    int distance = 10;//Rayの飛ばす距離

    [SerializeField, Header("プレイヤー")]
    private GameObject PlayerObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetNowPosition();
    }

    //現在のプレイヤーがイカダのどこにいるかを取得
    void GetNowPosition()
    {
        //Rayを飛ばす原点と方向を設定
        ray = new Ray(PlayerObj.transform.position, new Vector3(0, -1, 0));

        //Rayの可視化
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        //Rayの衝突検知
        if (Physics.Raycast(ray,out hit,distance,1 << PlayerRaftLayer))
        {
            //Rayが当たったオブジェクトのLayerがイカダなら
            Debug.Log(hit.textureCoord);

            //プレイヤーがイカダのどの位置にいるかを渡す
            PlayerObj.GetComponent<PlayerMove>().SetRaftPosition(hit.textureCoord);
        }
    }
}
