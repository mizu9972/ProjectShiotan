using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaScript : MonoBehaviour
{
    //エサが消えるまでの時間　測る用の変数
    private float wait;

    [Header("エサのオブジェクト")]
    public GameObject EsaPrefab;

    [Header("エサ消えるまでの時間")]
    public float Destroytime;


    void Start()
    {
        
    }


    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Stage")
        {
            var EsaInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
            EsaInstance.tag = "Esa";
            Destroy(EsaInstance, Destroytime);
            wait += Time.deltaTime;

            Destroy(this.gameObject);
        }
    }
}
