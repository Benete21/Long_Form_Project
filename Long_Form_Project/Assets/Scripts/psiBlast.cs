using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class PsiBlast : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject psiBlastPrefab;
    public Transform shootPoint;
    public Text ammoStatusText;
    public RawImage psiblast;
    public RawImage pyro;

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float fireRate = 0.2f;
    public int maxAmmo = 10;
    public float reloadDelay = 3f;
    public Slider Ammofill;
    private int shotsRemaining;

    [Header("Input")]
    public InputActionReference activatePsi;
    public InputActionReference altFire;

    private bool canShoot = false;
    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    [Header("Audio")]
    AudioManager audioManager;

    private void Start()
    {
        canShoot = false;
        currentAmmo = maxAmmo;
        shotsRemaining = currentAmmo;

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

        UpdateAmmoUI();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnDestroy()
    {
        if (activatePsi != null)
        {
            activatePsi.action.performed -= OnTogglePsiBlast;
        }
    }

    private void OnTogglePsiBlast(InputAction.CallbackContext context)
    {
        canShoot = !canShoot;
        
        if (psiblast != null)
            psiblast.gameObject.SetActive(canShoot);
    
        if (pyro != null)
            pyro.gameObject.SetActive(!canShoot);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            canShoot = true;
            if (psiblast != null)
                psiblast.gameObject.SetActive(true);
            if (pyro != null)
                pyro.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            canShoot = false;
            if (psiblast != null)
                psiblast.gameObject.SetActive(false);
            if (pyro != null)
                pyro.gameObject.SetActive(true);
        }

        if (canShoot && Time.time >= nextFireTime && !isReloading)
        {
            bool isHoldingShoot = false;

            if (altFire != null && altFire.action.ReadValue<float>() > 0.1f)
                isHoldingShoot = true;

            if (Input.GetMouseButton(1))
                isHoldingShoot = true;

            if (isHoldingShoot)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }


    }

    void Shoot()
    {
        if (currentAmmo <= 0 || isReloading) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit, 100f) ? hit.point : ray.GetPoint(100f);

        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        GameObject psiBullet = Instantiate(psiBlastPrefab, shootPoint.position, Quaternion.Euler(90, 0, 0));
        Rigidbody rb = psiBullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.velocity = direction * projectileSpeed;

        currentAmmo--;
        shotsRemaining--;
        UpdateAmmoUI();

        audioManager.PlaySFX(audioManager.psiBulletCollision);

        if (currentAmmo <= 0)
        {
            canShoot = false;
            StartCoroutine(AutoReload());
        }
    }

    IEnumerator AutoReload()
    {
        isReloading = true;
        if (ammoStatusText != null)
            ammoStatusText.text = "Reloading...";

        yield return new WaitForSeconds(reloadDelay);

        currentAmmo = maxAmmo;
        isReloading = false;
        canShoot = true;
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        currentAmmo = shotsRemaining;

        if (ammoStatusText != null)
            ammoStatusText.text = $"Ammo: {currentAmmo}/{maxAmmo}";

        if (Ammofill != null)
            Ammofill.value = currentAmmo;
    }



}
