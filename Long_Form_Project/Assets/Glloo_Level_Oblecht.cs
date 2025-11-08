using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glloo_Level_Oblecht : MonoBehaviour
{
    private Rigidbody rb;
    public float Gloo_Desolve;
    public Gun_Level_Object originGun;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnDestroy()
    {
        if (originGun != null)
            originGun.NotifyBulletDestroyed();

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sticky"))
        {
            rb.isKinematic = true;
        }
        else if (collision.gameObject.CompareTag("Melt"))
        {
            Destroy(gameObject);
        }
    }
}

