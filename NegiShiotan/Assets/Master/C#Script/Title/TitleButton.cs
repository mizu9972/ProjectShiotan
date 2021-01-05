using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    [SerializeField, Header("SEPlayer")]
    private SEPlayer TitleSEPlayer = null;
    private SceneLoader mySceneLoader = null;

    private bool isLoading = false;
    // Start is called before the first frame update
    void Start()
    {
        isLoading = false;  
        mySceneLoader = this.gameObject.GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (isLoading == false)
            {
                TitleSEPlayer.PlaySound();
                isLoading = true;
            }
            mySceneLoader.LoadScene();
        }
    }
}
