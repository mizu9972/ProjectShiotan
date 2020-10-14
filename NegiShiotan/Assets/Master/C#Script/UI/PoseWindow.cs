using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PoseWindow : MonoBehaviour
{
    [SerializeField, Header("ポーズ画面用キャンバスオブジェクト")]
    private GameObject PoseCanvasObject = null;

    [SerializeField, Header("メインキャンパスの背景imageオブジェクト")]
    private Image MainCanvasImage = null;

    [SerializeField, Header("暗くするときの色")]
    private Color DarkMaskColor;
    private Color m_DefaultColor;

    [SerializeField, Header("ポーズ中選択できないようにするボタン群")]
    private List<Button> UnSelectabeButtons;

    private bool m_isPoseActive = false;//ポーズ画面を開いているか判定用
    // Start is called before the first frame update
    void Start()
    {
        if (PoseCanvasObject == null)
        {
            PoseCanvasObject = this.gameObject;
        }
        PoseCanvasObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isPoseActive == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenPoseWindow();
            }
        }
    }

    //ポーズ画面表示
    private void OpenPoseWindow()
    {
        //キャンパス有効化
        PoseCanvasObject.SetActive(true);

        //色変更
        m_DefaultColor = MainCanvasImage.color;
        MainCanvasImage.color = DarkMaskColor;

        //ボタン無効化
        foreach(var Button in UnSelectabeButtons)
        {
            Button.interactable = false;
        }

        m_isPoseActive = true;
    }

    //ポーズ画面非表示
    public void ClosePoseWindow()
    {
        //キャンパス無効化
        PoseCanvasObject.SetActive(false);

        //色戻す
        MainCanvasImage.color = m_DefaultColor;

        //ボタン有効化
        foreach (var Button in UnSelectabeButtons)
        {
            Button.interactable = true;
        }

        m_isPoseActive = false;
    }
}
