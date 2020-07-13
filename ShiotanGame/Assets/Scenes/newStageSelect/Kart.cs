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

    [SerializeField]
    private bool isControll = false;

    private KartCamera kartCamera;

    private void Start()
    {
        if(playAnimation!=null)
        {
            playAnimation.StartAnimation();
        }
        kartCamera = Camera.main.GetComponent<KartCamera>();
    }
    void Update()
    {
        //カメラ動作中は操作不可、カメラ非動作中は操作可
        if(!GameManager.Instance.GetisFade())
        {
            isControll = !kartCamera.GetisMoving();
        }
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
        if(isControll)
        {
            if (!isMoving)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("MenuSelect"))
                {
                    string stageName = waypoint[count].GetComponent<GetStageName>().GetName();
                    Camera.main.GetComponent<SceneTransition>().SetTransitionRun(stageName);
                    this.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    count += 1;
                    CheckValue();
                    if (count > 0 && waypoint[count - 1].GetComponent<StageObject>().isEnd)
                    {
                        kartCamera.AddCount();
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    count -= 1;
                    CheckValue();
                    if (count > 0 && waypoint[count + 1].GetComponent<StageObject>().isStart)
                    {
                        kartCamera.SubCount();
                    }
                }
            }
        }
        
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
}