using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadEnemyClass : MonoBehaviour
{
    [SerializeField,Header("アニメーションデータ")]
    private RuntimeAnimatorController DeadAnimation;
    [SerializeField, Header("モデル")]
    private GameObject Model;

    public void StartDeadAnimation() {
        // 親オブジェクトの変更
        Model.transform.parent = null;
        Model.transform.position = new Vector3(Model.transform.position.x, Model.transform.position.y - 0.13f, Model.transform.position.z);

        // アニメーションの変更
        Model.GetComponent<Animator>().runtimeAnimatorController = DeadAnimation;

        //SE_GAMEOVERを鳴らす
        AudioManager.Instance.PlaySE("SE_EATEN");

        Destroy(gameObject);
    }
}
