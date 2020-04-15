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
    [SerializeField, Header("出力テクスチャ名(壁)")]
    private string WallTexName = "";
    [SerializeField, Header("出力テクスチャ名(地面)")]
    private string FloorTexName = "";

    //ゲームオブジェクト
    [SerializeField, Header("壁スキャンカメラ")]
    private GameObject WallScanCamera = null;
    [SerializeField, Header("地面スキャンカメラ")]
    private GameObject FloorScanCamera = null;


    private float m_outputTime = 0.0f;//待機時間

    // Start is called before the first frame update
    void Start()
    {
        //メイン処理
        Observable.Timer(System.TimeSpan.FromSeconds(m_outputTime))
            .Subscribe(_ => OutputTexture());
    }

    void OutputTexture()
    {
        if (PathName.Length <= 0)
        {
            return;
        }

        StartCoroutine("OutWallTexture");//壁スキャン処理

    }

    //↓  //実行順

    IEnumerator OutWallTexture()
    {
        if (WallScanCamera == null)
        {
            yield break;//return
        }
        WallScanCamera.SetActive(true);

        yield return new WaitForSeconds(1);//指定秒停止

        string OutPath = PathName + "/" + WallTexName + ".png";

        RenderTexture IniTex = RenderTexture.active;//元のレンダーテクスチャを保持
        RenderTexture rTex = WallScanCamera.GetComponent<DispDepth>().getDepthRenderTexture();//深度テクスチャを取得

        //描画書き込み対象に設定
        RenderTexture.active = rTex;



        //レンダーテクスチャをTexture2Dに変換
        Texture2D tex = new Texture2D(rTex.width, rTex.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        RenderTexture.active = IniTex;

        //書き出し
        var png = tex.EncodeToPNG();
        File.WriteAllBytes(OutPath, png);

        WallScanCamera.SetActive(false);

        StartCoroutine("OutFloorTexture");//地面スキャン処理
    }

    //↓  //実行順

    IEnumerator OutFloorTexture()
    {
        if(FloorScanCamera == null)
        {
            yield break;//return
        }

        FloorScanCamera.SetActive(true);

        yield return new WaitForSeconds(1);//指定秒停止

        string OutPath = PathName + "/" + FloorTexName + ".png";

        RenderTexture IniTex = RenderTexture.active;//元のレンダーテクスチャを保持
        RenderTexture rTex = FloorScanCamera.GetComponent<DispDepth>().getDepthRenderTexture();//深度テクスチャを取得

        //描画書き込み対象に設定
        RenderTexture.active = rTex;


        //レンダーテクスチャをTexture2Dに変換
        Texture2D tex = new Texture2D(rTex.width, rTex.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        RenderTexture.active = IniTex;

        //書き出し
        var png = tex.EncodeToPNG();
        File.WriteAllBytes(OutPath, png);

        FloorScanCamera.SetActive(false);

        //実行終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
