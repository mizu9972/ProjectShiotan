using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public GameObject pira;

    Transform save;


    [Header("ピラニア　進む力")]
    public float JumpPower ;

    [Header("ピラニア　飛ぶ高さ")]
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
        if(other.tag== "Piranha")
        {
            save = other.transform;
            Destroy(other.gameObject);
            GameObject bulletInstance = (GameObject)Instantiate(pira, save.position, this.transform.rotation);

            Vector3 Throwpos = this.transform.position;
            Throwpos.y = this.transform.position.y + JumpRange;

            //円　中心向かせる
            bulletInstance.transform.LookAt(Throwpos);

            //エフェクト再生
            bulletInstance.GetComponent<PiranhaScript>().EffectPlay();
            //向いた方向に　飛ばす
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * JumpPower, ForceMode.Impulse);
        }
    }
}
