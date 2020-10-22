using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{

    [SerializeField, Header("左前")]
    [Header("敵オブジェクト生成位置")]
    private GameObject LeftFront = null;
    [SerializeField, Header("前")]
    private GameObject Front = null;
    [SerializeField, Header("右前")]
    private GameObject RightFront = null;
    [SerializeField, Header("左")]
    private GameObject Left = null;
    [SerializeField, Header("中")]
    private GameObject Center = null;
    [SerializeField, Header("右")]
    private GameObject Right = null;
    [SerializeField, Header("左後ろ")]
    private GameObject LeftBack = null;
    [SerializeField, Header("後ろ")]
    private GameObject Back = null;
    [SerializeField, Header("右後ろ")]
    private GameObject RightBack = null;

    //スポーン位置名と配列添え字の対応表----
    /*
      左前/0     前/1     右前/2
       左/3      中/4      右/5
    左後ろ/6    後ろ/7   右後ろ/8
    */
    //-------------------------------


    private Dictionary<int,GameObject> SpawnPoints = new Dictionary<int, GameObject>();
    void Start()
    {
        SpawnPoints[0] = LeftFront;
        SpawnPoints[1] = Front;
        SpawnPoints[2] = RightFront;
        SpawnPoints[3] = Left;
        SpawnPoints[4] = Center;
        SpawnPoints[5] = Right;
        SpawnPoints[6] = LeftBack;
        SpawnPoints[7] = Back;
        SpawnPoints[8] = RightBack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform getEreaTrans(int EreaNumber)
    {
        return SpawnPoints[EreaNumber].transform;
    }
}
