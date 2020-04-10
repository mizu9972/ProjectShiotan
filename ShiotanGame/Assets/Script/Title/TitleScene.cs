using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    SceneTransition TransitionScript;
    // Start is called before the first frame update
    void Start()
    {
        TransitionScript = this.GetComponent<SceneTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            TransitionScript.SetTransitionRun();
        }
    }
}
