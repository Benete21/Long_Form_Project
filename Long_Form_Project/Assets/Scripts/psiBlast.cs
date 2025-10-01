using UnityEngine;
using UnityEngine.UI;

public class PsiBlist : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject psiBlastPrefab;
    public Transform shootPoint;
    public Text ammoStatusText;

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float cooldownTime = 0.5f;
    public int maxAmmo = 10;

    private bool canShoot = false;
    private int currentAmmo;
    private float lastShootTime;

    private void Start()
    {
        canShoot = false;
        currentAmmo = maxAmmo;
        lastShootTime = -cooldownTime;
      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            canShoot = true;
            
        }

        else if  (Input.GetKeyDown(KeyCode.Alpha1))

            {
                canShoot = false;
            }


        if (Input.GetMouseButtonDown(1) && canShoot && Time.time >= lastShootTime )
        {
            Shoot();
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


        GameObject psiBullet = Instantiate(psiBlastPrefab, shootPoint.position, Quaternion.Euler(90,0,0));

       
        Rigidbody rb = psiBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
      

       
    } 
}