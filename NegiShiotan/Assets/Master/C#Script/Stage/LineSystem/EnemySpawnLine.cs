using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLine : MonoBehaviour
{
    [SerializeField, Header("出現させる敵オブジェクト")]
    private GameObject EnemyObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {

        }
    }

    private void SpawnLine()
    {
        if(EnemyObject != null)
        {

        }
    }
}
