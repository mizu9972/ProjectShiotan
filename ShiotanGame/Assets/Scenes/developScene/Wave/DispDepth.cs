using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DispDepth : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, mat);
    }

    public RenderTexture getDepthRenderTexture()
    {
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        RenderTexture retTex = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGB32);
        retTex.Create();
        Graphics.Blit(null, retTex, mat);
        return retTex;
    }
}