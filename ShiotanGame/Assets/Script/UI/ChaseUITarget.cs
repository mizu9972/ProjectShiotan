using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseUITarget : MonoBehaviour
{
    private RectTransform MyRect;
    [Header("ターゲット")]
    public RectTransform TargetRect;

    [Header("Xの補正値")]
    public float XCorrection;
    // Start is called before the first frame update
    void Start()
    {
        MyRect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        float x, y, z = 1f;

        x = TargetRect.position.x+XCorrection;
        y = MyRect.position.y;
        z = MyRect.position.z;

        MyRect.position = new Vector3(x, y, z);
    }
}
