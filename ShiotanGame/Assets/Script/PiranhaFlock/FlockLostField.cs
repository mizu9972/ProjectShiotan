using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockLostField : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        // 見失うコリジョンから離れた時にたーげとロストする
        if(transform.parent.gameObject.GetComponent<FlockBase>().TargetObject == other.gameObject) {
            transform.parent.GetComponent<FlockBase>().LostTarget();
        }
    }
}
