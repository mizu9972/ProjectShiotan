using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PoseWindow : MonoBehaviour
{
    [SerializeField, Header("ポーズ画面用キャンバスオブジェクト")]
    private GameObject PoseCanvasObject = null;

    [SerializeField, Header("メインキャンパスの背景imageオブジェクト(あれば)")]
    private Image MainCanvasImage = null;

    [SerializeField, Header("暗くするときの色")]
    private Color DarkMaskColor = Color.grey;
    private Color m_DefaultColor;

    [SerializeField, Header("ポーズ中選択できないようにするボタン群")]
    private List<Button> UnSelectabeButtons = new List<Button>();

    [SerializeField, Header("初期選択ボタン")]
    private Button InitialSelectButton = null;

    [SerializeField, Header("キャンセルSEPlayer")]
    private SEPlayer cancelSEPlayer = null;

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
            if (Input.GetButtonDown("Pose"))
            {//開く
                OpenPoseWindow();
            }
        }
        else
        {
            if (Input.GetButtonDown("Pose"))
            {//閉じる
                ClosePoseWindow();
                cancelSEPlayer.PlaySound();
            }
        }
    }

    //ポーズ画面表示
    private void OpenPoseWindow()
    {
        //キャンパス有効化
        PoseCanvasObject.SetActive(true);

        if (MainCanvasImage != null)
        {
            //色変更
            m_DefaultColor = MainCanvasImage.color;
            MainCanvasImage.color = DarkMaskColor;
        }
        //ボタン無効化
        foreach(var Button in UnSelectabeButtons)
        {
            Button.interactable = false;
        }
        //時間停止
        Time.timeScale = 0;

        InitialSelectButton.Select();

        m_isPoseActive = true;
    }

    //ポーズ画面非表示
    public void ClosePoseWindow()
    {
        //キャンパス無効化
        PoseCanvasObject.SetActive(false);

        //色戻す
        if (MainCanvasImage != null)
        {
            MainCanvasImage.color = m_DefaultColor;

        }
        //ボタン有効化
        foreach (var Button in UnSelectabeButtons)
        {
            Button.interactable = true;
        }
        //時間再開
        Time.timeScale = 1;

        m_isPoseActive = false;
    }
}
