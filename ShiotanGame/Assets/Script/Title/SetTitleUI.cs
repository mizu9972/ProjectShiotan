using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTitleUI : MonoBehaviour
{
    public TitleScene titleScene;
    private bool isEnd=false;//処理が終わった時用
    // Start is called before the first frame update
    void Awake()
    {
        titleScene.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.GetisFade()&&!isEnd)
        {
            titleScene.enabled = true;
            isEnd = true;
        }
    }
}
