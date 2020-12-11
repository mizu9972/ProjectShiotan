using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class RaftTilt : MonoBehaviour
{
    [SerializeField, Header("傾かせるモデル")]
    private Transform MarutaParent = null;

    [SerializeField, Header("プレイヤーによる傾きの度合い")]
    private float TiltRatebyPlayer = 1.0f;

    [SerializeField, Header("ピラルクによる傾きの度合い")]
    private float TiltRatebyPirarucu = 1.0f;

    //傾き計算用
    private Vector3 TiltRotatebyPlayer = new Vector3(0, 0, 0);
    private Vector3 TiltRotatebyPirarucu = new Vector3(0, 0, 0);

    //丸太の角度
    private Vector3 marutaRotate;

    private void Update()
    {
        //角度取得
        marutaRotate = MarutaParent.localRotation.eulerAngles;
        //反映
        MarutaParent.localRotation = Quaternion.Euler(TiltRotatebyPlayer + TiltRotatebyPirarucu);
    }

    public void TiltbyPlayerPosition(float playerPos)
    {
        TiltRotatebyPlayer = new Vector3(marutaRotate.x, marutaRotate.y, -1 * playerPos * TiltRatebyPlayer);

    }

    public void TiltbyPirarucuPosition(float pirarucuPos)
    {
        TiltRotatebyPirarucu = new Vector3(marutaRotate.x, marutaRotate.y, -1 * pirarucuPos * TiltRatebyPirarucu);
    }
}
