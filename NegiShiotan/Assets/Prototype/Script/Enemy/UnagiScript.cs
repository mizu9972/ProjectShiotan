using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiScript : MonoBehaviour
{
    private GameObject PlayerObj;   //プレイヤーの位置
    private Rigidbody rb;           //Rigidbodyコンポーネントを入れる変数"rb"を宣言する
    private BoxCollider bc;         //BoxColliderコンポーネントを入れる変数"bc"を宣言する

    [Header("着地後　当たり判定　中心地点")] public Vector3 BoxCenter = new Vector3(-0.001f, 0.05f, 0);
    [Header("着地後　当たり判定　サイズ")] public Vector3 BoxSize = new Vector3(0.06f, 0.03f, 0.06f);

    [Header("ウナギ　吹き飛ぶ高さ")] public float BlowHigh;
    [Header("ウナギ　吹き飛ぶ力")] public float BlowPower;
    [Header("ウナギ　跳ねる力")] public float BoundPower;

    [Header("放電　エフェクト")] public GameObject ELEEfect;
    [Header("放電　クールタイム")] public float ELECoolTime=20;
    [Header("放電する時間")] public float ELEPlayTime = 10;

    [Header("電撃のクールタイム　吹っ飛び時指定時間カウントしない")] public float NotCoolCountTime = 5;
    private float NotCountTime=0;
    
    //電気放出中か
    private bool ELEEnable;
    [SerializeField]private float ELETime;  //何秒電気放出したか

    //着地したか
    private bool OnIkada;

    private bool onePlay;

    // Start is called before the first frame update
    void Start()
    {
        onePlay = true;
        ELEEnable = false;
        OnIkada = false;

        //親子関係したとき　メッシュがずれるバグ解消のための一文
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

        //イカダを親オブジェクトに設定
        PlayerObj = GameObject.FindGameObjectWithTag("Human");
        this.transform.SetParent(PlayerObj.transform.parent, true);

        rb = this.GetComponent<Rigidbody>();    //Rigidbodyコンポーネント　取得
        bc = this.GetComponent<BoxCollider>();  //BoxColliderコンポーネント　取得
    }

    // Update is called once per frame
    void Update()
    {
        if(NotCountTime<0)
        {
            ELETime += Time.deltaTime;  //放電時間カウント　増加
        }
        else
        {
            NotCountTime -= Time.deltaTime;
        }

        //ウナギ　角度初期化
        Vector3 localAngle = transform.localEulerAngles;
        localAngle.x = 0.0f;
        localAngle.y = 0.0f;
        localAngle.z = 0.0f;
        transform.localEulerAngles = localAngle; // 回転角度を設定

        //イカダに着地した
        if (OnIkada)
        {
            //電気放出中の時
            if (ELEEnable)
            {
                //設定した時間　電気放出
                if (ELETime > ELEPlayTime)
                {
                    ELETime = 0;
                    ELEEnable = false;
                    ELEEfect.SetActive(false);
                }
            }
            else
            {
                //電気放出　クールタイム終了
                if (ELETime > ELECoolTime)
                {
                    ELETime = 0;
                    ELEEnable = true;
                    ELEEfect.SetActive(true);
                }
            }
        }

        if (this.transform.localPosition.y < -4.0f)
        {
            //ウナギ削除
            Destroy(this.gameObject);
        }

        //当たり判定　変更
        if (onePlay && transform.localPosition.y > 0)
        {
            onePlay = false;
            bc.center = BoxCenter;
            bc.size = BoxSize;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //イカダとぶつかる
        if (other.gameObject.tag == "Player")
        {   
            //回転　防ぐ
            rb.isKinematic = true;

            OnIkada = true;
        }

        //イカダの上で魚とぶつかった時
        if ((other.gameObject.tag == "RidePiranha" || other.gameObject.tag == "RideFish") && rb.velocity.y < 0)
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
        //攻撃にあたる
        if (other.tag == "Attack")
        {
            //移動制限解除
            rb.isKinematic = false;

            //飛んでいく方向　指定
            Vector3 Throwpos = other.transform.forward;
            Throwpos.y = other.transform.localPosition.y + BlowHigh;

            //エフェクト再生
            //bulletInstance.GetComponent<PiranhaScript>().EffectPlay();
            //向いた方向に　飛ばす
            rb.AddForce(Throwpos * BlowPower, ForceMode.Force);

            NotCountTime = NotCoolCountTime;
        }
    }
}
