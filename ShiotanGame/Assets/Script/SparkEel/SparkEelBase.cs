﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SparkEelBase : MonoBehaviour
{
    [SerializeField] private GameObject SparkField;

    void Start() {
    }

    void Update() {
        // 電気ウナギAIの処理を行う
        gameObject.GetComponent<AISparkEel>().AIUpdate();

        // 電撃処理
        SparkField.GetComponent<SparkEelSparkAttackField>().SparkFieldUpdate();
    }
}
