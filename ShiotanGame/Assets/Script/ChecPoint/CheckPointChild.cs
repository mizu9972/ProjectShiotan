using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointChild : MonoBehaviour
{
    [SerializeField, Header("チェックポイントが起動したか")]
    private bool isChecked = false;

    [Header("回復するHP")]
    public float AddHP;

    [Header("回復するエサの数")]
    public float AddFood;

    private GameObject respawnObj = null;
    [Header("リスポーン地点の移動先")]
    public GameObject MoveRespown;

    [Header("リスポーン地点の角度")]
    public float Respown_YAngle;
    // Start is called before the first frame update
    void Start()
    {
        respawnObj = GetComponentInParent<CheckPoint>().GetRespawnObj();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)//プレイヤーが当たったらチェックポイント起動
    {
        if(other.tag=="Player")
        {
            //エサとHPを回復
            other.GetComponent<Player>().AddFoods(AddFood);
            other.GetComponent<Player>().AddHp(AddHP);
            isChecked = true;
        }
    }

    public bool GetisChecked()
    {
        return isChecked;
    }

    public void SetRespawnPosition()//リスポーン地点をチェックポイントの位置に変更
    {
        respawnObj.transform.position = MoveRespown.transform.position;
        respawnObj.transform.eulerAngles = new Vector3(0,Respown_YAngle,0);
    }
}
