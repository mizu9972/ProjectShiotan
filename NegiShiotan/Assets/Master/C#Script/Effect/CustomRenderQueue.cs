using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class CustomRenderQueue : MonoBehaviour
{
    [SerializeField]
    private int renderQueue = 2000;

    private void Awake()
    {
        GetComponent<Renderer>().material.renderQueue = renderQueue;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
