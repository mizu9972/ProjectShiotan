using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteWallShowHP : MonoBehaviour
{
    [SerializeField, Header("表示するHPバー")]
    private GameObject ShowHPBar;
    private List<GameObject> EnemyList;

    private void Start() {
        EnemyList = new List<GameObject>();
        gameObject.transform.parent.GetComponent<EnemyHpBase>().enabled = false;
        ShowHPBar.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "AttackField") {
            gameObject.transform.parent.GetComponent<EnemyHpBase>().enabled = true;
            ShowHPBar.SetActive(true);
            if (!EnemyList.Contains(other.gameObject)) {
                EnemyList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        foreach(GameObject List in EnemyList) {
            if(other.gameObject == List) {
                EnemyList.Remove(other.gameObject);
                break;
            }
        }
        if(EnemyList.Count <= 0) {
            gameObject.transform.parent.GetComponent<EnemyHpBase>().enabled = false;
            ShowHPBar.SetActive(false);
        }
    }
}
