using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glue_Gun : MonoBehaviour
{
    public GameObject Gloo_Bullet_Prefab;
    public Transform shoot_Point_Gloo;
    public float shootForce;

    public float Reload;
    public int maxShots;
    private int shotsRemaining;
    private bool isReloading = false;

    public Slider Gloo_Bar;

    AudioManager audioManager;
    void Start()
    {
        shotsRemaining = maxShots;
        UpdateAmmoUI();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
            if (Input.GetButtonDown("Fire1") && shotsRemaining > 0 && !isReloading)
            {
                Shoot();
                UpdateAmmoUI();
            }
    }

    void Shoot()
    {
        GameObject Gloo_Bullet = Instantiate(Gloo_Bullet_Prefab, shoot_Point_Gloo.position, Quaternion.identity);
        Gloo_Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);

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
        Debug.Log("Out of shots! Refilling in 10 seconds...");
        yield return new WaitForSeconds(Reload);
        shotsRemaining = maxShots;
        isReloading = false;
        Debug.Log("Glue gun refilled!");
        UpdateAmmoUI();
    }
    public void UpdateAmmoUI()
    {
        if(Gloo_Bar != null)
        {
            Gloo_Bar.value = shotsRemaining;
        }
    }
}
