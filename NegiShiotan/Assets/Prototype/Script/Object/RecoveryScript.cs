using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryScript : MonoBehaviour
{
    [Header("HP回復する値")]
    public int UpHP;

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

        if (layerName == "Player")
        {
            HPStatus.RecoveryHP
                (UpHP);
            Destroy(this.gameObject);
        }
    }
}
