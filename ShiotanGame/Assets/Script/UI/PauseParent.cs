using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParent : MonoBehaviour
{
    [Header("有効化するオブジェクト")]
    public GameObject OnActiveObj;

    [Header("ポーズ中の音量")]
    [Range(0f,1.0f)]public float PauseVol;

    private float NowVol;//現在のBGM音量
    // Start is called before the first frame update
    private void OnEnable()
    {
        NowVol = AudioManager.Instance.GetBgmVolume();//現在の音量取得しておく
        AudioManager.Instance.SetBgmVolume(PauseVol);//ポーズ中はBGMの音量を下げる
        OnActiveObj.SetActive(true);
        GameManager.Instance.PlayerControlStop();
        Time.timeScale = 0f;//更新処理の停止
        OnActiveObj.GetComponent<SelectItem>().SetState((int)SelectItem.MenuState.RESTART);
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;//更新処理の再開
        GameManager.Instance.PlayerControlStart();//プレイヤーの操作可能に
        AudioManager.Instance.SetBgmVolume(NowVol);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
