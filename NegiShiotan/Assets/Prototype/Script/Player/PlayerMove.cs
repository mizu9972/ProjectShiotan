using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed = 1f;

    [Header("左側の移動制限用コライダ")]
    public HitCheck LeftCollider;

    [Header("右側の移動制限用コライダ")]
    public HitCheck RightCollider;

    private float Zpos;

    [SerializeField,Header("イカダのどの位置にいるか")]
    private Vector2 OnRaftPosition;//イカダのどの位置にいるか

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveFunc();   
    }


    private void MoveFunc()
    {
        //←入力がある状態で左側が飛び出てなければ
        if (Input.GetKey(KeyCode.LeftArrow) && LeftCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x, 
                                             transform.position.y, 
                                             transform.position.z + (Speed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.RightArrow) && RightCollider.GetisHit())
        {
            transform.position = new Vector3(transform.position.x, 
                                             transform.position.y, 
                                             transform.position.z + (-Speed * Time.deltaTime));
        }
    }

    public void SetRaftPosition(Vector2 pos)
    {
        //イカダのどこにいるかをセット
        OnRaftPosition = pos;
    }
}
