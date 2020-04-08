using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthScanCamera : MonoBehaviour
{
    [SerializeField, Header("ステージスキャン用のマテリアル")]
    Material DepthScanMat = null;
    [SerializeField, Header("波紋計算のマテリアル")]
    Material WaveComputeMat = null;
    private int TextureSize = 256;

    // Start is called before the first frame update
    void Awake()
    {
        Scan();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Scan")]
    void Scan()
    {
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;

        RenderTexture rTex = new RenderTexture(TextureSize, TextureSize, 24, RenderTextureFormat.Depth);

        Texture2D texBuf = new Texture2D(1, 1);
        texBuf.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1));
        texBuf.Apply();

        Graphics.Blit(texBuf, rTex, DepthScanMat);

        WaveComputeMat.SetTexture("_MaskTex", rTex);
    }
}
