using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{

    [SerializeField, Header("左前")]
    [Header("敵オブジェクト生成位置")]
    private GameObject LeftFront;
    [SerializeField, Header("前")]
    private GameObject Front;
    [SerializeField, Header("右前")]
    private GameObject RightFront;
    [SerializeField, Header("左")]
    private GameObject Left;
    [SerializeField, Header("右")]
    private GameObject Right;
    [SerializeField, Header("左後ろ")]
    private GameObject LeftBack;
    [SerializeField, Header("後ろ")]
    private GameObject Back;
    [SerializeField, Header("右後ろ")]
    private GameObject RightBack;


    private Dictionary<string,GameObject> SpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
