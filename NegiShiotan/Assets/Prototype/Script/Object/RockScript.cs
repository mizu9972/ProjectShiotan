using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [Header("HP減る量")]
    public int Damage;

    //HP管理マネージャー
    private Status HPStatus;

    // Start is called before the first frame update
    void Start()
    {
        HPStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HPStatus.DamageHP(Damage);
            Destroy(this.gameObject);
        }
    }
}
