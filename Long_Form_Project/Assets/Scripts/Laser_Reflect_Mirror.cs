using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Laser_Reflect_Mirror : MonoBehaviour
{
    Vector3 pos, dir;

    GameObject laserObj;
    public LineRenderer laser;
    public List<Vector3> laserIndices = new List<Vector3>();
    public int length;
    public OpenDoor od;
  //  public AudioSource breakGlass;

    private Shoot_Laser_Mirror shoot;

    public void Start()
    {
        od = FindObjectOfType<OpenDoor>();
        
    }
    public void InitializeLaser(Vector3 startPos, Vector3 direction, Material mat)
    {
        pos = startPos;
        dir = direction;

            laserObj = new GameObject("LaserLine");
            laser = laserObj.AddComponent<LineRenderer>();

            laser.startWidth = 0.1f;
            laser.endWidth = 0.1f;
            laser.material = mat;
            laser.startColor = Color.green;
            laser.endColor = Color.green;

        CastRay(pos, dir, laser);
    }


    public void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser, int depth = 0)
    {
        if (depth == 0)
            laserIndices.Clear();

        if (depth > 10)
        {
            laserIndices.Add(pos + dir * 10f);
            UpdateLaser();
            return;
        }

        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            laserIndices.Add(hit.point);

            if (hit.collider.CompareTag("Mirror"))
            {
                Vector3 reflectDir = Vector3.Reflect(dir, hit.normal);
                Vector3 reflectOrigin = hit.point + reflectDir * 0.01f;

                CastRay(reflectOrigin, reflectDir, laser, depth + 1);
            }
            else if (hit.collider.CompareTag("Mirror_Move"))
            {
                Vector3 reflectDir = Vector3.Reflect(dir, hit.normal);
                Vector3 reflectOrigin = hit.point + reflectDir * 0.01f;

                CastRay(reflectOrigin, reflectDir, laser, depth + 1);
            }
            else if (hit.collider.CompareTag("LaserButton"))
            {
                Destroy(hit.collider.gameObject);
                shoot.breakGlass.Play();
            }
            else if (hit.collider.CompareTag("Melt"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            laserIndices.Add(pos + dir * 70f);
        }

        // Moved here: always update after cast
        UpdateLaser();
    }




    void UpdateLaser()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach(Vector3 idx in laserIndices)
        {
            laser.SetPosition(count, idx);
            count++;
        }
    }
}
