using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEsa : MonoBehaviour
{
    //餌のオブジェクト
    public GameObject EsaPrefab;

    public float wait;

    public float throwrange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(wait>0)
        {
            wait += Time.deltaTime;
            if(wait>5)
            {
                wait = 0;
            }
        }
        //えさ投げる
        if (Input.GetKey(KeyCode.Space))
        {
            if (wait == 0.0f)
            {
                var bulletInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
                bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwrange, ForceMode.VelocityChange);
                Destroy(bulletInstance, 5.0f);
                wait += Time.deltaTime;
            }
        }
    }
}
