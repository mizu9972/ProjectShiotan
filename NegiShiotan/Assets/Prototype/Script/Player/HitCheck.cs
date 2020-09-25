using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    [SerializeField, Header("衝突中か")]
    private bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetisHit()//衝突中かを返す
    {
        return isHit;
    }

    private void OnTriggerStay(Collider other)//ヒットチェック
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "Player_Raft")
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player_Raft")
        {
            isHit = false;
        }
    }
}
