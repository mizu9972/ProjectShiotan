using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagareAdjustParticle : MonoBehaviour
{

    private ParticleSystem myParticleSystem = null;
    private Transform ParentTrans = null;

    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = this.GetComponent<ParticleSystem>();
        ParentTrans = this.GetComponentInParent<Transform>();
        
    }

}
