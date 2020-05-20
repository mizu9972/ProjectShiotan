using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOpenGate : MonoBehaviour
{
    [Header("必要鍵敵")]
    public int NeedKeys = 0;

    public bool IsOpen = false;

    [SerializeField, Header("右扉")]
    private GameObject RightDoor;
    [SerializeField, Header("左扉")]
    private GameObject LeftDoor;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start() {
        //Vector3 OldScale = gameObject.transform.localScale;
        //gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //if (RightDoor) {
        //    Vector3 newScale = OldScale;
        //    RightDoor.transform.localScale = newScale;
        //    Vector3 newPos = gameObject.transform.localPosition;
        //    newPos.x += -OldScale.x;
        //    RightDoor.transform.position = newPos;
        //}
        //else {
        //    Debug.LogWarning("右扉が設定されていません");
        //}
        //if (LeftDoor) {
        //    Vector3 newScale = OldScale;
        //    LeftDoor.transform.localScale = newScale;
        //    Vector3 newPos = gameObject.transform.localPosition;
        //    newPos.x += OldScale.x;
        //    LeftDoor.transform.position = newPos;
        //}
        //else {
        //    Debug.LogWarning("左扉が設定されていません");
        //}
    }

    // Update is called once per frame
    void Update() {
        if (IsOpen) {
            OpenGate();
        }
    }

    private void OpenGate() {
        time += Time.deltaTime;
        float rightangle = Mathf.LerpAngle(gameObject.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.y + 90.0f, time);
        RightDoor.transform.eulerAngles = new Vector3(0, rightangle, 0);
        float leftangle = Mathf.LerpAngle(gameObject.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.y - 90.0f, time);
        LeftDoor.transform.eulerAngles = new Vector3(0, leftangle, 0);
    }
}
