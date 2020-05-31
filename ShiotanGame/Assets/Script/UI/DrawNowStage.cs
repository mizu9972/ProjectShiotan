using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DrawNowStage : MonoBehaviour
{
    private Text Mytext;
    // Start is called before the first frame update
    void Start()
    {
        Mytext = this.GetComponent<Text>();
        Mytext.text = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
