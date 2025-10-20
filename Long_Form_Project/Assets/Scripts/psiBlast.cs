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

    [Header("Settings")]
    public float projectileSpeed = 20f;
    public float cooldownTime = 0.5f;
    public int maxAmmo = 10;

    private bool canShoot = false;
    private int currentAmmo;
    private float lastShootTime;

    public RawImage psiblast;
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
            psiblast.gameObject.SetActive(true);
            
        }

        else if  (Input.GetKeyDown(KeyCode.Alpha1))

            {
                canShoot = false;
            psiblast.gameObject.SetActive(false);
            }


        if (Input.GetMouseButtonDown(1) && canShoot && Time.time >= lastShootTime )
        {
            Shoot();
            psiblast.gameObject.SetActive(false);
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