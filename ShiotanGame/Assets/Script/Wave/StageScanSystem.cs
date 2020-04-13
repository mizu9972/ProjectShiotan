using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

using UniRx;

public class StageScanSystem : MonoBehaviour
{

    [SerializeField, Header("テクスチャ書き出し場所")]
    private string PathName = "";
    [SerializeField, Header("出力テクスチャ名")]
    private string TexName = "";
    [SerializeField, Header("スキャンカメラ")]
    private GameObject ScanCamera = null;



    private float m_outputTime = 2.0f;//待機時間

    // Start is called before the first frame update
    void Start()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(m_outputTime)).Subscribe(_ => OutputTexture());
    }

    void OutputTexture()
    {
        if(ScanCamera == null)
        {
            return;
        }
        if(PathName.Length <= 0)
        {
            return;
        }

        PathName += "/" + TexName + ".png";

        RenderTexture IniTex = RenderTexture.active;//元のレンダーテクスチャを保持
        RenderTexture rTex = ScanCamera.GetComponent<DispDepth>().getDepthRenderTexture();//深度テクスチャを取得

        //レンダーテクスチャをTexture2Dに変換
        RenderTexture.active = rTex;

        Texture2D tex = new Texture2D(rTex.width, rTex.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        RenderTexture.active = IniTex;

        //書き出し
        var png = tex.EncodeToPNG();
        File.WriteAllBytes(PathName, png);

        //実行終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif


    }
}
