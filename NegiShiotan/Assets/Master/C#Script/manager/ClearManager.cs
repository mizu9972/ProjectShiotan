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

    private StageConveyorSystem stageConveyorSystem = null;
    private Sequence clearSeq = null;
    private Camera mainCamera = null;
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
    }

    //クリア演出
    public void ClearFunction()
    {
        ResultCanvas.SetActive(true);
        stageConveyorSystem.OnClearSystem();

        {
            clearSeq = DOTween.Sequence();
            clearSeq.Append(
                mainCamera.transform.DOMove(cameraPosition, cameraSpeed)
                )
                .Join(
                mainCamera.transform.DORotate(cameraRotate,cameraSpeed)
                );

        }
    }

}
