using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthScanCamera : MonoBehaviour
{
    [SerializeField, Header("ステージスキャン用のマテリアル")]
    Material DepthScanMat = null;
    [SerializeField, Header("波紋計算のマテリアル")]
    Material WaveComputeMat = null;
    [SerializeField, Header("デバッグ表示用マテリアル")]
    Material DepthViewMat_DEBUG = null;

    private Camera m_Camera = null;
    private RenderTexture m_ColorTex;
    private RenderTexture m_DepthTex;
    private int TextureSize = 256;

    // Start is called before the first frame update
    void Start()
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
        m_Camera = this.GetComponent<Camera>();
        m_Camera.depthTextureMode = DepthTextureMode.Depth;

        //カラーバッファ作成
        m_ColorTex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        m_ColorTex.Create();

        //デプスバッファ作成
        m_DepthTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
        m_DepthTex.Create();

        //カメラにセット
        m_Camera.SetTargetBuffers(m_ColorTex.colorBuffer, m_DepthTex.depthBuffer);

        Graphics.SetRenderTarget(null);
        Graphics.Blit(m_DepthTex, DepthViewMat_DEBUG);
        //RenderTexture rTex = new RenderTexture(TextureSize, TextureSize, 24, RenderTextureFormat.Depth);

        //Texture2D texBuf = new Texture2D(1, 1);
        //texBuf.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1));
        //texBuf.Apply();

        //Graphics.Blit(texBuf, rTex, DepthScanMat);

        //WaveComputeMat.SetTexture("_MaskTex", rTex);

        //if(DepthViewMat_DEBUG != null)
        //{
        //    DepthViewMat_DEBUG.SetTexture("_MainTex", rTex);
        //}
    }

    private void OnPostRender()
    {

        Graphics.SetRenderTarget(null);
        Graphics.Blit(m_DepthTex, DepthViewMat_DEBUG);
    }
}
