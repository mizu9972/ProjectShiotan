using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    //生成オブジェクト：ピラニア
    public GameObject pirania;

    //生成オブジェクト：ピラルク
    public GameObject piraruku;

    //生成オブジェクト：ウナギ
    public GameObject unagi;

    //生成位置
    Transform savepos;


    [Header("魚　進む力")]
    public float JumpPower ;

    [Header("魚　飛ぶ高さ")]
    public float JumpRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetJumpPower(float Power)
    {
        JumpPower = Power;
    }

    public void SetJumpRange(float Range)
    {
        JumpRange = Range;
    }

    private void OnTriggerEnter(Collider other)
    {
        //ピラニア
        if(other.tag== "Piranha")
        {
            savepos = other.transform;
            Destroy(other.gameObject);
            GameObject bulletInstance = (GameObject)Instantiate(pirania, savepos.position, this.transform.rotation);

            Vector3 Throwpos = this.transform.position;
            Throwpos.y = this.transform.position.y + JumpRange;

            //円　中心向かせる
            bulletInstance.transform.LookAt(Throwpos);

            //エフェクト再生
            bulletInstance.GetComponent<PiranhaScript>().EffectPlay();
            //向いた方向に　飛ばす
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * JumpPower, ForceMode.Impulse);
        }

        //ピラルク
        if(other.tag=="Pillarc")
        {
            savepos = other.transform;
            Destroy(other.gameObject);
            GameObject bulletInstance = (GameObject)Instantiate(piraruku, savepos.position, this.transform.rotation);

            Vector3 Throwpos = this.transform.position;
            Throwpos.y = this.transform.position.y + JumpRange/3;

            //円　中心向かせる
            bulletInstance.transform.LookAt(Throwpos);

            //エフェクト再生
            bulletInstance.GetComponent<PirarukuScript>().EffectPlay();
            //向いた方向に　飛ばす
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * JumpPower, ForceMode.Impulse);
        }

        //ウナギ
        if (other.tag == "Unagi")
        {
            savepos = other.transform;
            Destroy(other.gameObject);
            GameObject bulletInstance = (GameObject)Instantiate(unagi, savepos.position, this.transform.rotation);

            Vector3 Throwpos = this.transform.position;
            Throwpos.y = this.transform.position.y + JumpRange / 3;

            //円　中心向かせる
            bulletInstance.transform.LookAt(Throwpos);

            //エフェクト再生
            bulletInstance.GetComponent<UnagiScript>().EffectPlay();
            //向いた方向に　飛ばす
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * JumpPower, ForceMode.Impulse);
        }
    }
}
