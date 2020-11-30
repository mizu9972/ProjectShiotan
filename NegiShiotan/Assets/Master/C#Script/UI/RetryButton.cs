using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RetryButton : MonoBehaviour
{
    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 1.0f;

    private FadebyTex fadeCamera = null;
    private SceneLoader sceneLoader = null;
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

            StartCoroutine("RetryScene");            
        }
    }

    private IEnumerator RetryScene()
    {
        fadeCamera.setFeedinSpeed(FadeSpeed)
       .setFeedOutSpeed(FadeSpeed);

        fadeCamera.StartFadeOut();

        yield return new WaitForSeconds(FadeSpeed);

        sceneLoader.LoadMyScene();
    }
}
