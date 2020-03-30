using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [Header("発生するオブジェクト")]
    public GameObject RespawnPrefab;

    //リスポーン確認用
    public bool Respawn;

    // Start is called before the first frame update
    void Start()
    {
        Respawn = false;
        var RespawnInstance = Instantiate<GameObject>(RespawnPrefab, this.transform.position, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //リスポーンするか？
        if (Respawn)
        {
            Respawn = false;
            var RespawnInstance = Instantiate<GameObject>(RespawnPrefab, this.transform.position, this.transform.rotation);
        }
    }
}
