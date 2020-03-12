using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private bool flag = false;
    // Update is called once per frame
    void Update()
    {
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);
        ///
        if (flag)
        {
            Vector2 UVPos = UVDetector(0, 0, 0/*other.transform.position.x, other.transform.position.y, other.transform.position.z*/);
            matPaint.SetTexture("_MainTex", rTex);
            matPaint.SetVector("_UVPosition", new Vector4(UVPos.x, UVPos.y, 0, 0));
            Graphics.Blit(rTex, buf, matPaint);
            Graphics.Blit(buf, rTex);

            flag = false;
        }
        ///
        Graphics.Blit(rTex, buf, waveMat);
        Graphics.Blit(buf, rTex);
        RenderTexture.ReleaseTemporary(buf);
    }

    //オブジェクトの当たり判定
    public void OnTriggerEnter(Collider other)
    {
        //RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);
        //Vector2 UVPos = UVDetector(other.transform.position.x, other.transform.position.y, other.transform.position.z);
        //matPaint.SetTexture("_MainTex", rTex);
        //matPaint.SetVector("_UVPosition", new Vector4(UVPos.x, UVPos.y, 0, 0));
        //Graphics.Blit(rTex, buf, matPaint);
        //Graphics.Blit(buf, rTex);
        flag = true;
    }

    //オリジナルメソッド

    //座標から水面上のUV座標を計算
    private Vector2 UVDetector(float x,float y, float z)
    {
        //返り値
        Vector2 retUVVector = new Vector2(-1, -1);

        //与えられた数値を比較して-1~1の範囲に収める
        Func<float, float, float, float> ComputeUVPos = (float Value, float myPos, float myScale) =>
           {
               if (judgeInRange(Value, myPos - myScale / 2.0f, myPos + myScale / 2.0f))
               {
                   //計算
                   float tempOtherPos = Value + myScale;//0以上の数値にする
                   tempOtherPos = tempOtherPos / (myPos + myScale * 2.0f) * 2.0f - 1.0f;//-1~1の範囲に収める

                   Debug.Log("UVpos = " + tempOtherPos);
                   return tempOtherPos;
               }
               else
               {
                   //範囲外
                   Debug.Log("OutofRange");
                   return -1;
               }
           };

        //retUVVector.x = ComputeUVPos(x, this.transform.position.x, this.transform.lossyScale.x);
        //retUVVector.y = ComputeUVPos(z, this.transform.position.z, this.transform.lossyScale.z);

        //return retUVVector;
        return new Vector2(0, 0);//デバッグ用に固定
    }

    //数値が与えられた範囲内か判定
    public bool judgeInRange(float Value,float minValue,float maxValue)
    {
        return (Value == Mathf.Min(maxValue, Mathf.Max(minValue, Value)));
    }
}
