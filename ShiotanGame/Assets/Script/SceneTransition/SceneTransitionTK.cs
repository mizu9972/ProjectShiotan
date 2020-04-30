using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;
public class SceneTransitionTK : MonoBehaviour
{
    [Header("マスクに使用できるテクスチャ")]
    public Texture2D maskTexture;

    [Header("フェード時間")]
    public float Duration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            var mask = new ImageMaskTransition()
            {
                maskTexture = maskTexture,
                backgroundColor = Color.black,
                nextScene = SceneManager.GetActiveScene().buildIndex,
                duration = Duration
            };
            TransitionKit.instance.transitionWithDelegate(mask);
        }
    }
}
