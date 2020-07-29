using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColliderSize : MonoBehaviour
{
    [Header("Maxのサイズ")]
    public float MaxSize = 1f;

    public bool isMaxSize = false;


    private void Update()
    {
        //マックスサイズの状態でなければ半分のサイズでサイズを設定
        if (!isMaxSize)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (MaxSize / 2f));
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, MaxSize);
        }
    }
    public void SetisMaxSize(bool val)
    {
        isMaxSize = val;
    }
}
