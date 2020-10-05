using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject PlayerObj;

    [Header("イカダ")]
    public GameObject RaftObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //////////////////////////////////
    // プレイヤーとイカダ取得用関数
    //////////////////////////////////
    public GameObject GetPlayer()
    {
        return PlayerObj;
    }
    public GameObject GetRaft()
    {
        return RaftObj;
    }
}
