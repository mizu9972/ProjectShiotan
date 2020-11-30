using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [Header("再生成するオブジェクト")]
    public GameObject obj;


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
        if (other.tag == "RidePiranha")
        {
            //ぶつかった　魚削除
            Destroy(other.gameObject);

            //弾丸型の魚生成し　前方に飛ばす
            GameObject instance = (GameObject)Instantiate(obj,
                                                          this.transform.position,
                                                          this.transform.rotation);
        }
    }
}
