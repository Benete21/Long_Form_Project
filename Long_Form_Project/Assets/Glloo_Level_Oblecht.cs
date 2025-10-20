using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glloo_Level_Oblecht : MonoBehaviour
{
    private Rigidbody rb;
    public float Gloo_Desolve;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnDestroy()
    {
        // Make sure we don't go below zero
        Gun_Level_Object.activeBulletCount = Mathf.Max(0, Gun_Level_Object.activeBulletCount - 1);
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

