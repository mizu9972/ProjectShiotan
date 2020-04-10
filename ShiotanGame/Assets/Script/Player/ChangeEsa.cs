using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEsa : MonoBehaviour
{
    [Header("エサのオブジェクト")]
    public GameObject EsaPrefab;


    void Start()
    {

    }


    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Stage_Floor")
        {
            var EsaInstance = Instantiate<GameObject>(EsaPrefab, this.transform.position, this.transform.rotation);
            EsaInstance.tag = "Esa";

            Destroy(this.gameObject);
        }
    }
}
