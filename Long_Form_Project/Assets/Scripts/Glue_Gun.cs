using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Glue_Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject Gloo_Bullet_Prefab;
    public Transform shoot_Point_Gloo;
    public float shootForce = 20f;
    public float fireRate = 0.2f;

    [Header("Ammo Settings")]
    public float Reload = 10f;
    public int maxShots = 5;
    private int shotsRemaining;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    [Header("UI")]
    public Slider Gloo_Bar;

    [Header("Audio")]
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        shotsRemaining = maxShots;

        if (Gloo_Bar != null)
        {
            Gloo_Bar.maxValue = maxShots;
            Gloo_Bar.value = shotsRemaining;
        }
    }

    void Update()
    {
        // Shooting
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && shotsRemaining > 0 && !isReloading)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
            UpdateAmmoUI();
        }

        // Instant reload when pressing R
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && shotsRemaining < maxShots)
        {
            InstantReload();
        }
    }



    void Shoot()
    {
        GameObject Gloo_Bullet = Instantiate(Gloo_Bullet_Prefab, shoot_Point_Gloo.position, shoot_Point_Gloo.rotation);
        Rigidbody rb = Gloo_Bullet.GetComponent<Rigidbody>();
        rb.AddForce(shoot_Point_Gloo.forward * shootForce, ForceMode.Impulse);

        shotsRemaining--;
        Debug.Log("Shots left: " + shotsRemaining);

        audioManager.PlaySFX(audioManager.shootGloo);

        if (shotsRemaining <= 0)
        {
            StartCoroutine(AutoRefill());
        }
    }

    IEnumerator AutoRefill()
    {
        isReloading = true;
        float elapsed = 0f;

        while (elapsed < Reload)
        {
            elapsed += Time.deltaTime;
            if (Gloo_Bar != null)
                Gloo_Bar.value = Mathf.Lerp(0, maxShots, elapsed / Reload);
            yield return null;
        }

        shotsRemaining = maxShots;
        isReloading = false;
        UpdateAmmoUI();
        Debug.Log("Glue gun refilled!");
    }

    void UpdateAmmoUI()
    {
        if (Gloo_Bar != null)
        {
            Gloo_Bar.value = shotsRemaining;
        }
    }
    void InstantReload()
    {
        isReloading = true;
        shotsRemaining = maxShots;
        UpdateAmmoUI();
        Debug.Log("Instant reload complete!");
        isReloading = false;

    }

}
