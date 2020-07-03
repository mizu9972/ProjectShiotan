using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    public List<Transform> waypoint;
    public int count = 0;
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private bool isMoving = false;
    public PlayAnimation playAnimation;
    private void Start()
    {
        playAnimation.StartAnimation();
    }
    void Update()
    {
        KeyInput();

        Vector3 d = waypoint[count].transform.position - transform.position;
        if (d.magnitude < speed * Time.deltaTime)
        {
            isMoving = false;
            transform.position += d;
            
            return;
        }
        d.Normalize();
        transform.position += d * Time.deltaTime * speed;
        isMoving = true;
    }

    void KeyInput()
    {
        if(!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                count += 1;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                count -= 1;
            }
        }
        if (count >= waypoint.Count)
        {
            count = waypoint.Count - 1;
        }
        if (count <= 0)
        {
            count = 0;
        }
    }
}