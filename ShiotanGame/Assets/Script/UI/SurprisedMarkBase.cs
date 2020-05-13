using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurprisedMarkBase : MonoBehaviour
{
    [SerializeField, Header("生成してから削除するまでの時間")]
    private float DeleteTime;
    private float Count = 0.0f; // 削除タイム

    // Update is called once per frame
    void Update()
    {
        if(DeleteTime <= Count) {
            Destroy(gameObject);
        }
        Count += Time.deltaTime;
    }
}
