using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectsControl : MonoBehaviour
{
    public enum DrawQuality {
        all,
        low,
        medium,
        high,
        veryhigh,
        none,
    }

    private VisualEffect ve;
    [SerializeField] private DrawQuality Quality = DrawQuality.none;

    [SerializeField] private bool OnPlay = false;
    [SerializeField] private bool DontDestroy = false;
    private bool mStartVFX;
    private bool mEndVFX;
    private string mStartTiming;
    private string mEndTiming;

    // Start is called before the first frame update
    void Start()
    {
        //if (/*クオリティ設定変数*/ >= (int)Quality) {
            ve = gameObject.GetComponent<VisualEffect>();

            // 開始時にスタート
            if (OnPlay) {
                StartVFX();
            }
        //}
    }

    /// <summary>
    /// VFX開始処理
    /// </summary>
    /// <returns>実行できたらtrueを返す(受け取らなくてもよい)</returns>
    public bool StartVFX() {
        //if (/*クオリティ設定変数*/ >= (int)Quality) {
        // 一度終了されていたら開始できない様にロックする
            if (!mEndVFX) {
                mStartVFX = true;
                ve.SendEvent("Play");
                mStartTiming += System.DateTime.Now.Hour.ToString() + ':' + System.DateTime.Now.Minute.ToString() + ':' + System.DateTime.Now.Second.ToString();
                return true;
            }
        //}
        return false;
    }

    /// <summary>
    /// VFX停止処理
    /// </summary>
    /// <returns>停止できたらtrueを返す(受け取らなくてもよい)</returns>
    public bool EndVFX() {
        // 開始されていない場合終了できない様にする
        if (mStartVFX) {
            mEndVFX = true;
            ve.SendEvent("Stop");
            mEndTiming += System.DateTime.Now.Hour.ToString() + ':' + System.DateTime.Now.Minute.ToString() + ':' + System.DateTime.Now.Second.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// オブジェクトの破棄
    /// </summary>
    public void DestroyObj() {
        if (!DontDestroy) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 開始タイミングの確認（ログに表示）
    /// </summary>
    /// <returns>開始ならtrueを返す(受け取らなくてもよい)</returns>
    public bool ShowStartVFXTiming() {
        if (mStartTiming.Length != 0) {
            // 開始タイミングの確認
            string IsEnd = "現在稼働中";
            if (mEndTiming.Length != 0) {
                IsEnd = "終了済み";
            }

            Debug.Log(gameObject.name + "(VFX)は " + mEndTiming + "に開始されました" + '(' + IsEnd + ')');
            return true;
        }
        // 未開始
        else {
            Debug.Log(gameObject.name + "(VFX)は開始されていません");
            return false;
        }
    }


    /// <summary>
    /// 停止タイミングの確認（ログに表示）
    /// </summary>
    /// <returns>停止ならtrueを返す(受け取らなくてもよい)</returns>
    public bool ShowEndVFXTiming() {
        if (mEndTiming.Length != 0) {
            // 停止タイミングの確認
            Debug.Log(gameObject.name + "(VFX)は " + mEndTiming + "に停止されました");
            return true;
        }
        // 現在動作中
        else {
            Debug.Log(gameObject.name + "(VFX)は現在作動中です");
            return false;
        }
    }
}