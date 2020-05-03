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
        GameManager.Instance.PlayerControlStop();
        Time.timeScale = 0f;//更新処理の停止
        OnActiveObj.GetComponent<SelectItem>().SetState((int)SelectItem.MenuState.RESTART);
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;//更新処理の再開
        GameManager.Instance.PlayerControlStart();//プレイヤーの操作可能に
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
