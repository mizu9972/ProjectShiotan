using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirarukuScript : MonoBehaviour
{
    private GameObject PlayerObj;   //プレイヤーの位置
    private Rigidbody rb;           //Rigidbodyコンポーネントを入れる変数"rb"を宣言する

    [Header("ピラルク　吹き飛ぶ高さ")] public float BlowHigh;
    [Header("ピラルク　吹き飛ぶ力")] public float BlowPower;
    [Header("ピラルク　跳ねる力")] public float BoundPower;

    //イカダの上の位置　渡す
    private RaftMove IkadaPos;

    //一度だけの処理
    private bool OnePlay;

    [Header("Effect")]
    public ParticleEffectScript m_Effect;

    // Start is called before the first frame update
    void Start()
    {
        //イカダ移動スクリプト　取得
        IkadaPos = GameObject.FindGameObjectWithTag("Player").GetComponent<RaftMove>();

        //親子関係したとき　メッシュがずれるバグ解消のための一文
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

        //イカダを親オブジェクトに設定
        PlayerObj = GameObject.FindGameObjectWithTag("Human");
        this.transform.SetParent(PlayerObj.transform.parent, true);
        
        rb = this.GetComponent<Rigidbody>();    //Rigidbodyコンポーネント　取得

        OnePlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localPosition.y<-4.0f)
        {
            //ピラルク削除
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //イカダとぶつかる
        if (other.gameObject.tag == "Player")
        {
            //回転　防ぐ
            rb.isKinematic = true;

            //ピラルク　格納（一度だけ）
            if(OnePlay)
            {
                //ピラルクのデータ　イカダ移動スクリプトに渡す
                IkadaPos.SetOnPirarukuPos(this.gameObject);
                OnePlay = false;

                //ピラルク　吹き飛ぶ最高高度　設定
                BlowHigh += this.transform.localPosition.y;
            }
        }

        //イカダの上で魚とぶつかった時
        if ((other.gameObject.tag == "RidePiranha" || other.gameObject.tag == "RideFish") && rb.velocity.y<0)
        {
            //重力停止
            //rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);

            //吹き飛ぶ方向
            Vector3 Throwpos2 = -this.transform.forward;
            Throwpos2.y = BlowHigh;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos2 * BoundPower, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //クリアライン超えたとき
        if (other.gameObject.tag == "ClearLine")
        {
            IkadaPos.ClearFishDelete();
            Destroy(this.gameObject);
        }

        //攻撃にあたる
        if (other.tag == "Attack")
        {
            //移動制限解除
            rb.isKinematic = false;

            //飛んでいく方向　指定
            Vector3 Throwpos = other.transform.forward;
            Throwpos.y = other.transform.localPosition.y+ BlowHigh;

            Debug.Log("attack");

            //エフェクト再生
            //bulletInstance.GetComponent<PiranhaScript>().EffectPlay();
            //向いた方向に　飛ばす
            rb.AddForce(Throwpos* BlowPower, ForceMode.Force);
        }
    }

    public void EffectPlay()//Effect再生
    {
        m_Effect.StartEffect();
    }
}
