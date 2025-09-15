using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class pyrokinesis : MonoBehaviour
{
    #region Explosion
    [Header("Explosion")]
    [Space(5)]
    public float delay = 3f;

    float countdown;
    bool boomTime = false;

    public float explosionForce = 700f;
    public float radius;

    #endregion
    //_____________________________________________________________________________________________________________

    #region Placement
    [Header("Placemnt")]
    [Space(5)]

    public Camera Camera;
    public GameObject grenadePrefab;
    public GameObject displaySphere;

    public LayerMask ground;
    public float placemntDistance;

    public GameObject explosionEffect;

    public ParticleSystem explosionEffectPrefab;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !boomTime)
        {
            //Explode();
          //  boomTime = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))

        {
            boomTime = true ;
        }


            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placemntDistance, ground) && boomTime)
        {
           
            displaySphere.transform.position = hit.point;
            displaySphere.SetActive(true);
            

          
            displaySphere.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

         
            if (Input.GetMouseButtonDown(0))
            {
                Explode();
               // boomTime=true;
               // Instantiate(grenadePrefab, hit.point, Quaternion.identity);
            }
        }
        else
        {
           
            displaySphere.SetActive(false);
        }

        void Explode()
        {

            Instantiate(explosionEffect, hit.point, hit.transform.rotation);

            //explosionEffectPrefab.Play();


            Collider[] colliders = Physics.OverlapSphere(hit.point, radius);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null && boomTime )
                {
                    rb.AddExplosionForce(explosionForce, hit.point, radius);
                    boomTime = false;
                }

            }



          //  Destroy(gameObject);

        }



    }
}
