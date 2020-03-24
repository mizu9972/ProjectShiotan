using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyNav, IObserver
{
    //IObserver実装
    //ブイの動きの通知を受け取る
    void Start()
    {
        base.Init();
        m_Player.GetComponent<BuiScript>().AddObserver(this);
    }

    void Update()
    {
        if (isChaseActive)
        {
            base.Chase();
        }
    }

    public void OnNotify(NotifyAttribute notify)
    {
        switch (notify) {
            case NotifyAttribute.BUOY_UP:
                m_TargetObject = null;
                
                break;

            case NotifyAttribute.BUOY_DOWN:
                m_TargetObject = m_Player;

                break;
        }
    }
}
