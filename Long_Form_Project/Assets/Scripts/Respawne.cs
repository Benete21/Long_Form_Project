using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawne : MonoBehaviour
{
    public Transform respawnPoint;
    public float threshold;


    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = respawnPoint.position;
        }
    }

    
}
