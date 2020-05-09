using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakerPillarcBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ピラルクAIの処理を行う
        gameObject.GetComponent<AIWeakerPillarc>().AIUpdate();
    }
}
