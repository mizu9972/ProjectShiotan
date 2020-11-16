using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    public PlayParticle particle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            particle.Play();
            gameObject.SetActive(false);
        }
    }
}
