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
    #endregion

    #region Placement
    [Header("Placement")]
    [Space(5)]
    public Camera Camera;
    public GameObject grenadePrefab;
    public GameObject displaySphere;
    public LayerMask ground;
    public float placemntDistance;
    public GameObject explosionEffect;
    public ParticleSystem explosionEffectPrefab;
    public RawImage pyro;
    public RawImage psiBlast;
    #endregion

    #region Input
    [Header("Input")]
    [Space(5)]
    public InputActionReference activatePyro;
    public InputActionReference fire;
    #endregion

    private PlayerInput playerInput;
    private bool isActive = false;

    AudioManager audioManager;

    void Start()
    {
        countdown = delay;
        
        // Enable input actions
        if (activatePyro != null)
        {
            activatePyro.action.Enable();
            activatePyro.action.performed += OnTogglePyrokinesis;
        }
        
        if (fire != null)
        {
            fire.action.Enable();
            fire.action.performed += OnShoot;
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnDestroy()
    {
        // Clean up event subscriptions
        if (activatePyro != null)
        {
            activatePyro.action.performed -= OnTogglePyrokinesis;

        }
        
        if (fire != null)
        {
            fire.action.performed -= OnShoot;
        }
    }

    private void OnTogglePyrokinesis(InputAction.CallbackContext context)
    {
        
        isActive = !isActive;
        boomTime = isActive;

        pyro.gameObject.SetActive(isActive);
        psiBlast.gameObject.SetActive(!isActive);

        if (isActive)
        {
            audioManager.PlaySFX(audioManager.pyrokinesis);
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (isActive && boomTime)
        {
            TryExplode();
        }
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0f && !boomTime)
        {
            // Auto countdown logic if needed
        }

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placemntDistance, ground) && boomTime)
        {
            displaySphere.transform.position = hit.point;
            displaySphere.SetActive(true);
            displaySphere.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            displaySphere.SetActive(false);
            displaySphere.transform.Rotate(Vector3.right * 5 * Time.deltaTime);
        }

         if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            boomTime= false;
            
                pyro.gameObject.SetActive(false);
        }
    }

    void TryExplode()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placemntDistance, ground))
        {
            Explode(hit);
        }
    }

    void Explode(RaycastHit hit)
    {
        Instantiate(explosionEffect, hit.point, hit.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(hit.point, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && boomTime)
            {
                rb.AddExplosionForce(explosionForce, hit.point, radius);
               
            }

            BreakScript breakable = nearbyObject.GetComponent<BreakScript>();
            if (breakable != null && breakable.CompareTag("Wood"))
            {
                breakable.BreakIt();
            }
        }
        
        boomTime = false;
        isActive = false;
         pyro.gameObject.SetActive (false);
    }
}