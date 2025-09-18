using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawne : MonoBehaviour
{
    public float threshold;


    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = new Vector3(-79.3f,32.5f,-54.7f);
        }
    }

    
}
