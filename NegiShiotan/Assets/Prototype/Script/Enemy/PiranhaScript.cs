using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScript : MonoBehaviour
{
    private bool MoveOn;
    private float cooltime;

    //Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    private Rigidbody rb;

    private GameObject PlayerObj;   //プレイヤーの位置
    private Rigidbody PlayerRb;
    private Status PlayerStatus;    //プレイヤーのステータス

    [Header("与えるダメージ量")]
    public int ATK;

    [Header("回転の度合い")]
    public float ang;

    [Header("移動速度")]
    public float Speed;

    [Header("プレイヤー　吹き飛ばす力")]
    public float BlowPower;

    [Header("ステージ移動　落下阻止用")]
    public BoxCollider StageMoveLimit;

    //吹き飛び状態
    private bool BlowTime;

    float SaveY;


    //Start is called before the first frame update
    void Start()
    {
        BlowTime = false;   //吹っ飛んでいない
        MoveOn = false;     //動かない
        cooltime = 1;       //動かない時間
        PlayerObj = GameObject.FindGameObjectWithTag("Human");
        PlayerRb = GameObject.FindGameObjectWithTag("Human").GetComponent<Rigidbody>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
        rb = this.GetComponent<Rigidbody>();

        //親子関係したとき　メッシュがずれるバグ解消のための一文
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

        //イカダを親オブジェクトに設定
        this.transform.SetParent(PlayerObj.transform.parent, true);
    }

    //Update is called once per frame
    void Update()
    {
        //一定時間停止
        if (cooltime > 0)
        {
            cooltime -= 0.1f;
        }

        //吹っ飛び中
        if (BlowTime)
        {
            //XZ回転　初期化
            Quaternion XYrot = this.transform.rotation;
            XYrot.x = 0;
            XYrot.z = 0;

            //現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = XYrot;
        }

        //移動可能か（イカダの上）
        if (MoveOn && cooltime < 0&&BlowTime==false)
        {
            //プレイヤー方向のベクトル　取得
            Vector3 relativePos = PlayerObj.transform.position - this.transform.position;

            //プレイヤーの方　向く
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = 0;
            rotation.z = 0;

            //現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 0.1f);
            
            //移動処理（前方へ）
            Vector3 velocity = gameObject.transform.rotation * new Vector3(0, 0, Speed);
            gameObject.transform.position += velocity * Time.deltaTime;
        }

        //一定以下の高度　削除
        if(this.transform.position.y<SaveY)
        {
            Vector3 pos = this.transform.position;
            pos.y = SaveY;
            this.transform.position = pos;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human")
        {
            //プレイヤー向きをピラニアに(Z正面)
            other.transform.LookAt(this.transform.position);
            other.transform.rotation = new Quaternion(0, other.transform.rotation.y, 0, other.transform.rotation.w);


            //上に吹き飛ばす
            Vector3 Throwpos = this.transform.forward;
            Throwpos.y = this.transform.position.y + 3;

            //吹き飛ぶ力　追加
            PlayerRb.AddForce(Throwpos * BlowPower, ForceMode.Impulse);


            //吹き飛ぶ方向
            Vector3 Throwpos2 = -this.transform.forward;
            Throwpos2.y += 2;

            //吹き飛ぶ力　追加
            rb.AddForce(Throwpos2 * BlowPower, ForceMode.Impulse);


            //プレイヤーのHP減少
            PlayerStatus.DamageHP(ATK);
        }

        //イカダに着地時
        if (other.tag == "Player")
        {
            rb.useGravity = false;

            //移動可能
            BlowTime = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //イカダに着地時
        if (other.tag == "Player")
        {
            rb.useGravity = false;

            //移動可能
            BlowTime = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //イカダに着地時
        if (other.tag == "Player")
        {
            //重力　ON
            rb.useGravity = true;

            //移動不可能
            BlowTime = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //親子関係したとき　メッシュがずれるバグ解消のための一文
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
            
            //イカダを親オブジェクトに設定
            this.transform.SetParent(other.transform, true);

            //当たり判定処理　変更
            this.GetComponent<BoxCollider>().isTrigger = true;

            MoveOn = true;
            cooltime = 1.0f;
            StageMoveLimit.enabled = true;

            SaveY = this.transform.position.y;
        }

        if(other.gameObject.tag== "IkadaMoveLimit")
        {
            Debug.Log("Stop");
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //   if (other.tag == "IkadaMoveLimit"&&BlowTime)
    //   {
    //       rb.velocity = new Vector3(0, 0, 0);
    //       Debug.Log("bbb");
    //   }
    //}
}
