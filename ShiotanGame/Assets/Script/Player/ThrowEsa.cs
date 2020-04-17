using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEsa : MonoBehaviour
{
    [Header("エサ投げるクールタイム")]
    public float wait;

    //エサが消えるまでの時間　測る用の変数
    private float time;

    [Header("エサのオブジェクト")]
    public GameObject EsaPrefab;

    [Header("回数超えた後に投げるオブジェクト")]
    public GameObject HPDawnEsa;

    [Header("エサ投げる力")]
    public float throwrange;

    [Header("エサ投げれる回数")]
    public float count;

    [SerializeField,Header("犠牲にするHP量")]
    private float SacrificeHP;

    private HumanoidBase HPcnt;


    void Start()
    {
        HPcnt = this.GetComponentInParent<HumanoidBase>();

        //最初から投げれる状態
        time = wait;

    }


    void Update()
    {
        //エサ投げるまでのクールタイム
        if(time<=wait)
        {
            time += Time.deltaTime;
        }

        //えさ投げる
        if (Input.GetKeyDown("joystick button 0")|| Input.GetKey(KeyCode.Space))
        {
            //クールタイム超えたら投げれる
            if (time >= wait)
            {
                //エサ残っているか
                if (count > 0)
                {
                    var bulletInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
                    bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);

                    //エサ消える時間
                    Destroy(bulletInstance, 5);

                    //エサ一個消費
                    count--;
                }
                else
                {
                    var bulletInstance = Instantiate<GameObject>(HPDawnEsa, this.transform.position, this.transform.rotation);
                    bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);

                    //エサ消える時間
                    Destroy(bulletInstance, 5);

                    HPcnt.NowHP-=SacrificeHP;//犠牲にする分だけHPを減らす
                    
                }

                //エサ再び投げるまでのクールタイム
                time = 0;
            }
        }
    }

    public float GetCount()//残エサの数を返す
    {
        return count;
    }

    public float GetSacrificeHP()//犠牲にするHP量を返す
    {
        return SacrificeHP;
    }
}
