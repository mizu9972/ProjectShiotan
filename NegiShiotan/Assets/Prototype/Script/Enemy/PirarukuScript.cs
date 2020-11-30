using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirarukuScript : MonoBehaviour
{
    private GameObject PlayerObj;   //プレイヤーの位置

    [Header("ピラルク　吹き飛ぶ高さ")]
    public float BlowHigh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //イカダとぶつかる
        if (other.gameObject.tag == "Player")
        {
            //親子関係したとき　メッシュがずれるバグ解消のための一文
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

            //イカダを親オブジェクトに設定
            this.transform.SetParent(PlayerObj.transform.parent, true);
            
            //ピラルク　吹き飛ぶ最高高度　設定
            BlowHigh += this.transform.localPosition.y;
        }
    }
}
