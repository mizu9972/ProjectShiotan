using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBase : MonoBehaviour {
    public GameObject Piranha;      // ピラニアオブジェクトを入れるもの（なくてもできるけど一応配置してる）
    public int PiranihaCount = 0;   // プランナーがレベルデザインやりやすい用
    public List<GameObject> ChildPiranha;   // スクリプトで管理しないと、子の検索をかけた時に全ての子を返してくれないことがあったのでこういう設計にしています。

    [SerializeField] private Vector3 InstantPositionCorrct;     // ピラニア生成の座標の誤差をどこまで設定しますか?

    private Vector3 InitPos;        // 初期位置。住処の場所って認識でいいと思う
    [SerializeField] private float MoveSpeed = 10.0f;
    public int AttackPower = 0;

    [SerializeField] private int PiranhaChaceDirayFrame = 5;
    [SerializeField] private List<Vector3> TargetPosList;

    public GameObject TargetObject; 
    public bool IsChase = false;

    private void Awake()
    {
        // ピラニアカウント数分ピラニアを生成する
        for (int i = 0; i < PiranihaCount; i++) {
            Vector3 CreatePos = new Vector3(Random.Range(-InstantPositionCorrct.x, InstantPositionCorrct.x), Random.Range(-InstantPositionCorrct.y, InstantPositionCorrct.y), Random.Range(-InstantPositionCorrct.z, InstantPositionCorrct.z));
            GameObject newObj = Instantiate(Piranha, gameObject.transform.position + CreatePos, Quaternion.identity, gameObject.transform);
            newObj.transform.localScale = new Vector3(1 / gameObject.transform.localScale.x, 1 / gameObject.transform.localScale.y, 1 / gameObject.transform.localScale.z);
            // ToDo::ピラニアの動き方等もここで設定したい
            ChildPiranha.Add(newObj);
        }
    }

    void Start() 
    {
        InitPos = gameObject.transform.position;
    }

    void Update() 
    {
        // ToDo::群衆の処理を行う

        // ターゲットが存在するときにターゲットの座標を取得しておく
        if (IsChase) {
            // ディレイフレームを超え始めるとターゲットを追尾し、最初の座標を削除していく
            if (TargetPosList.Count > PiranhaChaceDirayFrame) {
                TargetPosList.RemoveAt(0);
                ChaseTarget();

                // もし、ついていたら追いかけずに攻撃する
                if (IsChase) {
                }
                else {
                    AttackTarget();
                }
            }
            TargetPosList.Add(TargetObject.transform.position);
        }
        // ターゲットを追っていないときの処理
        else {
            // ToDo::住処に戻る処理
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 0.0f;  // 動きを止める処理
        }


        // 各ピラニアの動き
        if (ChildPiranha.Count > 0) {
            // 子のすべての動きをここで再現させる
            foreach (GameObject obj in ChildPiranha) {
                // ToDo::追いかけ状態かそうでないかで動きが変わるかも？
                if (IsChase) {

                }
                else {

                }
            }
        }
        else {
            Debug.LogWarning(gameObject.transform.name + "にはピラニアが1匹もいません。");
        }
    }

    /// <summary>
    /// ターゲット追尾
    /// </summary>
    private void ChaseTarget() 
    {
        gameObject.transform.LookAt(TargetPosList[0]);  // ターゲットの方向を向く
        Vector3 newAngle = gameObject.transform.localEulerAngles;
        newAngle.x = newAngle.z = 0.0f;
        gameObject.transform.localEulerAngles = newAngle;
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * MoveSpeed;  // 追尾
        //gameObject.transform.Translate(transform.forward * MoveSpeed);    // 追尾
    }

    private void AttackTarget() 
    {
        
    }

    public void DiscoveryTarget(GameObject target) 
    {
        TargetObject = target;
        IsChase = true;
        TargetPosList.Clear();
    }

    public void LostTarget() 
    {
        TargetObject = null;
        IsChase = false;
        TargetPosList.Clear();
    }
}
