using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PsiBlist : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject psiBlastPrefab;
    public Transform shootPoint;
    public Text ammoStatusText;

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float fireRate = 0.2f; // Time between shots (lower = faster)
    public int maxAmmo = 10;

    [Header("Input")]
    public InputActionReference activatePsi;
    public InputActionReference altFire;

    private bool canShoot = false;
    private int currentAmmo;
    private float nextFireTime = 0f;
    public RawImage psiblast;

    private void Start()
    {
        canShoot = false;
        currentAmmo = maxAmmo;

        // Enable input actions
        if (activatePsi != null)
        {
            activatePsi.action.Enable();
            activatePsi.action.performed += OnTogglePsiBlast;
        }

        if (altFire != null)
        {
            altFire.action.Enable();
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        if (activatePsi != null)
        {
            activatePsi.action.performed -= OnTogglePsiBlast;
        }
    }

    private void OnTogglePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = true;
        if (psiblast != null)
        {
            psiblast.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Keep keyboard fallback for backward compatibility
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            canShoot = true;
            if (psiblast != null)
            {
                psiblast.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            canShoot = false;
            if (psiblast != null)
            {
                psiblast.gameObject.SetActive(false);
            }
        }

        // Continuous shooting when button/trigger is held
        if (canShoot && Time.time >= nextFireTime)
        {
            // Check if shoot button is being held (works for both mouse and controller)
            bool isHoldingShoot = false;

            // Check controller trigger
            if (altFire != null && altFire.action.ReadValue<float>() > 0.1f)
            {
                isHoldingShoot = true;
            }

            // Check mouse button (fallback)
            if (Input.GetMouseButton(1))
            {
                isHoldingShoot = true;
            }

            if (isHoldingShoot)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        GameObject psiBullet = Instantiate(psiBlastPrefab, shootPoint.position, Quaternion.Euler(90, 0, 0));
        Rigidbody rb = psiBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Optional: Add ammo consumption
        // currentAmmo--;
        // if (currentAmmo <= 0)
        // {
        //     canShoot = false;
        // }
    }
}