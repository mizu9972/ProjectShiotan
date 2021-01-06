using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

[RequireComponent(typeof(SceneLoader))]
public class EndingManager : MonoBehaviour
{
    [SerializeField, Header("操作可能になる時間")]
    private float ControlTime = 5.0f;

    [SerializeField, Header("クリアBGMが流れ始める時間")]
    private float BGMTime = 4.5f;

    [SerializeField, Header("クリアSEPlayer")]
    private SEPlayer ClearSEPlayer = null;

    [SerializeField, Header("FadeByTex")]
    private FadebyTex EndingFade = null;

    private bool isControlable = false;
    // Start is called before the first frame update
    void Start()
    {
        isControlable = false;

        //クリアBGM開始
        Observable.Timer(System.TimeSpan.FromSeconds(BGMTime))
            .Subscribe(_ => ClearSEPlayer.PlaySound());

        //一定時間後操作可能に
        Observable.Timer(System.TimeSpan.FromSeconds(ControlTime))
            .Subscribe(_ => isControlable = true);
    }

    private void Update()
    {
        if(isControlable == true)
        {
            if (Input.anyKey)
            {
                EndingFade.StartWhiteOut();
                this.GetComponent<SceneLoader>().LoadScenebyTimer();

                isControlable = false;
            }
        }
    }
}
