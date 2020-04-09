using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    [SerializeField,Header("何秒でダメージはいるか")]
    private float DamageTime;
    
    private HumanoidBase HPcnt;

    private float time;

    private RespawnScript resscript;

    void Start()
    {
        resscript = GameObject.Find("Respawn").GetComponent<RespawnScript>();
        HPcnt = this.GetComponent<HumanoidBase>();
        time = 0;
    }

    void Update()
    {
        if(HPcnt.HP<= 0)
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
            HPcnt.HP--;
            time = 0;
        }
        if (layerName == "Fish" && time > DamageTime)
        {
            HPcnt.HP--;
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
        return HPcnt.HP;
    }
}
