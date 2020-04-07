using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostFieldPillarc : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        // 見失うコリジョンから離れた時にたーげとロストする
        //if(transform.parent.gameObject.GetComponent<FlockBase>().TargetObject == other.gameObject) {
        //    transform.parent.GetComponent<FlockBase>().LostTarget();
        //}

        foreach (GameObject Target in transform.parent.gameObject.GetComponent<AIPillarc>().TargetList) {
            if (Target == other.gameObject) {
                gameObject.transform.parent.GetComponent<AIPillarc>().TargetList.Remove(other.gameObject);
                break;
            }
        }
    }
}
