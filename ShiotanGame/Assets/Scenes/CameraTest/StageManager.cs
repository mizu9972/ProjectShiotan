﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class StageManager : MonoBehaviour
{
    [SerializeField, Header("ステージの幅")]
    private FloatReactiveProperty StageWidth = new FloatReactiveProperty(1.0f);

    [SerializeField, Header("ステージの奥行き")]
    private FloatReactiveProperty StageDepth = new FloatReactiveProperty(1.0f);

    [SerializeField,Header("Transform")]
    private Transform MyTrans;
    
    // Start is called before the first frame update
    void Start()
    {
        //値の変更を検知して更新処理を実行するストリームの登録
        StageWidth.Subscribe(_ => StageUpdate());
        StageDepth.Subscribe(_ => StageUpdate());

        if (!MyTrans)
        {
            MyTrans = this.GetComponent<Transform>();
        }
        MyTrans.localScale = new Vector3(StageWidth.Value, 1.0f, StageDepth.Value);
    }

    void Update()
    {
        
    }

    [ContextMenu("StageUpdate")]
    void StageUpdate()
    {
        MyTrans.localScale = new Vector3(StageWidth.Value, 1.0f, StageDepth.Value);
    }
}
