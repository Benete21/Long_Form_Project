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
    public RawImage psiblast;
    public Slider ammoBar; // 🔹 Added: UI bar to show ammo visually

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float cooldownTime = 0.1f;
    public int maxAmmo = 10;
    public float reloadTime = 5f;

    private bool canShoot = false;
    private bool isReloading = false;
    private int currentAmmo;
    private float lastShootTime;

    #region Input System
    private PlayerControls controls;
    private bool isUsingMouse = true;
    private Vector2 aimInput;
    #endregion

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();

        controls.Player.ActivatePsi.performed += OnActivatePsiBlast;
        controls.Player.ActivatePyro.performed += OnDeactivatePsiBlast;
        controls.Player.AltFire.performed += OnAltFire;
        controls.Player.Look.performed += OnAim;
    }

    void OnDisable()
    {
        controls.Player.ActivatePsi.performed -= OnActivatePsiBlast;
        controls.Player.ActivatePyro.performed -= OnDeactivatePsiBlast;
        controls.Player.AltFire.performed -= OnAltFire;
        controls.Player.Look.performed -= OnAim;

        controls.Disable();
    }

    void Start()
    {
        canShoot = false;
        isReloading = false;
        currentAmmo = maxAmmo;
        lastShootTime = -cooldownTime;

        if (psiblast != null)
            psiblast.gameObject.SetActive(false);

        // 🔹 Initialize ammo bar
        if (ammoBar != null)
        {
            ammoBar.maxValue = maxAmmo;
            ammoBar.value = currentAmmo;
        }

        UpdateAmmoUI();
    }

    void Update()
    {
        // Detect input device
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
            isUsingMouse = false;
        else if (Mouse.current != null && Mouse.current.delta.ReadValue().magnitude > 0.1f)
            isUsingMouse = true;
    }

    // Input callbacks
    void OnActivatePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = true;
        if (psiblast != null)
            psiblast.gameObject.SetActive(true);
    }

    void OnDeactivatePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = false;
        if (psiblast != null)
            psiblast.gameObject.SetActive(false);
    }

    void OnAltFire(InputAction.CallbackContext context)
    {
        if (!canShoot || isReloading)
            return;

        if (Time.time >= lastShootTime && currentAmmo > 0)
        {
            Shoot();
            if (psiblast != null)
                psiblast.gameObject.SetActive(false);
        }

        // Start reload if out of ammo
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
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
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        else
        {
            Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
            Vector3 aimOffset = new Vector3(aimInput.x * 0.1f, aimInput.y * 0.1f, 0);
            ray = playerCamera.ViewportPointToRay(viewportCenter + aimOffset);
        }

        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f)
            ? hit.point
            : ray.GetPoint(100f);

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        GameObject psiBullet = Instantiate(psiBlastPrefab, shootPoint.position, Quaternion.Euler(90, 0, 0));
        Rigidbody rb = psiBullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.velocity = direction * projectileSpeed;

        lastShootTime = Time.time + cooldownTime;

       
        currentAmmo--;
        UpdateAmmoUI();

       
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (ammoStatusText != null)
            ammoStatusText.text = "Reloading...";

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoStatusText != null)
            ammoStatusText.text = $"Ammo: {currentAmmo}/{maxAmmo}";

      
        if (ammoBar != null)
            ammoBar.value = currentAmmo;
    }
}
