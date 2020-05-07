﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillOpenGateBase : MonoBehaviour
{
    [SerializeField,Header("倒す必要のある敵")]
    private List<GameObject> Enemy;

    [SerializeField, Header("右扉")]
    private GameObject RightDoor;
    [SerializeField, Header("左扉")]
    private GameObject LeftDoor;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 OldScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if (RightDoor) {
            Vector3 newScale = OldScale;
            RightDoor.transform.localScale = newScale;
            Vector3 newPos = gameObject.transform.localPosition;
            newPos.x += -OldScale.x;
            RightDoor.transform.position = newPos;
        }
        else {
            Debug.LogWarning("右扉が設定されていません");
        }
        if (LeftDoor) {
            Vector3 newScale = OldScale;
            LeftDoor.transform.localScale = newScale;
            Vector3 newPos = gameObject.transform.localPosition;
            newPos.x += OldScale.x;
            LeftDoor.transform.position = newPos;
        }
        else {
            Debug.LogWarning("左扉が設定されていません");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DeleteNullObject();
        if (Enemy.Count <= 0) {
            OpenGate();
        }
    }

    private void OpenGate() {
        time += Time.deltaTime;
        float rightangle = Mathf.LerpAngle(0.0f, 90.0f, time);
        RightDoor.transform.eulerAngles = new Vector3(0, rightangle, 0);
        float leftangle = Mathf.LerpAngle(0.0f, -90.0f, time);
        LeftDoor.transform.eulerAngles = new Vector3(0, leftangle, 0);
    }

    private void DeleteNullObject() {
        for(int i = 0; i < Enemy.Count; i++) {
            if(Enemy[i] == null) {
                Enemy.Remove(Enemy[i]);
            }
        }
    }
}
