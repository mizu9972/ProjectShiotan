using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotest : MonoBehaviour
{
    public SEPlayer sePlayer;
    public BGMPlayer bgmPlayer;
    float x = 0f;
    float z = 0f;
    // Start is called before the first frame update
    void Start()
    {
        bgmPlayer.PlayBgm();
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
            x += 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x -= 0.1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            z += 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            z -= 0.1f;
        }
        transform.position = new Vector3(x, 0f,z);
    }
}
