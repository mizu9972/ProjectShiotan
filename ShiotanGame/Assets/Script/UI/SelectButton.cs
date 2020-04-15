using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    [Header("移動先シーン")]
    public string NextScene;

    private Camera SceneManager;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickFunc()
    {
        if(NextScene!=null)//指定した移動先に遷移
        {
            SceneManager.GetComponent<SceneTransition>().SetTransitionRun(NextScene);
        }
    }
}
