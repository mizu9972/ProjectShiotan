using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOpenGateField : MonoBehaviour
{
    private GameObject Parent;

    private void Start() {
        Parent = gameObject.transform.parent.gameObject;    
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player" && !Parent.GetComponent<KeyOpenGate>().IsOpen) {
            if(other.gameObject.GetComponent<Player>().KeyCount >= Parent.GetComponent<KeyOpenGate>().NeedKeys) {
                other.gameObject.GetComponent<Player>().KeyCount -= Parent.GetComponent<KeyOpenGate>().NeedKeys;
                AudioManager.Instance.PlaySE("SE_OPEN");
                Parent.GetComponent<KeyOpenGate>().IsOpen = true;
            }
        }
    }
}
