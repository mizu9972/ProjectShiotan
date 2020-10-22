using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallCamera : MonoBehaviour
{
    //作業用
    [SerializeField, Header("座標メモ用")]
    private Vector3 tempPosition;
    //-----

    [SerializeField, Header("落ちる時の目標座標")]
    private Vector3 FallingPosition = new Vector3(9, 10, -0.4f);
    [SerializeField, Header("移動速度")]
    private float MoveSpeed = 1.0f;
    [SerializeField, Header("元の位置に戻る速度")]
    private float BackSpeed = 3.0f;
    private Vector3 m_DefaultPosition;//元の座標

    [SerializeField, Header("落ちる時の目標X角度")]
    private float FallingRotationX = 75.0f;
    [SerializeField, Header("回転速度")]
    private float RotateSpeed = 1.0f;
    [SerializeField, Header("元の角度に戻る回転速度")]
    private float ReverseSpeed = 1.5f;
    private float m_DefaultRotateX;//元の回転角度


    [SerializeField, Header("落ちている最中のカメラ停止時間の高さ倍率")]
    private float StopTimeRate = 0.00035f;

    private Camera m_myCamera;//カメラ
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

    [ContextMenu("落下カメラ")]
    public void StartRotateTween(float FallHeight)
    {
        //元の位置の保存
        m_DefaultPosition = m_myCamera.transform.position;

        //落下地点の上空へ移動した後元の位置へ戻る
        // +
        //下を向いた後元の方向を向く
        m_CameraFallSequence = DOTween.Sequence();
        m_CameraFallSequence.Append(//下を向く
    transform.DORotate(new Vector3(FallingRotationX, 0, 0), 1.0f / RotateSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuart).SetRelative()
    )
    .Join(//上空へ移動
    transform.DOMove(FallingPosition,1.0f / MoveSpeed).SetEase(Ease.InOutQuart)
    )
    .AppendInterval(FallHeight * StopTimeRate)//落下中は停止
    .Append(//角度を戻す
    transform.DORotate(new Vector3(-FallingRotationX, 0, 0), 1.0f / ReverseSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).SetRelative()
    )
    .Join(//位置を戻す
    transform.DOMove(m_DefaultPosition,1.0f /BackSpeed).SetEase(Ease.Linear)
    );
        m_CameraFallSequence.Play();

    }
}
