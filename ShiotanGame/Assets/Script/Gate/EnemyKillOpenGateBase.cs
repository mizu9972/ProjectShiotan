using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillOpenGateBase : MonoBehaviour
{
    [SerializeField,Header("倒す必要のある敵")]
    private List<GameObject> Enemy;

    // Start is called before the first frame update
    void Start()
    {
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
        Destroy(gameObject);
    }

    private void DeleteNullObject() {
        for(int i = 0; i < Enemy.Count; i++) {
            if(Enemy[i] == null) {
                Enemy.Remove(Enemy[i]);
            }
        }
    }
}
