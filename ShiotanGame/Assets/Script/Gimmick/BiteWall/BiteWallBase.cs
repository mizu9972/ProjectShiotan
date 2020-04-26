using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteWallBase : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // HPが0以下になったら削除
        if (gameObject.GetComponent<HumanoidBase>().DeadCheck()) {
            Destroy(gameObject);
        }
    }
}
