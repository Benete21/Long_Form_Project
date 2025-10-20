using UnityEngine;

public class Gun_Level_Object : MonoBehaviour
{
    public GameObject bulletPrefab;       // Your bullet prefab
    public Transform muzzlePoint;         // Where the bullet spawns
    public float fireRate = 10f;          // Bullets per second
    public float bulletSpeed = 20f;       // Speed of the bullet

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = 1f / fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.LookRotation(transform.forward));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
        }

        Destroy(bullet, 3f); // Clean up after 3 seconds
    }
}
