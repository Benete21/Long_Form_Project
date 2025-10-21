using UnityEngine;

public class Gun_Level_Object : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float fireRate = 10f;
    public float bulletSpeed = 20f;
    public int maxActiveBullets = 6;

    private float fireTimer = 0f;
    public static int activeBulletCount = 0; // 🔢 Track active bullets globally

    void Update()
    {
        // Only fire if we have room for more bullets
        if (activeBulletCount >= maxActiveBullets) return;

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

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
        }

        // Count this bullet
        activeBulletCount++;
    }
}
