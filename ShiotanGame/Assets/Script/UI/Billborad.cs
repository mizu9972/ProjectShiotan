using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billborad : MonoBehaviour
{
    private Transform MyTrans = null;
    private Camera mainCamera = null;
    // Start is called before the first frame update
    void Start() {
        MyTrans = this.GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate() {
        MyTrans.LookAt(Camera.main.transform.position);//カメラの方向を向かせる
    }
}
