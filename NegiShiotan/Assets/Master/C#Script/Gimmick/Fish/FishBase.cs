using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fish;

namespace Fish {
    enum FishType {
        Piranha,
        Arapaima,
        SparkEel,
    }
}

public class FishBase : MonoBehaviour {
    [SerializeField] private FishType m_FishType;

    private Transform Player;
    private Rigidbody rb;

    [Header("SE:水の中移動中")]
    public SEPlayer SE;

    [SerializeField] private bool m_TrackingFlg = false;
    [SerializeField] private bool m_SpawnFlg = false;
    [SerializeField] private float m_TrackingSpeed = 0.0f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = gameObject.GetComponent<Rigidbody>();
        SE.PlaySound();
    }

    private void FixedUpdate() {
        if (m_TrackingFlg) {
            Tracking();
        }
    }

    // 魚の追尾関数
    private void Tracking() {
        // プレイヤーの方向を向く
        gameObject.transform.LookAt(Player);
        gameObject.transform.localEulerAngles = new Vector3(0.0f, gameObject.transform.localEulerAngles.y, 0.0f);

        // 移動
        rb.AddForce(gameObject.transform.forward * m_TrackingSpeed, ForceMode.VelocityChange);
    }
    #region Getter//Setter
    public void SetSpawnFlg(bool val) {
        m_SpawnFlg = val;
        m_TrackingFlg = !val;   // スポーンフラグがtrueになったら追尾フラグを強制オフにする（スポーンフラグがfalseになったら再度追尾する）
    }

    public bool GetSpawnFlg() {
        return m_SpawnFlg;
    }

    public void SetTrackingFlg(bool val) {
        m_TrackingFlg = val;
    }

    public bool GetTrackingFlg() {
        return m_TrackingFlg;
    }
    #endregion
}