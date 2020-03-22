using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiScript : MonoBehaviour
{
    public float speed;
    public float rbspeed;

    //移動フラグ用変数
    private int MoveOn;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;


        //左へ進む
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = new Vector3(-rbspeed, 0, 0);
            rb.AddForce(force, ForceMode.Force);
            pos.x -= speed;
        }

        //右へ進む
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 force = new Vector3(rbspeed, 0, 0);
            rb.AddForce(force, ForceMode.Force);
            pos.x += speed;
        }

        //上へ進む
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 force = new Vector3(0, 0, rbspeed);
            rb.AddForce(force, ForceMode.Force);
            pos.z += speed;
        }

        //下へ進む
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 force = new Vector3(0, 0, -rbspeed);
            rb.AddForce(force, ForceMode.Force);
            pos.z -= speed;
        }


        // マウス左クリック
        if (Input.GetMouseButtonUp(0))
        {
            //カメラから例をとばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //レイヤー"Hit"のオブジェクトに当たったか
            int layerMask = (1 << LayerMask.NameToLayer("Hit"));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //レイが当たった位置を得る
                Vector3 clickpos = hit.point;
                this.transform.position = clickpos;

                //Rigidbodyを停止
                rb.velocity = Vector3.zero;
            }
        }

        //マウス右クリック
        if (Input.GetMouseButtonUp(1))
        {
            if (rb.useGravity == false)
            {
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
                pos.y += 5;
                this.transform.position = pos;
            }
        }
    }
}
