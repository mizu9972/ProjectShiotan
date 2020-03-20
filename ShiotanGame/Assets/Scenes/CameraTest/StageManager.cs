using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField, Header("ステージの幅")]
    private float StageWidth = 1.0f;

    [SerializeField, Header("ステージの幅")]
    private float StageDepth = 1.0f;

    private Transform MyTrans;
    // Start is called before the first frame update
    void Start()
    {
        MyTrans = this.GetComponent<Transform>();
        MyTrans.localScale = new Vector3(StageWidth, 1.0f, StageDepth);
    }

    // Update is called once per frame
    void Update()
    {
        MyTrans.localScale = new Vector3(StageWidth, 1.0f, StageDepth);
    }
}
