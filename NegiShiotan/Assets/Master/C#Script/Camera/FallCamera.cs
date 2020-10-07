using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallCamera : MonoBehaviour
{

    [SerializeField, Header("落ちる時の目標X角度")]
    float FallingRotationX;

    [SerializeField, Header("回転速度")]
    float RotateSpeed = 1.0f;
    [SerializeField, Header("元の角度に戻る回転速度")]
    float ReverseSpeed = 1.0f;
    private Camera m_myCamera;//カメラ
    private float m_DefaultRotateX;

    ////カメラ挙動用Tween
    //Tween m_CameraFallTween;
    Sequence m_CameraFallSequence;

    private void Awake()
    {
        m_myCamera = this.gameObject.GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_DefaultRotateX = m_myCamera.transform.localEulerAngles.x;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("回転")]
    public void StartRotateTween()
    {

        //下を向いた後元の方向を向く
        m_CameraFallSequence = DOTween.Sequence();
        m_CameraFallSequence.Append(
    transform.DORotate(new Vector3(FallingRotationX, 0, 0), 1.0f / RotateSpeed, RotateMode.LocalAxisAdd)
    .SetEase(Ease.InBack).SetRelative()
    )
    .Append(
    transform.DORotate(new Vector3(-FallingRotationX, 0, 0), 1.0f / ReverseSpeed, RotateMode.LocalAxisAdd)
    .SetEase(Ease.Linear).SetRelative()
    );
        m_CameraFallSequence.Play();

    }
}
