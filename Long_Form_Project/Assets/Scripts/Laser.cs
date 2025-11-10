using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 8f;
    [SerializeField] private LayerMask ignoreMask; // layers TO IGNORE
    [SerializeField] private UnityEvent OnHitTarget;

    private RaycastHit rayHit;
    private Ray ray;
    private GameRespaw lastTarget; // avoid repeatedly calling Hit() every frame

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
            lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        // Invert ignoreMask so raycast checks all layers EXCEPT ignoreMask
        int mask = ~ignoreMask;

        Vector3 endPoint = transform.position + transform.forward * laserDistance;

        // Use laserDistance and mask
        if (Physics.Raycast(ray, out rayHit, laserDistance, mask))
        {
            endPoint = rayHit.point;
            Debug.DrawLine(transform.position, endPoint, Color.red);

            Collider hitCol = rayHit.collider;
            Debug.Log($"Laser hit: {hitCol.name} (layer {hitCol.gameObject.layer})");

            // Try get component on the collider's GameObject, then parents, then children
            GameRespaw target = null;
            if (!hitCol.TryGetComponent<GameRespaw>(out target))
                target = hitCol.GetComponentInParent<GameRespaw>();
            if (target == null)
                target = hitCol.GetComponentInChildren<GameRespaw>();

            if (target != null)
            {
                // Only call once when entering the laser
                if (target != lastTarget)
                {
                    Debug.Log($"Calling Hit() on {target.gameObject.name}");
                    target.Hit();
                    OnHitTarget?.Invoke();
                    lastTarget = target;
                }
            }
            else
            {
                Debug.Log($"No GameRespaw on {hitCol.name} or its parents/children.");
                lastTarget = null;
            }
        }
        else
        {
            Debug.DrawLine(transform.position, endPoint, Color.green);
            lastTarget = null;
        }

        // update line renderer positions
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    private void OnDrawGizmos()
    {
        // guard against using ray before it's been set in editor
        Vector3 origin = (ray.origin == Vector3.zero) ? transform.position : ray.origin;
        Vector3 dir = (ray.direction == Vector3.zero) ? transform.forward : ray.direction;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, dir * laserDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rayHit.point, 0.23f);
    }
}
