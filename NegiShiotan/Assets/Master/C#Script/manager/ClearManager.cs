using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearManager : MonoBehaviour
{
    [SerializeField, Header("リザルト画面キャンバス")]
    private GameObject ResultCanvas = null;

    [SerializeField, Header("クリア演出カメラ位置")]
    private Vector3 cameraPosition = new Vector3(3, 3, 0);
    [SerializeField, Header("クリア演出カメラ角度")]
    private Vector3 cameraRotate = new Vector3(0, 90, 0);
    [SerializeField, Header("カメラの移動速度")]
    private float cameraSpeed = 1.0f;

    [SerializeField, Header("元の位置へ戻るカメラの速度")]
    private float removeCameraSpeed = 1.0f;

    private StageConveyorSystem stageConveyorSystem = null;
    private Sequence clearSeq = null;
    private Camera mainCamera = null;

    //初期のカメラ座標
    private Vector3 defaultPosition;
    private Vector3 defaultRotate;

    private PlayerMove clear;

    private void Awake()
    {
        if(ResultCanvas == null)
        {
            ResultCanvas = GameObject.FindGameObjectWithTag("ClearCanvas");
        }
        ResultCanvas.SetActive(false);
        
    }

    private void Start()
    {
        stageConveyorSystem = GameObject.FindGameObjectWithTag("StageConveyor").GetComponent<StageConveyorSystem>();
        mainCamera = Camera.main;
        defaultPosition = mainCamera.transform.position;
        defaultRotate = mainCamera.transform.rotation.eulerAngles;

        clear = GameObject.FindGameObjectWithTag("Human").GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if(Camera.main.transform.localPosition==cameraPosition)
        {
            clear.SetZoomC();
        }
    }

    //クリア演出
    public void ClearFunction()
    {
        stageConveyorSystem.OnClearSystem();

        {
            clearSeq = DOTween.Sequence();
            clearSeq.Append(
                mainCamera.transform.DOMove(cameraPosition, cameraSpeed)
                )
                .Join(
                mainCamera.transform.DORotate(cameraRotate, cameraSpeed)
                );

            clearSeq.Play();
            PlayDirecting();
        }
    }

    //クリア演出再生
    private void PlayDirecting()
    {
        //カメラ追跡処理　停止
        Camera.main.GetComponent<PlayerCamera>().enabled = false;

        //プレイヤーアニメーション開始

        //クリア画像表示
        ResultCanvas.SetActive(true);
        
        //エフェクト再生

        //BGM再生

        //SE再生

    }

    //元のカメラ位置へ戻る
    public void moveToDefaultCamera()
    {
        clearSeq = DOTween.Sequence();
        clearSeq.Append(
            mainCamera.transform.DOMove(defaultPosition, removeCameraSpeed)
            )
            .Join(
            mainCamera.transform.DORotate(defaultRotate, removeCameraSpeed)
            );

        clearSeq.Play();
    }

}
