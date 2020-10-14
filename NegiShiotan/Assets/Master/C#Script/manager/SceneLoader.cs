using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Header("読み込むシーン名")]
    private string SceneName;

    [ContextMenu("シーン読み込み")]
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
