using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Header("コイン増える数")]
    public int UpCoin;

    //コイン数管理マネージャー
    private Status CoinStatus;

    // Start is called before the first frame update
    void Start()
    {
        CoinStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Human"|| other.tag == "Bullet")
        {
            CoinStatus.UpCoin(UpCoin);
            Destroy(this.gameObject);
        }
    }
}
