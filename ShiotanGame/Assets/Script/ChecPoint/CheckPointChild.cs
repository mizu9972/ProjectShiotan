using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointChild : MonoBehaviour
{
    [SerializeField, Header("チェックポイントが起動したか")]
    private bool isChecked = false;

    private GameObject respawnObj = null;
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
            isChecked = true;
        }
    }

    public bool GetisChecked()
    {
        return isChecked;
    }

    public void SetRespawnPosition()//リスポーン地点をチェックポイントの位置に変更
    {
        respawnObj.transform.position = this.transform.position;
    }
}
