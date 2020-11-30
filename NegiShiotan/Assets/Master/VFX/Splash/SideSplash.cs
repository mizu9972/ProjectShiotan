using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SideSplash : MonoBehaviour
{
    private GameObject collision;

    private VisualEffect ve;

    void Start()
    {
        collision = transform.GetChild(0).gameObject;
        collision.GetComponent<MeshRenderer>().enabled = false;        

        ve = gameObject.GetComponent<VisualEffect>();
        ve.SetVector3("ColliderPos1", collision.transform.position);
        ve.SetFloat("ColliderRadius1", collision.transform.lossyScale.x * 0.5f);
        ve.SetFloat("ColliderHeight1", collision.transform.lossyScale.y * 2.0f);
    }

    private void FixedUpdate() {
        ve.SetVector3("ColliderPos1", collision.transform.position);
        ve.SetFloat("ColliderRadius1", collision.transform.lossyScale.x * 0.5f);
        ve.SetFloat("ColliderHeight1", collision.transform.lossyScale.y * 2.0f);
    }
}
