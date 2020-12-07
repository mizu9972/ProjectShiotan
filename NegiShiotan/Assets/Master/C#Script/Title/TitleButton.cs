using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    private SceneLoader mySceneLoader = null;
    // Start is called before the first frame update
    void Start()
    {
        mySceneLoader = this.gameObject.GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            mySceneLoader.LoadScene();
        }
    }
}
