using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiScript : MonoBehaviour
{
    public GameObject Bui;

    public float speed;
    public float rbspeed;

    //移動フラグ用変数
    private int MoveOn;

    // Rigidbodyコンポーネントを入れる変数"rb"を宣言する。
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;


        // 左ボタンクリック
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = (1 << LayerMask.NameToLayer("Hit"));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //レイが当たった位置を得るよ
                Vector3 clickpos = hit.point;
                Bui.transform.position = clickpos;
            }
        }

        //移動フラグ　初期化
        MoveOn = 0;

        //回転の度合い
        float step = 3.0f * Time.deltaTime;

        //コントローラー入力　取得
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

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

        //myTransform.position = pos;  // 座標を設定
    }
}
