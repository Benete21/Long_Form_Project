using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    public GameObject fractured;
    public GameObject pyroBall;


    public RawImage pyroImage;

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

    #region Sphere
    [Header("Sphere Scaling")]
    public float scaleSpeed = 2f;        
    public float maxScale = 2f;          
    private Vector3 originalScale;       
    private Coroutine currentScaleRoutine;
    public Camera playerCamera;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;

        if (displaySphere != null)
            originalScale = displaySphere.transform.localScale;
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
            pyroImage.gameObject.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           boomTime= false ;
            pyroImage.gameObject.SetActive(false);

        }


        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placemntDistance, ground) && boomTime)
        {
           
            displaySphere.transform.position = hit.point;
            displaySphere.SetActive(true);
            

          
            displaySphere.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

           

            if (Input.GetMouseButtonDown(2))
            {
                Explode();
                boomTime = false;
                pyroImage.gameObject.SetActive(false);
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

                BreakScript breakable = nearbyObject.GetComponent<BreakScript>();
                if (breakable != null && breakable.CompareTag("Wood"))
                {
                    breakable.BreakIt();
                }

            }



          //  Destroy(gameObject);

        }



    }
}
