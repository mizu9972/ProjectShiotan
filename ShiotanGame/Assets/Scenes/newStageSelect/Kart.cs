using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Kart : MonoBehaviour
{
    public List<GameObject> waypoint;
    public int count = 0;
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private bool isMoving = false;
    public PlayAnimation playAnimation;

    [SerializeField]
    private bool isControll = false;

    
    private KartCamera kartCamera;

    private InputStick inputStick;

    private bool isAnim = false;

    private void Start()
    {
        inputStick = new InputStick();
        
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
                inputStick.StickUpdate();
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("MenuSelect"))
                {
                    string stageName = waypoint[count].GetComponent<GetStageName>().GetName();
                    Camera.main.GetComponent<SceneTransition>().SetTransitionRun(stageName);
                    AudioManager.Instance.PlaySE("SE_ENTER");
                    this.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) || inputStick.GetRightStick())
                {
                    SetRotationRight();
                    count += 1;
                    CheckValue();
                    if (count > 0 && waypoint[count - 1].GetComponent<StageObject>().isEnd)
                    {
                        kartCamera.AddCount();
                    }
                    playAnimation.StartAnimation();
                    AudioManager.Instance.PlaySE("SE_EOW");
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow)|| inputStick.GetLeftStick())
                {
                    SetRotationLeft();
                    count -= 1;
                    CheckValue();
                    if (count > 0 && waypoint[count + 1].GetComponent<StageObject>().isStart)
                    {
                        kartCamera.SubCount();
                    }
                    playAnimation.StartAnimation();
                    AudioManager.Instance.PlaySE("SE_EOW");
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
        if (count < 0)
        {
            count = 0;
        }
    }

    public bool GetisMoving()
    {
        return isMoving;
    }

    private void SetRotationLeft()
    {
        transform.eulerAngles = new Vector3(0f,180f,0f);
    }

    private void SetRotationRight()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}