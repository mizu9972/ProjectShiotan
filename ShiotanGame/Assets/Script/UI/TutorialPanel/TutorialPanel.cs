using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// プレイヤーが触れた時に表示したいUIを表示するパネルにアタッチ
/// プレイヤーがたどり着いたときにUIを表示したい場所にオブジェクトを配置してください。
/// 例：流れの手前で連打ボタンを表示したいなら流れの手前にオブジェクトを配置
/// </summary>
public class TutorialPanel : MonoBehaviour
{
    [Header("表示するパネル")]
    public Image DrawPanel;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            DrawPanel.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            DrawPanel.enabled = false;
        }
    }
}
