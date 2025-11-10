using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private static Transform currentCheckpoint;
    [SerializeField] private Transform player;

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Call this when player passes a checkpoint
    public static void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log("Checkpoint set: " + checkpoint.name);
    }

    // Add this method to get the current checkpoint
    public static Transform GetCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnAtCheckpoint();
        }
    }

    private void RespawnAtCheckpoint()
    {
        if (currentCheckpoint == null || player == null)
        {
            Debug.LogWarning("No checkpoint set or player missing!");
            return;
        }

        player.position = currentCheckpoint.position;
        player.rotation = currentCheckpoint.rotation;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
