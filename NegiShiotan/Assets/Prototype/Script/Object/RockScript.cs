using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [Header("HP減る量")]
    public int Damage;

    [Header("HP管理マネージャー")]
    public Status HPStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Player_Raft")
        {
            HPStatus.DamageHP(Damage);
            Destroy(this.gameObject);
        }
    }
}
