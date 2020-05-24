using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnStop : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
