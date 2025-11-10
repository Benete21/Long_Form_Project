using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawne : MonoBehaviour
{
    public float threshold = -10f;

    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            RespawnAtCheckpoint();
        }
    }

    private void RespawnAtCheckpoint()
    {
        Transform checkpoint = CheckPointManager.GetCurrentCheckpoint();
        
        if (checkpoint != null)
        {
            transform.position = checkpoint.position;
            transform.rotation = checkpoint.rotation;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Player cannot respawn.");
        }
    }
}
