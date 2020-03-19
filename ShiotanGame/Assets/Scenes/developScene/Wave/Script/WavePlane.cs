﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;
public class WavePlane : MonoBehaviour
{
    public Material waveMat, matPaint;
    public Material mat_gray_tex;    //2019/11/09追加
    public Texture texBlush;
    public int TextureSize = 256;
    private Material mat;
    private RenderTexture rTex;

    // Use this for initialization
    void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        rTex = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RGFloat, RenderTextureReadWrite.Default);

        //2019/11/07修正後
        Texture2D texBuf = new Texture2D(1, 1);
        texBuf.SetPixel(0, 0, new Color(0.0f, 1.0f, 1.0f, 1));
        texBuf.Apply();
        Graphics.Blit(texBuf, rTex, mat_gray_tex);

        waveMat.SetTexture("_MainTex", rTex);
        waveMat.SetTexture("_MaskTex", null);
        matPaint.SetTexture("_AddTex", texBlush);
        mat.SetTexture("_HeightMap", rTex);

        matPaint.SetFloat("_Size", Mathf.Max(0.1f, 0.01f));
        waveMat.SetFloat("_PhaseVelocity", 0.2f);
        waveMat.SetFloat("_Attenuation", 0.999f);
        mat.SetFloat("_BumpScale", 0.1f);
    }

    //UV座標検出
    private Vector2 UVDetector(RaycastHit hitInfo)
    {
        bool pass = false;
        Mesh mesh = hitInfo.collider.gameObject.GetComponent<MeshFilter>().sharedMesh;
        int[] index = mesh.triangles;
        Vector3[] vert = mesh.vertices;

        //座標計算
        Vector3 pos;
        pos.x = hitInfo.collider.gameObject.transform.position.x / 10.0f / gameObject.transform.lossyScale.x;
        pos.y = 0.5f;
        pos.z = hitInfo.collider.gameObject.transform.position.z / 10.0f / gameObject.transform.lossyScale.z;

        for (int i = 0; i < index.Length; i += 3)
        {
            Vector3 p1 = vert[index[i]];
            Vector3 p2 = vert[index[i + 1]];
            Vector3 p3 = vert[index[i + 2]];

            //メッシュの同一平面上に存在するか
            float val = Vector3.Dot(Vector3.Cross(p2 - p1, p3 - p1), pos - p1);
            pass = (val > -0.000001f && val < 0.000001f);

            //メッシュ内に存在するか
            if (pass)
            {
                Vector3 pcp1 = Vector3.Cross(pos - p1, p2 - p1).normalized;
                Vector3 pcp2 = Vector3.Cross(pos - p2, p3 - p2).normalized;
                Vector3 pcp3 = Vector3.Cross(pos - p3, p1 - p3).normalized;

                float d12 = Vector3.Dot(pcp1, pcp2);
                float d23 = Vector3.Dot(pcp2, pcp3);

                pass = (d12 > 0.999f && d23 > 0.999f);

                //返上か否か
                if (!pass)
                {
                    if (0.999f <= Vector3.Dot((p1 - pos).normalized, (pos - p2).normalized))
                    {
                        return OnLineUV(mesh, pos, p1, p2, i, i + 1);
                    }
                    else if (0.999f <= Vector3.Dot((p2 - pos).normalized, (pos - p3).normalized))
                    {
                        return OnLineUV(mesh, pos, p2, p3, i + 1, i + 2);
                    }
                    else if (0.999f <= Vector3.Dot((p3 - pos).normalized, (pos - p1).normalized))
                    {
                        return OnLineUV(mesh, pos, p3, p1, i + 2, i);
                    }
                }
            }

            //uv座標の算出
            if (pass)
            {
                Vector3 uv1 = mesh.uv[mesh.triangles[i]];
                Vector3 uv2 = mesh.uv[mesh.triangles[i + 1]];
                Vector3 uv3 = mesh.uv[mesh.triangles[i + 2]];

                Vector3 f1 = p1 - pos;
                Vector3 f2 = p2 - pos;
                Vector3 f3 = p3 - pos;

                float a = Vector3.Cross(p1 - p2, p1 - p3).magnitude;
                float a1 = Vector3.Cross(f2, f3).magnitude / a;
                float a2 = Vector3.Cross(f3, f1).magnitude / a;
                float a3 = Vector3.Cross(f1, f2).magnitude / a;

                Vector2 uv = uv1 * a1 + uv2 * a2 + uv3 * a3;

                return uv;
            }
        }
        return new Vector2(-1, -1);
    }

    Vector2 OnLineUV(Mesh mesh, Vector3 v, Vector3 v1, Vector3 v2, int n1, int n2)
    {
        Vector3 uv1 = mesh.uv[mesh.triangles[n1]];
        Vector3 uv2 = mesh.uv[mesh.triangles[n2]];
        float m = (v - v1).magnitude;
        float n = (v2 - v).magnitude;
        Vector3 uv = (n * uv1 + m * uv2) / (m + n);
        return uv;
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);

        Graphics.Blit(rTex, buf, waveMat);
        Graphics.Blit(buf, rTex);

        Material myMat = gameObject.GetComponent<Renderer>().material;
        myMat.SetTexture("_MainTex", rTex);

        RenderTexture.ReleaseTemporary(buf);
    }

    public void OnTriggerEnter(Collider other)
    {
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);

        //レイを作成
        Ray ray = new Ray(new Vector3(other.transform.position.x, other.transform.position.y + Vector3.up.y * 1, other.transform.position.z), Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);//デバッグ用　レイを可視化

        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo, 2))
        {
            //水面上ならシェーダーんいUV座標を計算して渡す
            Vector2 UVPos = UVDetector(hitInfo);
            matPaint.SetTexture("_MainTex", rTex);
            matPaint.SetVector("_UVPosition", new Vector4(UVPos.x, UVPos.y, 0, 0));
            Graphics.Blit(rTex, buf, matPaint);
            Graphics.Blit(buf, rTex);
        }

        RenderTexture.ReleaseTemporary(buf);
    }
}