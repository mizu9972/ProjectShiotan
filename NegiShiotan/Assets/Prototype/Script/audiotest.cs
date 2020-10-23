using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotest : MonoBehaviour
{
    public SEPlayer sePlayer;
    float x, y, z = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            sePlayer.PlaySound();
        }
        if(Input.GetKey(KeyCode.A))
        {
            x += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x -= 1;
        }
        transform.position = new Vector3(x, y, z);
    }
}
