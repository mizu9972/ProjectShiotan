using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoatImg : MonoBehaviour
{
    [Header("開始オブジェクト")]
    public RectTransform StartObj;

    [Header("終了オブジェクト")]
    public RectTransform EndObj;

    [Header("動かすオブジェクト")]
    public RectTransform MoveObj;

    [Header("HPゲージ")]
    public GameObject Gage;

    private float Proportion = 0f;
    private float YPos;
    // Start is called before the first frame update
    void Start()
    {
        YPos = MoveObj.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Proportion = Gage.GetComponent<Gage>().GetProportion();
        Proportion = 1f - Proportion;
        MoveObj.position = Vector3.Slerp(StartObj.position, EndObj.position, Proportion);
        MoveObj.position = new Vector3(MoveObj.position.x, YPos, MoveObj.position.z);
    }
}
