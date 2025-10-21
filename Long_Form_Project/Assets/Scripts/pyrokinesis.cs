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
    #endregion

    #region Input System
    [Header("Input")]
    private PlayerControls controls;
    private bool isUsingMouse = true;
    private Vector2 aimInput;
    #endregion

    void Awake()
    {
        // Initialize Input System
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
        
        // Subscribe to input events
        controls.Player.ActivatePyro.performed += OnActivatePyrokinesis;
        controls.Player.Fire.performed += OnFire;
        controls.Player.Look.performed += OnAim;
    }

    void OnDisable()
    {
        // Unsubscribe from input events
        controls.Player.ActivatePyro.performed -= OnActivatePyrokinesis;
        controls.Player.Fire.performed -= OnFire;
        controls.Player.Look.performed -= OnAim;
        
        controls.Disable();
    }

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        
        if (countdown <= 0f && !boomTime)
        {
            //Explode();
            //boomTime = true;
        }

        // Detect input device (mouse vs gamepad)
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
        {
            isUsingMouse = false;
        }
        else if (Mouse.current != null && Mouse.current.delta.ReadValue().magnitude > 0.1f)
        {
            isUsingMouse = true;
        }

        HandlePlacement();
    }

    void HandlePlacement()
    {
        Ray ray;
        
        if (isUsingMouse)
        {
            // Mouse aiming
            ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
        else
        {
            // Controller aiming (using center of screen + right stick offset)
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 aimOffset = aimInput * 100f; // Adjust sensitivity as needed
            ray = Camera.ScreenPointToRay(screenCenter + aimOffset);
        }

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
        }
    }

    // Input callbacks
    void OnActivatePyrokinesis(InputAction.CallbackContext context)
    {
        boomTime = true;
    }

    void OnFire(InputAction.CallbackContext context)
    {
        if (boomTime)
        {
            Ray ray;
            if (isUsingMouse)
            {
                ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            }
            else
            {
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Vector2 aimOffset = aimInput * 100f;
                ray = Camera.ScreenPointToRay(screenCenter + aimOffset);
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, placemntDistance, ground))
            {
                Explode(hit);
            }
        }
    }

    void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
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
                boomTime = false;
            }

            BreakScript breakable = nearbyObject.GetComponent<BreakScript>();
            if (breakable != null)
            {
                breakable.BreakIt();
            }
        }
    }
}