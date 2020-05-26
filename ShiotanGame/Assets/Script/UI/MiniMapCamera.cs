using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    private Transform Target = null;
    private Transform MyTrans;
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!Target)
        {
            Target = GameManager.Instance.GetPlayer().transform;
        }
        MyTrans.position = new Vector3(Target.position.x,MyTrans.position.y,Target.position.z);
    }
}
