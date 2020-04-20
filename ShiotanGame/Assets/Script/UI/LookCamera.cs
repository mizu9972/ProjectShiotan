using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Transform MyTrans = null;
    private Vector3 work_Pos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        work_Pos = MyTrans.localPosition;
        MyTrans.localPosition = new Vector3(0, 0, 0);
        MyTrans.LookAt(Camera.main.transform.position);
        MyTrans.localPosition = work_Pos;
    }
}
