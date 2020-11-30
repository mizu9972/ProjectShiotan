using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField, Header("初期選択ボタン")]
    private Button InitialSelevtButton = null;
    // Start is called before the first frame update
    void Start()
    {
        if(InitialSelevtButton != null)
        {
            InitialSelevtButton.Select();
        }
    }

}
