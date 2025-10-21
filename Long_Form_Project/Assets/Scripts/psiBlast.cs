using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PsiBlist : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject psiBlastPrefab;
    public Transform shootPoint;
    public Text ammoStatusText;
    public RawImage psiblast;

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float cooldownTime = 0.1f;
    public int maxAmmo = 10;

    private bool canShoot = false;
    private int currentAmmo;
    private float lastShootTime;
    
    #region Input System
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
        controls.Player.ActivatePsi.performed += OnActivatePsiBlast;
        controls.Player.ActivatePyro.performed += OnDeactivatePsiBlast;
        controls.Player.AltFire.performed += OnAltFire;
        controls.Player.Look.performed += OnAim;
    }

    void OnDisable()
    {
        // Unsubscribe from input events
        controls.Player.ActivatePsi.performed -= OnActivatePsiBlast;
        controls.Player.ActivatePyro.performed -= OnDeactivatePsiBlast;
        controls.Player.AltFire.performed -= OnAltFire;
        controls.Player.Look.performed -= OnAim;
        
        controls.Disable();
    }

    private void Start()
    {
        canShoot = false;
        currentAmmo = maxAmmo;
        lastShootTime = -cooldownTime;
        
        if (psiblast != null)
        {
            psiblast.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Detect input device (mouse vs gamepad)
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
        {
            isUsingMouse = false;
        }
        else if (Mouse.current != null && Mouse.current.delta.ReadValue().magnitude > 0.1f)
        {
            isUsingMouse = true;
        }
    }

    // Input callbacks
    void OnActivatePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = true;
        if (psiblast != null)
        {
            psiblast.gameObject.SetActive(true);
        }
    }

    void OnDeactivatePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = false;
        if (psiblast != null)
        {
            psiblast.gameObject.SetActive(false);
        }
    }

    void OnAltFire(InputAction.CallbackContext context)
    {
        if (canShoot && Time.time >= lastShootTime)
        {
            Shoot();
            if (psiblast != null)
            {
                psiblast.gameObject.SetActive(false);
            }
        }
    }

    void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }

    void Shoot()
    {
        Ray ray;
        
        if (isUsingMouse)
        {
            // Mouse aiming from viewport center
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
        else
        {
            // Controller aiming (center + right stick)
            Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
            Vector3 aimOffset = new Vector3(aimInput.x * 0.1f, aimInput.y * 0.1f, 0); // Adjust sensitivity
            ray = playerCamera.ViewportPointToRay(viewportCenter + aimOffset);
        }

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

        lastShootTime = Time.time + cooldownTime;
    }
}