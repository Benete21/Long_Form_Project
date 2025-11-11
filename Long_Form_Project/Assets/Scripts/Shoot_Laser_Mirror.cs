using UnityEngine;

public class Shoot_Laser_Mirror : MonoBehaviour
{
    [Header("Laser Settings")]
    public Material material;
    public AudioSource breakGlass;

    private Laser_Reflect_Mirror beam;

    private void Start()
    {
        beam = GetComponent<Laser_Reflect_Mirror>();
        if (beam == null)
            beam = gameObject.AddComponent<Laser_Reflect_Mirror>();

        beam.InitializeLaser(transform.position, transform.right, material);
    }

    private void Update()
    {
        if (beam?.laser == null) return;

        beam.laser.positionCount = 0;
        beam.laserIndices.Clear();

        beam.CastRay(transform.position, transform.right, beam.laser);
    }
}

