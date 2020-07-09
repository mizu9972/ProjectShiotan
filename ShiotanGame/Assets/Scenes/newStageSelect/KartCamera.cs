using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartCamera : MonoBehaviour
{
    [Header("Z値の固定値")]
    public float defZPos;
    public List<Transform> waypoint;
    public int count = 0;
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private bool isMoving = false;

    private void Start()
    {

    }
    void Update()
    {
        CheckValue();

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


    void CheckValue()//値をチェックして最大値、最小値の設定を適用
    {
        if (count >= waypoint.Count)
        {
            count = waypoint.Count - 1;
        }
        if (count <= 0)
        {
            count = 0;
        }
    }

    public bool GetisMoving()
    {
        return isMoving;
    }

    public void AddCount()//カウント加算
    {
        count += 1;
    }
    public void SubCount()//カウント減算
    {
        count -= 1;
    }
}
