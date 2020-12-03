using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [Header("HP減る量")]
    public int Damage;

    //HP管理マネージャー
    private Status HPStatus;

    private EffectCamera effectCamera;

    // Start is called before the first frame update
    void Start()
    {
        HPStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
        effectCamera = Camera.main.GetComponent<EffectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            effectCamera.Shake();
            HPStatus.DamageHP(Damage,true);
            Destroy(this.gameObject);
        }
    }
}
