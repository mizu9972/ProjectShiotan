using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(SceneLoader))]
public class RetryButton : MonoBehaviour
{
    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 1.0f;

    private FadebyTex fadeCamera = null;
    private SceneLoader sceneLoader = null;

    [SerializeField, Header("SEPlayer")]
    private SEPlayer RetrySEPlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        fadeCamera = Camera.main.GetComponent<FadebyTex>();
        sceneLoader = this.gameObject.GetComponent<SceneLoader>();
    }


    //リトライ挙動
    public void retryFunction()
    {
        if (fadeCamera != null)
        {

            //Observable.Timer(System.TimeSpan.FromSeconds(FadeSpeed)).Subscribe(_ =>
            //fadeCamera.StartFadeOut()
            //);
            RetrySEPlayer.PlaySound();
            RetryScene();            
        }
    }

    private void RetryScene()
    {
        fadeCamera.setFeedinSpeed(FadeSpeed)
       .setFeedOutSpeed(FadeSpeed);

        fadeCamera.StartFadeOut();

        Observable.Timer(System.TimeSpan.FromSeconds(FadeSpeed))
            .Subscribe(_ => sceneLoader.LoadMyScene());
    }
}
