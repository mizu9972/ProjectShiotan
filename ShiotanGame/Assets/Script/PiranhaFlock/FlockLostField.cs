using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockLostField : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        // 見失うコリジョンから離れた時にたーげとロストする
        //if(transform.parent.gameObject.GetComponent<FlockBase>().TargetObject == other.gameObject) {
        //    transform.parent.GetComponent<FlockBase>().LostTarget();
        //}

        foreach (GameObject Target in transform.parent.gameObject.GetComponent<AIFlock>().TargetList) {
            if (Target == other.gameObject) {
                if(Target.tag == "Player") {
                    if (gameObject.transform.parent.gameObject.GetComponent<AIFlock>().RashSEChannel != -1) {
                        AudioManager.Instance.StopLoopSe(gameObject.transform.parent.gameObject.GetComponent<AIFlock>().RashSEChannel);
                        gameObject.transform.parent.gameObject.GetComponent<AIFlock>().RashSEChannel = -1;
                        gameObject.transform.parent.gameObject.GetComponent<AIFlock>().NowSEType = RushSE.None;
                    }
                }
                gameObject.transform.parent.GetComponent<AIFlock>().TargetList.Remove(other.gameObject);
                break;
            }
        }
    }
}
