using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePlane : MonoBehaviour
{
    [SerializeField, Header("波紋計算用マテリアル")]
    private Material waveMat;
    [SerializeField, Header("画像合成用マテリアル")]
    private Material matPaint;
    [SerializeField, Header("レンダーテクスチャ初期化用マテリアル")]
    private Material mat_gray_tex;
    [SerializeField, Header("外力テクスチャ")]
    private Texture texBlush;
    public int TextureSize = 256;

    private Material mat;
    private RenderTexture rTex;
    // Start is called before the first frame update
    void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        rTex = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RGFloat, RenderTextureReadWrite.Default);

        Texture2D texBuf = new Texture2D(1, 1);
        texBuf.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1));
        texBuf.Apply();
        Graphics.Blit(texBuf, rTex, mat_gray_tex);

        //シェーダのテクスチャを設定
        waveMat.SetTexture("_MainTex", rTex);
        waveMat.SetTexture("_MaskTex", null);
        matPaint.SetTexture("_PaintTex", texBlush);
        mat.SetTexture("_HeightMap", rTex);

        //シェーダのパラメータ設定
        matPaint.SetFloat("_Size", 0.1f);
        waveMat.SetFloat("_PhaseVelocity", 0.2f);
        waveMat.SetFloat("_Attenuation", 0.999f);
        mat.SetFloat("_BumpScale", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);

        Graphics.Blit(rTex, buf, waveMat);
        Graphics.Blit(buf, rTex);
        RenderTexture.ReleaseTemporary(buf);
    }
}
