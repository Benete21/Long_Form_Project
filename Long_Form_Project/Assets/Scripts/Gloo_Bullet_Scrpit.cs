using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloo_Bullet_Scrpit : MonoBehaviour
{
    private Rigidbody rb;
    public float Gloo_Desolve;

    AudioManager audioManager;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moving_Object"))
        {
            collision.gameObject.GetComponent<Moving_Object_Stop>().StopMovement();
            audioManager.PlaySFX(audioManager.glooCollision);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Mirror"))
        {
            collision.gameObject.GetComponent<Moving_Object_Stop>().StopMovement();
            audioManager.PlaySFX(audioManager.glooCollision);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Sticky"))
        {
            rb.isKinematic = true;
            audioManager.PlaySFX(audioManager.glooCollision);
            Destroy(gameObject,Gloo_Desolve);
        }
        else if (collision.gameObject.CompareTag("Melt"))
        {
            audioManager.PlaySFX(audioManager.glooCollision);
            Destroy(gameObject);
        }

    }

}
