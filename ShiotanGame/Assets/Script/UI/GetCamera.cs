using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetCamera : MonoBehaviour
{
    Canvas MyCanvas = null;
    Camera mainCamera = null;
    // Start is called before the first frame update
    void Start()
    {
        MyCanvas = this.GetComponent<Canvas>();
        mainCamera = Camera.main;
        MyCanvas.worldCamera = mainCamera;
        MyCanvas.planeDistance = 1f;
    }

    public Canvas GetCanvas()
    {
        return MyCanvas;
    }
}
