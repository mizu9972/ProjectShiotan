using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    [Header("何秒でダメージはいるか")]
    public float DamageTime;

    [Header("体力")]
    public float HP;

    private float time;

    private RespawnScript resscript;

    void Start()
    {
        resscript = GameObject.Find("Respawn").GetComponent<RespawnScript>();
        time = 0;
    }

    void Update()
    {
        if(HP<=0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        resscript.Respawn = true;
    }

    //当たっているとき常時判定
    void OnCollisionStay(Collision other)
    {
        //HP減るまでの時間
        time += 3.0f * Time.deltaTime;
        
        //当たっているもののレイヤー取得
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Esa" && time > DamageTime)
        {

        }
        if (layerName == "Enemy" && time > DamageTime)
        {
            HP--;
            time = 0;
        }
        if (layerName == "Fish" && time > DamageTime)
        {
            HP--;
            time = 0;
        }
    }

    //当たるのをやめたとき
    void OnCollisionExit(Collision other)
    {
        time = 0;
    }

    public float GetNowHP()
    {
        return HP;
    }
}
