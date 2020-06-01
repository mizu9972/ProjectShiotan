using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHopupBase : MonoBehaviour
{
    [SerializeField, Header("どこまで上に上げるか")]
    private float Up;
    [SerializeField, Header("上に上がるまでの時間")]
    private float UpTime;
    [SerializeField, Header("上で何秒止まるか")]
    private float StopTopTime;

    private float CreateY;
    private float timecntA = 0.0f;
    private float timecntB = 0.0f;

    private void Start() {
        CreateY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // 上にあげる処理
        if(CreateY + Up > gameObject.transform.position.y) {
            Vector3 newPosition = gameObject.transform.position;
            newPosition.y = CreateY + ((CreateY + Up) - CreateY) *((UpTime * 2 - timecntA + 1) * timecntA / 2.0f) / ((UpTime + 1) * UpTime / 2.0f);
            gameObject.transform.position = newPosition;
            timecntA += Time.deltaTime;
        }
        else {
            // ストップ処理
            if (timecntB < StopTopTime) {
                timecntB += Time.deltaTime;
            }
            // 削除処理
            else {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g,
                    gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a - 0.1f);
                if (gameObject.GetComponent<SpriteRenderer>().color.a <= 0.0f) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
