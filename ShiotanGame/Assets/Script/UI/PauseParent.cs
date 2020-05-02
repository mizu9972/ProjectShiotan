using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParent : MonoBehaviour
{
    [Header("有効化するオブジェクト")]
    public GameObject OnActiveObj;
    // Start is called before the first frame update
    private void OnEnable()
    {
        OnActiveObj.SetActive(true);
        Time.timeScale = 0f;//更新処理の停止
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;//更新処理の再開
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
