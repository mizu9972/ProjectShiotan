using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWall : MonoBehaviour
{
    [Header("与えるダメージ量"), SerializeField] private float Damage;

    [Header("ダメージを与える間隔"), SerializeField] private float DamageTime;

    [Header("ダメージの衝撃の強さ"), SerializeField] private float DamageImpact;

    //壁に触れている時間を計算する用
    private float DamageCount;

    private HumanoidBase Humanoid;

    // Start is called before the first frame update
    void Start()
    {
        DamageCount = DamageTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 壁に当たったらダメージを与えるメソッド
    private void OnCollisionStay(Collision other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        //ダメージを与える間隔
        DamageCount += Time.deltaTime;

        //一定時間ダメージ壁に触れているとダメージ（初回はすぐにダメージ）
        if (DamageTime < DamageCount)
        {
            if (layerName == "Player")
            {
                other.gameObject.GetComponentInParent<HumanoidBase>().Damage(Damage);
                DamageCount = 0;

                Vector3 StanVec = GetAngleVec(this.gameObject, other.gameObject);
                other.gameObject.GetComponent<Rigidbody>().AddForce(StanVec * DamageImpact, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //離れたらまたダメージをすぐに与えるようにしておく
        DamageCount = DamageTime;
    }

    Vector3 GetAngleVec(GameObject _from, GameObject _to)
    {
        //高さの概念を入れないベクトルを作る
        Vector3 fromVec = new Vector3(_from.transform.position.x, 0, _from.transform.position.z);
        Vector3 toVec = new Vector3(_to.transform.position.x, 0, _to.transform.position.z);

        return Vector3.Normalize(toVec - fromVec);
    }

}
