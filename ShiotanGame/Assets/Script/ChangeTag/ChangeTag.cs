using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTag : MonoBehaviour
{
    [Header("変更前のタグ名")]
    public string BeforTag;
    [Header("変更後のタグ名")]
    public string AfterTag;
    // Start is called before the first frame update
    void Start()
    {
        if(BeforTag!=null)
        {
            this.tag = BeforTag;//最初のタグを設定
        }
    }

    public void ChangeTagName()//タグ名を変更する
    {
        if(AfterTag!=null)
        {
            Debug.Log("Change");
            this.tag = AfterTag;
        }
        else
        {
            Debug.Log("変更後のタグ名が設定されてません。");
        }
    }
}
