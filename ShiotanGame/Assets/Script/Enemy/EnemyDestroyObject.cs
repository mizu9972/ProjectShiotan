using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyObject : MonoBehaviour
{
    public void DestroyObject() {
        Destroy(gameObject);
    }
}
