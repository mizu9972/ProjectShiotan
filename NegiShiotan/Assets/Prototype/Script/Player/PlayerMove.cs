using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed = 1f;
    private float Zpos;
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
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Zpos += Speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            Zpos -= Speed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, Zpos);
    }
}
