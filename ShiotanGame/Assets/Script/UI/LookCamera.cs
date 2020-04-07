using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Transform MyTrans = null;

    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        MyTrans.rotation = Camera.main.transform.rotation;//カメラの方向を向く
    }
}
