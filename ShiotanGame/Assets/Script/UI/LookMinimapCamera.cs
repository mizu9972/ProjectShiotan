using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookMinimapCamera : MonoBehaviour
{
    private Transform MyTrans = null;
    private Vector3 work_Pos = Vector3.zero;
    
    public Transform parentTransform = null;//親オブジェクト
    private float YAngle = 0;//親の回転を打ち消す用

    [Header("追いかけるカメラ")]
    public Camera TargetCamera = null;
    [Header("Y軸回転補正値")]
    public float ModYAngle = 40f;
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
        TargetCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //親オブジェクトのY軸回転
        YAngle = parentTransform.eulerAngles.y;

        MyTrans.LookAt(TargetCamera.transform.position);//カメラの方向を向かせる

        //親オブジェクトの回転を打ち消してY軸は固定、カメラのX回転を反映
        YAngle = parentTransform.localEulerAngles.y+ModYAngle;
        MyTrans.localEulerAngles = new Vector3(0f, -YAngle, MyTrans.localEulerAngles.z);
    }
}
