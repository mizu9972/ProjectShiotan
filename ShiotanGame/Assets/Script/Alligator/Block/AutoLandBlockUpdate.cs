using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLandBlockUpdate : MonoBehaviour
{
    [SerializeField] private Transform SideBlock;

    private void Awake() {
        gameObject.transform.position = SideBlock.position;
        gameObject.transform.rotation = SideBlock.rotation;

        Vector3 newScale = SideBlock.localScale;
        newScale.x -= 0.01f;
        newScale.y += 0.001f;
        newScale.z -= 0.01f;
        gameObject.transform.localScale = newScale;
    }
}
