using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneTable{
    MyScene,
    StageSelect
}
public class PreSceneLoader : MonoBehaviour
{
    [SerializeField, Header("ステージ選択シーン読み込みコンポーネント")]
    private SceneLoader selectSceneLoader = null;

    private int m_mySceneBuildIndex = 0;

    //シーン事前読み込み用
    private AsyncOperation myScene = null;//自身


    // Start is called before the first frame update
    void Start()
    {
        m_mySceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        PreLoadScenes();
    }


    //シーン事前読み込み
    private void PreLoadScenes()
    {
        myScene = SceneManager.LoadSceneAsync(m_mySceneBuildIndex);
        myScene.allowSceneActivation = false;
        selectSceneLoader.PreLoadScene();
    }

    //シーン有効化
    public void SceneActivate(SceneTable sceneTable)
    {
        switch (sceneTable)
        {
            case SceneTable.MyScene:
                myScene.allowSceneActivation = true;
                selectSceneLoader.UnloadScene();
                break;

            case SceneTable.StageSelect:
                selectSceneLoader.LoadScene();
                SceneManager.UnloadSceneAsync(m_mySceneBuildIndex);
                break;
        }
    }

    public void MySceneActivate()
    {
        SceneActivate(SceneTable.MyScene);
    }

    public void SelectSceneActivate()
    {
        SceneActivate(SceneTable.StageSelect);
    }
}
