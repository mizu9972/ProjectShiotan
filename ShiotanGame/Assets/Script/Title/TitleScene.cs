using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    SceneTransition TransitionScript;
    [Header("遷移先シーン名")]
    public string NextSceneName = null;
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
            TransitionScript.SetTransitionRun(NextSceneName);
        }
    }
}
