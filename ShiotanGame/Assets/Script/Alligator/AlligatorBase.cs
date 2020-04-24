using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlligatorBase : MonoBehaviour
{
    void Start() {
    }

    void Update() {
        // ワニAIの処理を行う
        gameObject.GetComponent<AIAlligator>().AIUpdate();
    }
}
