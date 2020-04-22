using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Transform MyTrans = null;
    private Vector3 work_Pos = Vector3.zero;
    private Camera mainCamera = null;
    public Transform parentTransform = null;//親オブジェクト
    private float YAngle = 0;//親の回転を打ち消す用
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
        mainCamera = Camera.main;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        //親オブジェクトのY軸回転
        YAngle = parentTransform.eulerAngles.y;
        
        MyTrans.LookAt(Camera.main.transform.position);//カメラの方向を向かせる

        //親オブジェクトの回転を打ち消してY軸は固定、カメラのX回転を反映
        YAngle = parentTransform.localEulerAngles.y;
        MyTrans.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, -YAngle, MyTrans.localEulerAngles.z);
    }
}