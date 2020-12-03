using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectCamera : MonoBehaviour
{
    [SerializeField, Header("衝撃の強さ")]
    private float ShakeImpact = 1.0f;
    [SerializeField, Header("衝撃の長さ")]
    private float ShakeDuration = 0.5f;

    private Transform myTrans = null;
    Tween CameraTween;
    // Start is called before the first frame update
    void Start()
    {
        myTrans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        CameraTween = myTrans.DOShakePosition(ShakeDuration, ShakeImpact);
    }
}
