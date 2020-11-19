using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModel : MonoBehaviour
{
    float x=0f;
    float y=0f;
    float z=0f;
    Vector3 Angle = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        Angle = gameObject.transform.eulerAngles;
        x = Angle.x;
        y = Angle.y;
        z = Angle.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            y++;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            y--;
        }
        Angle = new Vector3(x, y, z);
        gameObject.transform.eulerAngles = Angle;
    }
}
