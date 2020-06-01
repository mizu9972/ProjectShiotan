using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckReturnStop : MonoBehaviour
{
    [Header("移動させる処理止める"), SerializeField]
    public GameObject ReturnStop;

    [Header("出す扉"), SerializeField]
    public GameObject[] CheckDoor;

    //一方通行にする
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<ProtoMove2>().enabled = true;    //プレイヤー動く処理　ON

            //出たとき動きとめる
            other.GetComponentInParent<ProtoMove2>().Stop();

            //扉出現
            for (int cnt = 0; cnt < 2; cnt++)
            {
                CheckDoor[cnt].SetActive(true);
            }

            AudioManager.Instance.PlaySE("SE_OPEN");

            ReturnStop.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
