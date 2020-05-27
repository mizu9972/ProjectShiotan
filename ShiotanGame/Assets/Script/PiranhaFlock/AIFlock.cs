using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum RushSE {
    None,
    Far,
    Near,
}

[RequireComponent(typeof(NavMeshAgent))]
public class AIFlock : MonoBehaviour
{
    private Vector3 InitPos;

    //+
    [SerializeField, Header("巡回ポイント")]
    private List<Vector3> PatrolPoint = new List<Vector3>();
    private int PatrolNum = 0;
    //-

    public List<GameObject> TargetList;
    public bool IsHit;
    public bool IsAttack;

    [SerializeField, Header("Rayをスルーするレイヤー")]
    private LayerMask Mask;
    private int IntLayerMask;
    [SerializeField, Header("Rayを飛ばす距離")]
    private float RayDistance;

    [SerializeField,Header("ピラニアがターゲットのフレーム前の方向を向くフレーム")]
    private int PiranhaChaceDirayFrame = 5;

    [SerializeField, Header("ピラニアの追いかける精度 　　　高精度<--->低精度"),Range(1,60,order = 1)]
    private int ChaceAccuracy = 1;
    [SerializeField] private List<Vector3> TargetPosList;
    [SerializeField] private float MoveSpeed;

    [SerializeField, Header("ピラニアがターゲットを見つけた時に表示画像")]
    private GameObject SurprisedMark;

    private NavMeshAgent m_NavMeshAgent;

    [SerializeField] private float SEFarDistance;
    public int RashSEChannel = -1;
    private RushSE NowSEType = RushSE.None;

    // Start is called before the first frame update
    void Start()
    {
        InitPos = gameObject.transform.position;

        //+
        PatrolPoint.Add(InitPos);

        for (int i = 0; i < gameObject.transform.childCount; i++) {
            if (gameObject.transform.GetChild(i).tag == "PatrolPoint") {
                PatrolPoint.Add(gameObject.transform.GetChild(i).gameObject.transform.position);
            }
        }
        //-

        m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        //m_NavMeshAgent.destination = InitPos;

        IntLayerMask = ~Mask.value;

        // ここだけNavMeshAgentを使う
        m_NavMeshAgent.destination = PatrolPoint[PatrolNum];
    }

    public void AIUpdate() 
    {
        // Missingになったオブジェクトがあれば削除する
        List<int> DeleteArrayNum = new List<int>();
        for(int i = 0; i < TargetList.Count; i++) {
            if(TargetList[i] == null) {
                TargetList.Remove(TargetList[i]);
            }
        }
        
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
                    if (TargetList[0] != null) {
                        if (IsHit = RayShot(TargetList[0])) {
                            // ビックリマーク表示
                            if(TargetPosList.Count == 0) {
                                SurprisedMark.transform.GetChild(0).GetComponent<LookCamera>().parentTransform = gameObject.transform;
                                Instantiate(SurprisedMark, gameObject.transform);
                                AudioManager.Instance.PlaySE("SE_FIND");
                            }

                            gameObject.GetComponent<HumanoidBase>().AttackObject = TargetList[0];
                            gameObject.GetComponent<PiranhaAnimation>().SetIsAttack(true);
                            gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            foreach(GameObject Piranha in gameObject.GetComponent<FlockBase>().ChildPiranha) {
                                Piranha.GetComponent<PiranhaBase>().SetPiranhaDirection(TargetList[0].transform);
                            }
                            break;
                        }
                        else {
                            // レイが当たらなかったターゲットは後ろに持ってくる
                            TargetList.Add(TargetList[0]);
                            TargetList.RemoveAt(0);
                            //TargetPosList.Clear();  //　追加分
                            //IsAttack = false; // 追加分
                        }
                    }
                    // ターゲットがいなくなった場合、削除
                    else {
                        TargetList.RemoveAt(0);
                    }
                }
            }
            // Targetがいないため初期位置に戻る処理
            else {
                gameObject.GetComponent<HumanoidBase>().AttackObject = null;
                gameObject.GetComponent<PiranhaAnimation>().SetIsAttack(false);
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                m_NavMeshAgent.destination = PatrolPoint[PatrolNum];
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                foreach (GameObject Piranha in gameObject.GetComponent<FlockBase>().ChildPiranha) {
                    Piranha.GetComponent<PiranhaBase>().SetPiranhaDirection(PatrolPoint[PatrolNum]);
                }
                TargetPosList.Clear();
            }
        }

        // ターゲットを見つけた場合ターゲットのほうに向かう
        if (IsHit) {
            if (TargetList.Count > 0) {
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
                TargetPosList.Add(TargetList[0].transform.position);
            }
            // ターゲットが見つからない場合初期位置に戻る
            else {
                gameObject.GetComponent<HumanoidBase>().AttackObject = null;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                m_NavMeshAgent.destination = PatrolPoint[PatrolNum];
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (RashSEChannel != -1) {
                    AudioManager.Instance.StopLoopSe(RashSEChannel);
                    NowSEType = RushSE.None;
                }
                TargetPosList.Clear();
            }
        }
        //+
        if (gameObject.GetComponent<NavMeshAgent>().enabled) {
            Patrol();
        }
        //-
    }

    // Rayをターゲットに飛ばして当たったかを返す
    private bool RayShot(GameObject TargetObj) 
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray(transform.position, Vector3.Normalize(TargetObj.transform.position - gameObject.transform.position));

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * 500.0f, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                         ↓Ray  ↓Rayが当たったオブジェクト ↓距離          ↓レイヤー
        if (Physics.Raycast(ray,  out hit,                           RayDistance, IntLayerMask)) {
            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.gameObject == TargetObj) {
                //Debug.Log("Rayが"+ TargetList[0] +"に当たった");
                return true;
            }
        }

        // もし、ターゲットと同じ座標にいたらtrueを返す
        if(gameObject.transform.position == TargetObj.transform.position) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ターゲット追尾
    /// </summary>
    private void ChaseTarget() {
        gameObject.transform.LookAt(TargetPosList[0]);  // ターゲットの方向を向く
        gameObject.transform.localEulerAngles = new Vector3(0.0f, gameObject.transform.localEulerAngles.y, 0.0f); // y軸のみ向かせる
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * MoveSpeed;  // 追尾

        // SE再生
        if(TargetList[0].tag == "Player") {
            if (Vector3.Distance(TargetList[0].transform.position, gameObject.transform.position) > SEFarDistance) {
                if (RashSEChannel == -1) {
                    // Far
                    if (NowSEType != RushSE.Far) {
                        RashSEChannel = AudioManager.Instance.PlayLoopSe("SE_CHASE_FAR", true);
                        NowSEType = RushSE.Far;
                    }
                }
            }
            else {
                // Near
                if (RashSEChannel == -1) {
                    if (NowSEType != RushSE.Near) {
                        RashSEChannel = AudioManager.Instance.PlayLoopSe("SE_CHASE", true);
                        NowSEType = RushSE.Near;
                    }
                }
            }
        }
        else {
            if (RashSEChannel != -1) {
                AudioManager.Instance.StopLoopSe(RashSEChannel);
                NowSEType = RushSE.None;
            }
        }
    }

    //+
    /// <summary>
    /// パトロール
    /// </summary>
    private void Patrol() {
        if (!m_NavMeshAgent.pathPending && m_NavMeshAgent.remainingDistance <= 0.1f) {
            PatrolNum++;
            if (PatrolNum >= PatrolPoint.Count) {
                PatrolNum = 0;
            }
            m_NavMeshAgent.destination = PatrolPoint[PatrolNum];
        }
    }
    //-

    // 強制散会
    public void CompulsionReturnPosition() {
        gameObject.GetComponent<HumanoidBase>().AttackObject = null;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        m_NavMeshAgent.destination = InitPos;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        TargetPosList.Clear();
    }

    // 帰宅したかどうか
    public bool ReturnHomeCompleted() {
        if(gameObject.GetComponent<NavMeshAgent>().remainingDistance <= 1.0f) {
            return true;
        }
        return false;
    }


    /// <summary>
    /// ターゲットのソート
    /// </summary>
    private void TargetSort() 
    {
        GameObject Player = null;
        GameObject[] newSort = new GameObject[TargetList.Count];

        // プレイヤーの探索
        if (TargetList[TargetList.Count - 1].tag != "Player") {
            foreach (GameObject Target in TargetList) {
                if (Target.tag == "Player") {
                    //if(Target == TargetList[0]) {
                    //    TargetPosList.Clear();  //　追加分
                    //    IsAttack = false; // 追加分
                    //}

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
        for(int i = 1; i < TargetList.Count; i++) {
            float Distance = GetAbsDistance(gameObject.transform.position, TargetList[i].transform.position);

            // 最小以下の時
            if(Distance <= MinDistance) {
                newSort = AddArray(newSort, TargetList[i], 0);
                MinDistance = Distance;
            }
            // 最大以上の時
            else if(Distance >= MaxDistance) {
                newSort[i] = TargetList[i];
                MaxDistance = Distance;
            }
            // 間に入れる場合
            else {
                for(int j = 0; j < newSort.Length; j++) {
                    if(Distance <= GetAbsDistance(gameObject.transform.position, newSort[j].transform.position)) {
                        newSort = AddArray(newSort, TargetList[i], j);
                    }
                }
            }
        }

        // ターゲットソートが終わってから最後にプレイヤーを入れる
        if (Player) {
            TargetList.Add(Player);
        }
    }

    /// <summary>
    /// 2つのVectorの距離を絶対値で返す
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private float GetAbsDistance(Vector3 a,Vector3 b) 
    {
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
    private T[] AddArray<T>(T[] Ret, T Set, int Num) 
    {
        T[] newT = new T[Ret.Length];
        int i = 0;
        foreach(T t in Ret) {
            if (t == null) break;
            newT[i] = t;
            i++;
        }

        newT[Num] = Set;
        for (;Num < Ret.Length - 1; Num++) {
            newT[Num + 1] = Ret[Num];
        }
        return newT;
    }
}