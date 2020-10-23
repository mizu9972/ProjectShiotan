using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Header("コイン増える数")]
    public int UpCoin;

    [Header("コイン数管理マネージャー")]
    public Status CoinStatus;

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
            CoinStatus.UpCoin(UpCoin);
            Destroy(this.gameObject);
        }
    }
}
