using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPillarc : MonoBehaviour
{
    private int Frame = 0;

    private Vector3 InitPos;

    public List<GameObject> TargetList;
    public bool IsHit;
    public bool IsAttack;

    [SerializeField, Header("Rayをスルーするレイヤー")]
    private LayerMask Mask;
    private int IntLayerMask;
    [SerializeField, Header("Rayを飛ばす距離")]
    private float RayDistance;

    [SerializeField, Header("ピラルクがターゲットのフレーム前の方向を向くフレーム")]
    private int PiranhaChaceDirayFrame = 5;

    [SerializeField, Header("ピラルクの追いかける精度 　　　高精度<--->低精度"), Range(1, 60, order = 1)]
    private int ChaceAccuracy = 1;
    [SerializeField] private List<Vector3> TargetPosList;
    [SerializeField] private float MoveSpeed;

    private NavMeshAgent m_NavMeshAgent;

    // Start is called before the first frame update
    void Start() {
        InitPos = gameObject.transform.position;

        m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        //m_NavMeshAgent.destination = InitPos;

        IntLayerMask = ~Mask.value;

        // ここだけNavMeshAgentを使う
        m_NavMeshAgent.destination = InitPos;
    }

    public void AIUpdate() {
        Frame++;

        // ターゲットが2以上の時にソートを行う
        if (TargetList.Count > 1) {
            TargetSort();
        }

        // Rayを飛ばす
        IsHit = false;
        if (IsAttack) {
            IsHit = true;
        }
        else {
            if (TargetList.Count > 0) {
                for (int i = 0; i < TargetList.Count; i++) {
                    if (IsHit = RayShot(TargetList[0])) {
                        gameObject.GetComponent<NavMeshAgent>().enabled = false;
                        break;
                    }
                    else {
                        TargetList.Add(TargetList[0]);
                        TargetList.RemoveAt(0);
                    }
                }
            }
            // Targetがいないため初期位置に戻る処理
            else {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                m_NavMeshAgent.destination = InitPos;
                TargetPosList.Clear();
            }
        }

        // ターゲットを見つけた場合ターゲットのほうに向かう
        if (IsHit) {
            //m_NavMeshAgent.destination = TargetList[0].transform.position;
            // ディレイフレームを超え始めるとターゲットを追尾し、最初の座標を削除していく
            if (TargetPosList.Count > PiranhaChaceDirayFrame) {
                TargetPosList.RemoveAt(0);
                if (Time.frameCount % ChaceAccuracy == 0) {
                    ChaseTarget();
                }
            }
            else {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            // ToDo::もし、ついていたら追いかけずに攻撃する
            TargetPosList.Add(TargetList[0].transform.position);
        }
        // ターゲットが見つからない場合初期位置に戻る
        else {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            m_NavMeshAgent.destination = InitPos;
            TargetPosList.Clear();
        }
    }

    // Rayをターゲットに飛ばして当たったかを返す
    private bool RayShot(GameObject TargetObj) {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray(transform.position, Vector3.Normalize(TargetObj.transform.position - gameObject.transform.position));

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * 500.0f, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                         ↓Ray  ↓Rayが当たったオブジェクト ↓距離          ↓レイヤー
        if (Physics.Raycast(ray, out hit, RayDistance, IntLayerMask)) {
            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.gameObject == TargetObj) {
                // Debug.Log("RayがTargetに当たった");
                return true;
            }
        }

        // もし、ターゲットと同じ座標にいたらtrueを返す
        if (gameObject.transform.position == TargetObj.transform.position) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ターゲット追尾
    /// </summary>
    private void ChaseTarget() {
        gameObject.transform.LookAt(TargetPosList[0]);  // ターゲットの方向を向く
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * MoveSpeed;  // 新追尾
    }


    /// <summary>
    /// ターゲットのソート
    /// </summary>
    private void TargetSort() {
        GameObject Player = null;
        GameObject[] newSort = new GameObject[TargetList.Count];

        // プレイヤーの探索
        if (TargetList[TargetList.Count - 1].tag != "Player") {
            foreach (GameObject Target in TargetList) {
                if (Target.tag == "Player") {
                    Player = Target;
                    TargetList.Remove(Player);
                    break;
                }
            }
        }

        // その他のターゲットをソート
        float MinDistance = GetAbsDistance(gameObject.transform.position, TargetList[0].transform.position);
        float MaxDistance = GetAbsDistance(gameObject.transform.position, TargetList[0].transform.position);
        newSort[0] = TargetList[0];
        for (int i = 1; i < TargetList.Count; i++) {
            float Distance = GetAbsDistance(gameObject.transform.position, TargetList[i].transform.position);

            // 最小以下の時
            if (Distance <= MinDistance) {
                newSort = AddArray(newSort, TargetList[i], 0);
                MinDistance = Distance;
            }
            // 最大以上の時
            else if (Distance >= MaxDistance) {
                newSort[i] = TargetList[i];
                MaxDistance = Distance;
            }
            // 間に入れる場合
            else {
                for (int j = 0; j < newSort.Length; j++) {
                    if (Distance <= GetAbsDistance(gameObject.transform.position, newSort[j].transform.position)) {
                        newSort = AddArray(newSort, TargetList[i], j);
                    }
                }
            }
        }

        // ターゲットソートが終わってから最初にプレイヤーを入れる
        if (Player) {
            TargetList.Insert(0, Player);
        }
    }

    /// <summary>
    /// 2つのVectorの距離を絶対値で返す
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private float GetAbsDistance(Vector3 a, Vector3 b) {
        return Mathf.Abs(Vector3.Distance(a, b));
    }

    /// <summary>
    /// どこの配列に入れたいかを指定して配列の間に入れる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Ret">入れたい配列</param>
    /// <param name="Set">入れたい変数</param>
    /// <param name="Num">入れたい場所（添え字）</param>
    /// <returns></returns>
    private T[] AddArray<T>(T[] Ret, T Set, int Num) {
        T[] newT = new T[Ret.Length];
        int i = 0;
        foreach (T t in Ret) {
            if (t == null) break;
            newT[i] = t;
            i++;
        }

        newT[Num] = Set;
        for (; Num < Ret.Length - 1; Num++) {
            newT[Num + 1] = Ret[Num];
        }
        return newT;
    }

    #region 当たり判定系

    #endregion
}
