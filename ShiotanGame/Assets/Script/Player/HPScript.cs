using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
public class HPScript : MonoBehaviour
{
    [SerializeField,Header("何秒でダメージはいるか")]
    private float DamageTime;
    
    private HumanoidBase HPcnt;
    private ProtoMove2 MoveStop;        //死んだとき移動処理　ストップ
    private RespawnScript resscript;    //リスポーン用
    private Rigidbody rb;               //rigidbody読み込み
    
    private WaveAct Wave;

    [SerializeField, Header("HPゲージのスクリプト")]
    Gage GageScript;


    [Header("沈む深さ"), SerializeField] private float Depth;
    [Header("設定した時間経過でリスタート"), SerializeField] private float RestartTime;
    private float R_time;


    [SerializeField, Header("死亡時エフェクト")]
    private GameObject DeathEffect = null;
    private ParticleEffectScript m_ParEffScp = null;

    private bool ReloadFlg = false;//Reload処理は一回のみにするため


    [SerializeField, Header("体力が０になってからフェードアウト開始までの時間")]
    private float WaitTimeforFade = 0.5f;


    void Start()
    {
        // Rigidbodyコンポーネントを取得する
        rb = GetComponent<Rigidbody>();
        Wave = this.GetComponent<WaveAct>();

        //リスポーン　読み込み
        resscript = GameObject.Find("Respawn").GetComponent<RespawnScript>();
        MoveStop = GetComponent<ProtoMove2>();
        HPcnt = GetComponent<HumanoidBase>();

        //リスタート時間計る用
        R_time = 0;

        m_ParEffScp = DeathEffect.GetComponent<ParticleEffectScript>();


        //体力０を感知して一回だけ行う処理設定
        this.UpdateAsObservable()
            .First(x => HPcnt.DeadCheck())
            .Subscribe(_ => {
                DeathFunctionOnce();//死亡時処理
            }).AddTo(this);
    }

    void Update()
    {
        // 座標を取得
        Vector3 pos = transform.position;
        if (HPcnt.DeadCheck())
        {
           // this.GetComponentInChildren<BoxCollider>().enabled = false; //波紋発生に必要なコライダーまでfalseにされてたのでコメントアウトしました 沈むうえでのバグは発生してないです

            MoveStop.Stop();

            //指定した方向にゆっくり回転する場合
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-70f, 0, 0), Time.deltaTime/2);

            //沈む処理
            if(pos.y> Depth)
            {
                pos.y -= 1 * Time.deltaTime;
                R_time += Time.deltaTime;
                transform.position = pos;
            }

            //リスタート処理
            //if(R_time>RestartTime)
            //{ 
                
            //}
        }
    }

    private void OnDestroy()
    {
        resscript.Respawn = true;
    }
    
    public float GetNowHP()
    {
        return HPcnt.NowHP;
    }

    //体力が0になったときに一回だけ行う処理
    void DeathFunctionOnce()
    {
        if (m_ParEffScp != null)
        {
            Vector3 pos = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);
            Instantiate(m_ParEffScp, pos, Quaternion.identity);
            rb.useGravity = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<ProtoMove2>().enabled = false;

            AudioManager.Instance.PlaySE("SE_GAMEOVER");
        }
        Wave.AwakeMultiWave();
        Wave.StopWaveAct();

        //フェードアウト開始時間設定
        Observable.Timer(System.TimeSpan.FromSeconds(WaitTimeforFade))
            .Subscribe(_ =>

                GameManager.Instance.SceneReload(true)
            );
    }


    #region デバッグ用
    
    //死亡させる
    [ContextMenu("死")]
    private void Death_Debug()
    {
        HPcnt.NowHP = 0;
    }
    #endregion 
}
