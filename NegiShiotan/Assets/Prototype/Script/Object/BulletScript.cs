using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("弾　速度")]
    public float Speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime),
                                         transform.position.y,
                                         transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kabe")
        {
            Destroy(this.gameObject);
        }
    }
}
