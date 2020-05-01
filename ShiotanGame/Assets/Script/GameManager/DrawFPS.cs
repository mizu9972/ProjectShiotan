using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFPS : MonoBehaviour
{
    [Header("FPSを描画するか")]
    public bool isDraw = true;

    private int frameCount = 0;
    private float prevTime = 0.0f;
    private float fps;

    // Start is called before the first frame update
    void Start()
    {
        if(!isDraw)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;
            Debug.Log(fps);

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }

    // 表示処理
    private void OnGUI()
    {
        if(isDraw)
        {
            GUI.skin.label.fontSize = 100;
            GUILayout.Label(fps.ToString());
        }
    }
}
