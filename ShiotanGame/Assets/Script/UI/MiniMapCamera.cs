using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [Header("Z座標の上限")]
    public float MaxZPos;

    [Header("Z座標の下限")]
    public float MinZPos;

    [Header("移動範囲制限を使用するか")]
    public bool isUseLimit;
    private Transform Target = null;
    private Transform MyTrans;
    private float x, y, z;
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
        //移動範囲制限設定
        if(isUseLimit)
        {
            SetPosition();
        }
        else
        {
            MyTrans.position = new Vector3(Target.position.x, MyTrans.position.y, Target.position.z);
        }
    }

    private void SetPosition()
    {
        float max = Camera.main.GetComponent<ChaceCamera>().GetMaxValue().x;
        float min = Camera.main.GetComponent<ChaceCamera>().GetMinValue().x;
        x = Mathf.Clamp(x, min, max);
        y = this.transform.position.y;
        z = Mathf.Clamp(z, MinZPos, MaxZPos);
        MyTrans.position = new Vector3(x,y,z);
    }
}
