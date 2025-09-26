using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Laser_Mirror : MonoBehaviour
{
    public Material material;
    public Laser_Reflect_Mirror beam;

    void Update()
    {
        Destroy(GameObject.Find("New Game Object"));
       beam = new Laser_Reflect_Mirror(gameObject.transform.position, gameObject.transform.right, material); 
    }
}
