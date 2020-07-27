using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliderSize : MonoBehaviour
{
    public TutorialColliderSize setSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            setSize.SetisMaxSize(true);
            this.gameObject.SetActive(false);
        }
    }
}
