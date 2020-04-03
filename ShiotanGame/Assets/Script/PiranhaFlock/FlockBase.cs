using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBase : MonoBehaviour {

    [SerializeField, Header("ピラニアオブジェクト")]
    private GameObject Piranha;      // ピラニアオブジェクトを入れるもの（なくてもできるけど一応配置してる）

    [SerializeField, Header("ピラニア生成数")]
    private int PiranihaCount = 0;   // プランナーがレベルデザインやりやすい用

    [HideInInspector] public List<GameObject> ChildPiranha;   // スクリプトで管理しないと、子の検索をかけた時に全ての子を返してくれないことがあったのでこういう設計にしています。

    [SerializeField, Header("ピラニア生成の生み出す誤差")]
    private Vector3 InstantPositionCorrct;     // ピラニア生成の座標の誤差をどこまで設定しますか?

    private void Awake()
    {
        // ピラニアカウント数分ピラニアを生成する
        for (int i = 0; i < PiranihaCount; i++) {
            Vector3 CreatePos = new Vector3(Random.Range(-InstantPositionCorrct.x, InstantPositionCorrct.x), Random.Range(-InstantPositionCorrct.y, InstantPositionCorrct.y), Random.Range(-InstantPositionCorrct.z, InstantPositionCorrct.z));
            GameObject newObj = Instantiate(Piranha, gameObject.transform.position + CreatePos, Quaternion.identity, gameObject.transform);
            newObj.transform.localScale = new Vector3(1 / gameObject.transform.localScale.x, 1 / gameObject.transform.localScale.y, 1 / gameObject.transform.localScale.z); // サイズ指定

            // ToDo::ピラニアの動き方等もここで設定したい
            ChildPiranha.Add(newObj);
        }
    }

    void Start() 
    {
    }

    void Update() 
    {
        // 群衆AIの処理を行う
        gameObject.GetComponent<AIFlock>().AIUpdate();

        // 各ピラニアの動き
        if (ChildPiranha.Count > 0) {
            // 子のすべての動きをここで再現させる
            foreach (GameObject obj in ChildPiranha) {
                // とりあえずピラニアのアップデートを掛ける
                obj.GetComponent<PiranhaBase>().PiranhaUpdate();

                // ToDo::追いかけ状態かそうでないかで動きが変わるかも？
            }
        }
        else {
            Debug.LogWarning(gameObject.transform.name + "にはピラニアが1匹もいません。");
        }
    }
}
