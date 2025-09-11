using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloo_Bullet_Scrpit : MonoBehaviour
{
    private Rigidbody rb;
    public float Gloo_Desolve;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moving_Object"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Sticky"))
        {
            rb.isKinematic = true;
            Destroy(gameObject,Gloo_Desolve);
        }

    }
}
