using UnityEngine;

public class Gun_Level_Object : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float fireRate = 10f;
    public float bulletSpeed = 20f;
    public int maxActiveBullets = 6;

    private float fireTimer = 0f;
    [HideInInspector] public int activeBulletCount = 0; // 👈 now per gun

    void Update()
    {
        if (activeBulletCount >= maxActiveBullets)
            return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = 1f / fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            Quaternion.LookRotation(transform.forward)
        );

        // Add force or velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = transform.forward * bulletSpeed;

        // Link the bullet back to this gun
        Glloo_Level_Oblecht bulletScript = bullet.GetComponent<Glloo_Level_Oblecht>();
        if (bulletScript != null)
            bulletScript.originGun = this;

        activeBulletCount++;
    }

    // Called when a bullet from this gun is destroyed
    public void NotifyBulletDestroyed()
    {
        activeBulletCount = Mathf.Max(0, activeBulletCount - 1);
    }
}
