using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryScript : MonoBehaviour
{
    [Header("HP回復する値")]
    public int UpHP;

    [Header("HP管理マネージャー")]
    public Status HPStatus;

    [Header("SE:回復のオブジェクトにあたった時")]
    public SEPlayer SE;

    // Start is called before the first frame update
    void Start()
    {
        SE=this.GetComponent<SEPlayer>();
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
            SE.PlaySound();

            HPStatus.RecoveryHP
                (UpHP);
            Destroy(this.gameObject);
        }
    }
}
