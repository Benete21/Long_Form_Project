using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Reflect_Mirror : MonoBehaviour
{
    Vector3 pos, dir;

    GameObject laserObj;
    public LineRenderer laser;
    public List<Vector3> laserIndices = new List<Vector3>();
    public int length;
    public OpenDoor od;

    public void Start()
    {
        laser = GetComponent<LineRenderer>();
        od = FindObjectOfType<OpenDoor>();
    }
    public void InitializeLaser(Vector3 startPos, Vector3 direction, Material mat)
    {
        pos = startPos;
        dir = direction;

        if (laser == null)
        {
            laserObj = new GameObject("LaserLine");
            laser = laserObj.AddComponent<LineRenderer>();

            laser.startWidth = 0.1f;
            laser.endWidth = 0.1f;
            laser.material = mat;
            laser.startColor = Color.green;
            laser.endColor = Color.green;
        }

        CastRay(pos, dir, laser);
    }


    public void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser, int depth = 0)
    {
        if (depth == 0)
            laserIndices.Clear();

        if (depth > 10) // Avoid infinite recursion
        {
            laserIndices.Add(pos + dir * 10f);
            UpdateLaser();
            return;
        }

        laserIndices.Add(pos); // Add the current ray start position

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            laserIndices.Add(hit.point); // Add hit point

            if (hit.collider.CompareTag("Mirror"))
            {
                Vector3 reflectDir = Vector3.Reflect(dir, hit.normal);
                Vector3 reflectOrigin = hit.point + reflectDir * 0.01f; // small offset

                CastRay(reflectOrigin, reflectDir, laser, depth + 1);
            }
            else if (hit.collider.CompareTag("LaserButton"))
            {
                var door = hit.collider.GetComponent<OpenDoor>();
                if (door != null)
                {
                    door.OpenDoorAnimator();
                }

                UpdateLaser();
            }
            else
            {
                // Hit something else
                UpdateLaser();
            }
        }
        else
        {
            // Hit nothing
            laserIndices.Add(pos + dir * 70f);
            UpdateLaser();
        }
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
