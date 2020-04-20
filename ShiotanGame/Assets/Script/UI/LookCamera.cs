using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Transform MyTrans = null;
    private Vector3 work_Pos = Vector3.zero;
    private Camera mainCamera = null;
    public Transform parentTransform = null;
    private float posy = 0;
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
        mainCamera = Camera.main;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {

        //MyTrans.eulerAngles = new Vector3(60, 0, 0);
        posy = parentTransform.eulerAngles.y;
        //MyTrans.LookAt(mainCamera.transform);
        //posy = posy - MyTrans.eulerAngles.y;
        MyTrans.LookAt(Camera.main.transform.position);
        posy = parentTransform.localEulerAngles.y;
        MyTrans.localEulerAngles = new Vector3(MyTrans.localEulerAngles.x, -posy , MyTrans.localEulerAngles.z);
    }
}
