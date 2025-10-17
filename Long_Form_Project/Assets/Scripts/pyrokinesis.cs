using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
    
    private PlayerControls controls;
    private bool usePyroPressed;
    private bool activatePyroPressed;
    
    void Awake()
        {
            controls = new PlayerControls();
    
            // Input callbacks
            controls.Player.UsePyro.performed += ctx => usePyroPressed = true;
            controls.Player.UsePyro.canceled += ctx => usePyroPressed = false;
    
            controls.Player.ActivatePyro.performed += ctx => activatePyroPressed = true;
            controls.Player.ActivatePyro.canceled += ctx => activatePyroPressed = false;
        }
    
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

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

        if (activatePyroPressed)

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

         
            if (usePyroPressed)
            {
                Explode();
               // boomTime=true;
               // Instantiate(grenadePrefab, hit.point, Quaternion.identity);
            }
        }
        else
        {
           
            displaySphere.SetActive(false);
            displaySphere.transform.Rotate(Vector3.right * 5 * Time.deltaTime);
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
                if (breakable != null)
                {
                    breakable.BreakIt();
                }

            }



          //  Destroy(gameObject);

        }



    }
}
