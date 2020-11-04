using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public GameObject pira;

    Transform save;


    [Header("ピラニア　ジャンプ力")]
    public float JumpPower ;

    [Header("ピラニア　発射角度")]
    public float JumpRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Piranha")
        {
            save = other.transform;
            GameObject bulletInstance = (GameObject)Instantiate(pira, save.position, this.transform.rotation);

            Vector3 Throwpos = this.transform.position;
            Throwpos.y = this.transform.position.y + JumpRange;

            //円　中心向かせる
            bulletInstance.transform.LookAt(Throwpos);

            //向いた方向に　飛ばす
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * JumpPower, ForceMode.Impulse);
        }
    }
}
