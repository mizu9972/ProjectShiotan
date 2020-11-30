using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

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

    [SerializeField, Header("イカダ　幅")]
    public float IkadaWidth;

    [SerializeField, Header("ステージ右壁")]
    public GameObject RightWall;

    [SerializeField, Header("ステージ左壁")]
    public GameObject LeftWall;

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

                if (ZPos+ IkadaWidth > LeftWall.transform.position.z)
                {
                    ZPos = LeftWall.transform.position.z- IkadaWidth;
                }
                if (ZPos- IkadaWidth < RightWall.transform.position.z)
                {
                    ZPos = RightWall.transform.position.z+ IkadaWidth;
                }

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

            if (ZPos + IkadaWidth > LeftWall.transform.position.z)
            {
                ZPos = LeftWall.transform.position.z - IkadaWidth;
            }
            if (ZPos - IkadaWidth < RightWall.transform.position.z)
            {
                ZPos = RightWall.transform.position.z + IkadaWidth;
            }

            //実際の座標に代入
            transform.position = new Vector3(transform.position.x, transform.position.y, ZPos);
        }
    }

    //画面奥へ移動させる
    public void moveFar()
    {
        Tween moveTween;

        moveTween = transform.DOMoveX(1000f, 100.0f)
            .SetRelative();

        moveTween.Play();
    }
}
