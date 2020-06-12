using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMainCamera : MonoBehaviour
{
    Canvas MyCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        MyCanvas = this.GetComponent<Canvas>();
        MyCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        MyCanvas.worldCamera = Camera.main;
        MyCanvas.planeDistance = 1f;
    }
}
