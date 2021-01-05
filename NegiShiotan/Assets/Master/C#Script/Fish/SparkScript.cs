using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkScript : MonoBehaviour
{
    [SerializeField, Header("電気　吹っ飛ばす高さの方角")]
    private float ELEBlowHigh = 2;

    [SerializeField, Header("電気　吹っ飛ばす力")]
    private float ELEBlowPower = 1;

    [SerializeField, Header("電気　ダメージ")]
    private int ELEATK = 1;

    private Status PlayerStatus;    //プレイヤーのステータス
    private Rigidbody PlayerRb;     //プレイヤーのrigidbody

    [Header("SE:放電")]
    public SEPlayer SE;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
        PlayerRb = GameObject.FindGameObjectWithTag("Human").GetComponent<Rigidbody>();
        SE.PlaySound();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human"&&PlayerRb.velocity.y<=0)
        {
            //電撃　ダメージ
            bool sts = PlayerStatus.DamageHP(ELEATK, false);

            //  無敵時間外にプレイヤーに当たったら吹っ飛ばす
            if (sts)
            {
                //プレイヤー向きをウナギ方向に
                other.transform.LookAt(this.transform.position);
                other.transform.rotation = new Quaternion(0, other.transform.rotation.y, 0, other.transform.rotation.w);

                //上に吹き飛ばす
                Vector3 Throwpos = -other.transform.forward;
                Throwpos.y = other.transform.position.y + ELEBlowHigh;

                //プレイヤー速度　初期化
                PlayerRb.velocity *= 0;

                //吹き飛ぶ力　追加
                PlayerRb.AddForce(Throwpos * ELEBlowPower, ForceMode.Force);
            }
        }
    }
}
