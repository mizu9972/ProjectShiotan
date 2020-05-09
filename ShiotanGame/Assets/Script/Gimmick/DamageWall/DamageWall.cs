using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWall : MonoBehaviour
{
    [Header("与えるダメージ量"), SerializeField] private float Damage;

    [Header("ダメージを与える間隔"), SerializeField] private float DamageTime;

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
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //離れたらまたダメージをすぐに与えるようにしておく
        DamageCount = DamageTime;
    }

}
