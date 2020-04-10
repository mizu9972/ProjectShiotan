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

    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;
    void Start()
    {
        resscript = GameObject.Find("Respawn").GetComponent<RespawnScript>();
        HPcnt = this.GetComponent<HumanoidBase>();
        time = 0;
    }

    void Update()
    {
        if(HPcnt.NowHP<= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        resscript.Respawn = true;
    }

    //当たるのをやめたとき
    void OnCollisionExit(Collision other)
    {
        time = 0;
    }

    public float GetNowHP()
    {
        return HPcnt.NowHP;
    }
}
