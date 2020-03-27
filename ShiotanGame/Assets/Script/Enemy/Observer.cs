using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//利用する通知属性はここに追加する
public enum NotifyAttribute
{
    BUOY_UP,
    BUOY_DOWN
}

//監視オブジェクトインターフェース
public interface IObserver
{
    void OnNotify(NotifyAttribute notify);
}

//監視される側クラス
public class Subject : MonoBehaviour
{
    private List<IObserver> m_ObserverList = new List<IObserver>();

    //監視オブジェクト追加
    public void AddObserver(IObserver observer_) {
        m_ObserverList.Add(observer_);
    }

    //通知送信
    public void Notify(NotifyAttribute notify)
    {
        foreach (var Observer in m_ObserverList){
            Observer.OnNotify(notify);
        }
    }
}