using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEsa : MonoBehaviour
{
    //餌のオブジェクト
    public GameObject EsaPrefab;

    public float wait;

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
                bulletInstance.GetComponent<Rigidbody>().AddForce(this.transform.forward * 12, ForceMode.VelocityChange);
                bulletInstance.tag = "Esa";     // ピラニアが追いかける対象の判別にタグが必要だったため追加　青木
                Destroy(bulletInstance, 5.0f);
                wait += Time.deltaTime;
            }
        }
    }
}
