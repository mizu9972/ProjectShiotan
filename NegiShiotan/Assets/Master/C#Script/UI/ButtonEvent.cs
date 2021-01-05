using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField, Header("選択白線")]
    private GameObject WhiteLine = null;

    [SerializeField, Header("決定赤線")]
    private GameObject RedLine = null;

    private void Start()
    {
        WhiteLine.SetActive(false);
        RedLine.SetActive(false);
    }

    //選ばれたとき
    public void SelectEvent()
    {
        WhiteLine.SetActive(true);

        RedLine.SetActive(false);
    }

    //選ばれなくなったとき
    public void DeselectEvent()
    {
        WhiteLine.SetActive(false);
    }

    //決定したとき
    public void ClickEvent()
    {
        WhiteLine.SetActive(false);
        RedLine.SetActive(true);
    }
}
