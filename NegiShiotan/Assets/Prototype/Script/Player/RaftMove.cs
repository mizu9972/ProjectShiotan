using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftMove : MonoBehaviour
{
    [Header("あそびのOn,Off")]
    public bool UseRange = true;
    [Header("プレイヤーの位置による移動の割合")]
    public float MoveRate;

    [Header("Z軸方向のスピード(左が+、右が-)")]
    public float RaftSpead;

    [Header("プレイヤーの中心からのあそび")]
    public float Range;

    [SerializeField, Header("Z座標")]
    private float ZPos;

    [SerializeField, Header("プレイヤーがイカダのどこにいるか")]
    private Vector2 OnPlayerPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveMain();
    }

    public void SetOnPlayerPos(Vector2 pos)
    {
        OnPlayerPos = pos;
    }

    private void MoveMain()
    {
        if(UseRange)//あそびあり
        {
            //あそびの部分の範囲を超えていたら
            if ((OnPlayerPos.y - 0.5f) <= -Range || (OnPlayerPos.y - 0.5f) >= Range)
            {
                //プレイヤーの位置によってスピードを決定
                RaftSpead = (OnPlayerPos.y - 0.5f) * MoveRate;

                //Z座標用変数に代入
                ZPos -= RaftSpead * Time.deltaTime;

                //実際の座標に代入
                transform.position = new Vector3(transform.position.x, transform.position.y, ZPos);
            }
        }
        else//あそびなし
        {
            //プレイヤーの位置によってスピードを決定
            RaftSpead = (OnPlayerPos.y - 0.5f) * MoveRate;

            //Z座標用変数に代入
            ZPos -= RaftSpead * Time.deltaTime;

            //実際の座標に代入
            transform.position = new Vector3(transform.position.x, transform.position.y, ZPos);
        }
    }
}
