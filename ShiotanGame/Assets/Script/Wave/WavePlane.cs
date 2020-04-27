using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;

using UniRx;

public class WavePlane : MonoBehaviour
{
    public Material waveMat, matPaint;
    public Material InitRanderMat;
    public Texture texBlush;
    private Texture texMask = null;
    public int TextureSize = 256;
    public float Size = 1.0f;

    [SerializeField, Header("波の速度")]
    private float PhaseVelocity = 0.2f;//波の速度

    [SerializeField, Header("波の減衰率")]
    private float Attenuation = 0.999f;//減衰率
    private Material mat;
    private RenderTexture rTex;
    private Material myMat;

    [SerializeField, Header("潮テクスチャ")]
    private Texture TideTexture = null;
    [SerializeField, Header("潮テクスチャの明るさ"), Range(0.0f, 1.0f)]
    private float TideLitRate = 1.0f;

    [SerializeField, Header("壁"), Header("マスクテクスチャ")]
    private Texture WallMaskTex = null;
    [SerializeField, Header("地面")]
    private Texture FloorMaskTex = null;

    private Transform myTrans;//自身のtransform
    private Transform ParentTrans;//親オブジェクトのtransform
    private float ScaleX, ScaleZ;//水面オブジェクトのスケール
    // Use this for initialization
    void Awake()
    {
        //取得
        myTrans = this.GetComponent<Transform>();
        ParentTrans = this.gameObject.GetComponentInParent<Transform>();
        ScaleX = myTrans.lossyScale.x;// * ParentTrans.lossyScale.x;
        ScaleZ = myTrans.lossyScale.z;// * ParentTrans.lossyScale.z;


        Debug.Log("Scale = " + ScaleX + ScaleZ);
        
        mat = gameObject.GetComponent<Renderer>().material;
        //myMat = gameObject.GetComponent<Renderer>().material;

        rTex = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);

        Texture2D texBuf = new Texture2D(1, 1);
        texBuf.SetPixel(0, 0, new Color(0.0f, 1.0f, 1.0f, 1));
        texBuf.Apply();
        Graphics.Blit(texBuf, rTex, InitRanderMat);

        //シェーダー初期化-------------------------------------
        {

            //テクスチャ設定
            {
                waveMat.SetTexture("_MainTex", rTex);
                waveMat.SetTexture("_MaskTex", WallMaskTex);

                matPaint.SetTexture("_AddTex", texBlush);
                mat.SetTexture("_HeightMap", rTex);
                mat.SetTexture("_FloorTex", FloorMaskTex);

                mat.SetTexture("_TideTex", TideTexture);
            }

            //パラメータ初期化
            mat.SetFloat("_TideLitRate", TideLitRate);

            matPaint.SetFloat("_SizeX",0.10f/* Size / ScaleX / 4.0f*/);
            matPaint.SetFloat("_SizeY",0.10f/* Size / ScaleZ / 4.0f*/);

            waveMat.SetFloat("_PhaseVelocity", PhaseVelocity / ScaleX);
            waveMat.SetFloat("_Attenuation", Attenuation);
            mat.SetFloat("_BumpScale", 0.1f);
        }
        //--------------------------------------------------
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
            //if (pass)
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
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.ARGBFloat);

        Graphics.Blit(rTex, buf, waveMat);
        Graphics.Blit(buf, rTex);


        mat.SetTexture("_MainTex", rTex);

        RenderTexture.ReleaseTemporary(buf);
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    AwakeWave(other.transform, 0.1f, texBlush);
    //}

    private class AwakeWaveArg{
        public float PaintSize;
        public Vector2 UVPos;
        public Texture Tex;
        public RenderTexture buf;
        public bool RandomFlag;
        public float RandomRange;

        public AwakeWaveArg(float PaintSize_, Vector2 UVPos_, Texture Tex_, RenderTexture buf_,bool RandomFlag_, float RandomRange_)
        {
            PaintSize = PaintSize_;
            UVPos = UVPos_;
            Tex = Tex_;
            buf = buf_;
            RandomFlag = RandomFlag_;
            RandomRange = RandomRange_;
        }
    }
    //波を発生させる
    public void AwakeWave(Transform ObjectTrans, float PaintSize, Texture Tex, bool RandomFlag = false, float RandomRange = 1.0f, float LoopSecond = 0.0f,float AwakeIntervalSecond = 1.0f)
    {
        RenderTexture buf = RenderTexture.GetTemporary(rTex.width, rTex.height, 0, RenderTextureFormat.ARGBFloat);

        //レイを作成
        Ray ray = new Ray(new Vector3(ObjectTrans.position.x, ObjectTrans.position.y + Vector3.up.y * 1, ObjectTrans.position.z), Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);//デバッグ用　レイを可視化

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, 2))
        {

            //水面上ならシェーダーにUV座標を計算して渡す
            Vector2 UVPos = UVDetector(hitInfo);

            //波発生
            StartCoroutine("LoopWaveFunction", new AwakeWaveArg(PaintSize, UVPos, Tex, buf, RandomFlag, RandomRange));

            //波を連続して発生させる処理
            var StopWaveLoopSelect = Observable.Timer(System.TimeSpan.FromSeconds(LoopSecond));
            var LoopWaveFunction = Observable.Interval(System.TimeSpan.FromSeconds(AwakeIntervalSecond))
                .TakeUntil(StopWaveLoopSelect)
                .Subscribe(_ => StartCoroutine("LoopWaveFunction", new AwakeWaveArg(PaintSize, UVPos, Tex, buf, RandomFlag, RandomRange)));


        }


        RenderTexture.ReleaseTemporary(buf);
    }

    IEnumerator LoopWaveFunction(AwakeWaveArg Arg)
    {
        matPaint.SetTexture("_MainTex", rTex);
        if (Arg.RandomFlag == true)
        {
            matPaint.SetVector("_UVPosition", new Vector4(Arg.UVPos.x + RandomReturn(Arg.RandomRange), Arg.UVPos.y + RandomReturn(Arg.RandomRange), 0, 0));
        }
        else
        {
            matPaint.SetVector("_UVPosition", new Vector4(Arg.UVPos.x, Arg.UVPos.y, 0, 0));
        }
        matPaint.SetFloat("_SizeX", Arg.PaintSize / (ScaleX / 4.0f));
        matPaint.SetFloat("_SizeY", Arg.PaintSize / (ScaleZ / 4.0f));

        matPaint.SetTexture("_AddTex", Arg.Tex);

        Graphics.Blit(rTex, Arg.buf, matPaint);
        Graphics.Blit(Arg.buf, rTex);

        yield break;
    }
    
    //-Ramdom/2 ~ Random/2 の範囲でランダムな値を返す
    float RandomReturn(float RandomRange)
    {
        return (UnityEngine.Random.value * RandomRange) - RandomRange / 2.0f;
    }
}
