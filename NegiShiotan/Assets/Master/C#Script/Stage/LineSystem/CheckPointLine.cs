using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //PlayerPointと触れたら
        if (other.gameObject.layer == 11)
        {
            
        }
    }
}
