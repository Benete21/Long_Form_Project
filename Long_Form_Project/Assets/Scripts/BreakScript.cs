using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{

    public GameObject fractured;
    public float breakForce;
    public float explosionForce;
    public float radius;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            BreakIt();
        }
    }

    public void BreakIt()
    {
        GameObject frac = Instantiate(fractured, transform.position,transform.rotation);

        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }

        Destroy(gameObject);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Psi-Blast"))

        {
            BreakIt();

        }
    }



}
