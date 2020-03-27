using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaScript : MonoBehaviour
{
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

    void OnCollisionEnter(Collision other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Stage")
        {
            var EsaInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
            EsaInstance.tag = "Esa";
            Destroy(EsaInstance, Destroytime);

            Destroy(this.gameObject);
        }
    }
}
