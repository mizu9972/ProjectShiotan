using System.Collections;
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
    public Texture tex_circle, tex_star;
    public Slider slider_blushsize, slider_phasevelocity, slider_attenuation, slider_bump_scale;
    public Toggle toggle_none, toggle_circle, toggle_star;
    public Button button_init;
    private Material mat;
    private RenderTexture rTex;
    private Camera mainCam;
    private float rayDist = 100f;

    // Use this for initialization
    void Start()
    {
        mainCam = Camera.main;
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

        if (slider_blushsize != null)
        {
            slider_blushsize.value = 0.2f;
            slider_phasevelocity.value = 0.4f;
            slider_attenuation.value = 0.99f;
            matPaint.SetFloat("_Size", Mathf.Max(slider_blushsize.value * 0.5f, 0.01f));
            waveMat.SetFloat("_PhaseVelocity", slider_phasevelocity.value * 0.5f);
            waveMat.SetFloat("_Attenuation", slider_attenuation.value * 0.1f + 0.9f);
        }

        if (toggle_none != null)
        {
            toggle_none.onValueChanged.AddListener((flg) =>
            {
                waveMat.SetTexture("_MaskTex", null);
            });
            toggle_circle.onValueChanged.AddListener((flg) =>
            {
                waveMat.SetTexture("_MaskTex", tex_circle);
            });
            toggle_star.onValueChanged.AddListener((flg) =>
            {
                waveMat.SetTexture("_MaskTex", tex_star);
            });
        }

        if (button_init != null)
        {
            button_init.onClick.AddListener(() =>
            {
                Texture2D buf = new Texture2D(1, 1);
                texBuf.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1));
                texBuf.Apply();
                Graphics.Blit(texBuf, rTex, mat_gray_tex);
            });
        }

        if (slider_bump_scale != null)
        {
            slider_bump_scale.value = 1.0f;
            slider_bump_scale.minValue = 0.0f;
            slider_bump_scale.maxValue = 20.0f;
            mat.SetFloat("_BumpScale", slider_bump_scale.value);
        }
    }

    //UV座標検出
    private Vector2 UVDetector(RaycastHit hitInfo)
    {
        bool pass = false;
        Mesh mesh = hitInfo.collider.gameObject.GetComponent<MeshFilter>().sharedMesh;
        int[] index = mesh.triangles;
        Vector3[] vert = mesh.vertices;
        Vector3 pos = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        for (int i = 0; i < index.Length; i += 3)
        {
            Vector3 p1 = vert[index[i]];
            Vector3 p2 = vert[index[i + 1]];
            Vector3 p3 = vert[index[i + 2]];

            //クリックしたポイントがメッシュの同一平面上に存在するか
            float val = Vector3.Dot(Vector3.Cross(p2 - p1, p3 - p1), pos - p1);
            pass = (val > -0.000001f && val < 0.000001f);

            //クリックしたポイントがメッシュ内に存在するか
            if (pass)
            {
                Vector3 pcp1 = Vector3.Cross(pos - p1, p2 - p1).normalized;
                Vector3 pcp2 = Vector3.Cross(pos - p2, p3 - p2).normalized;
                Vector3 pcp3 = Vector3.Cross(pos - p3, p1 - p3).normalized;

                float d12 = Vector3.Dot(pcp1, pcp2);
                float d23 = Vector3.Dot(pcp2, pcp3);

                pass = (d12 > 0.999f && d23 > 0.999f);

                //クリックしたポイントが返上か否か
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
        RaycastHit hitInfo = new RaycastHit();
        //2019/11/09修正前
        //RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height);
        //2019/11/07修正後
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.RGFloat);

        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, rayDist))
            {
                Vector2 UVPos = UVDetector(hitInfo);
                matPaint.SetTexture("_MainTex", rTex);
                matPaint.SetVector("_UVPosition", new Vector4(UVPos.x, UVPos.y, 0, 0));
                Graphics.Blit(rTex, buf, matPaint);
                Graphics.Blit(buf, rTex);
            }
            if (slider_blushsize != null)
            {
                matPaint.SetFloat("_Size", Mathf.Max(slider_blushsize.value * 0.5f, 0.01f));
                waveMat.SetFloat("_PhaseVelocity", slider_phasevelocity.value * 0.5f);
                waveMat.SetFloat("_Attenuation", slider_attenuation.value * 0.1f + 0.9f);
            }

            if (slider_bump_scale != null)
            {
                mat.SetFloat("_BumpScale", slider_bump_scale.value);
            }
        }
        Graphics.Blit(rTex, buf, waveMat);
        Graphics.Blit(buf, rTex);

        Material myMat = gameObject.GetComponent<Renderer>().material;
        myMat.SetTexture("_MainTex", rTex);

        RenderTexture.ReleaseTemporary(buf);
    }
}
