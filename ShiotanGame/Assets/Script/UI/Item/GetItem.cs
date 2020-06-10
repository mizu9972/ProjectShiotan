using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    Animator MyAnim;

    // Start is called before the first frame update
    void Start()
    {
        MyAnim = this.GetComponent<Animator>();
    }

    public void StartAnim()
    {
        MyAnim.Play("GetItem");
    }
}
