using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class UIScript : MonoBehaviour
{
    [Header("HPUI")]
    public Image[] HPUI;

    private int MAXHP;
    private int HPnumber = 0;
    private int BeforeHP = 0;

    [Header("PLAYER")]
    public Status UIStatus;

    // Start is called before the first frame update
    void Start()
    {
        MAXHP = UIStatus.GetMAXHP();
        HPnumber = MAXHP;
        BeforeHP = HPnumber;
    }

    // Update is called once per frame
    void Update()
    {
        HPnumber = UIStatus.GetHP();

        //HP減少
        if (HPnumber<BeforeHP)
        {
            while (BeforeHP != HPnumber)
            {
                BeforeHP--;
                if (BeforeHP < 0)
                {
                    BeforeHP = 0;
                    Debug.Log("break");
                    break;
                }
                else
                {
                    HPUI[BeforeHP].enabled = false;
                }
            }
        }

        //HP増加
        if(HPnumber>BeforeHP)
        {
            while (BeforeHP != HPnumber)
            {
                BeforeHP++;
                if (BeforeHP > MAXHP)
                {
                    BeforeHP = MAXHP;
                    Debug.Log("break");
                    break;
                }
                else
                {
                    HPUI[BeforeHP-1].enabled = true;
                }
            }
        }

    }
}
