using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaDestroy : MonoBehaviour
{
    [SerializeField,Header("エサ消えるまでの時間")]
    private float Destroytime=0;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(Destroytime<=time)
        {
            Destroy(this.gameObject);
        }
    }
}
