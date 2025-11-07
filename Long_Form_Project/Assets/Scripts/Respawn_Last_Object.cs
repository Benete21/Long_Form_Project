using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Last_Object : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 5f;       // How long to wait before respawning
    public bool destroyAfterRespawn = false; // Should this script destroy itself after respawning?
    public bool randomizeSpawn = false;   // Optionally randomize spawn position
    public float randomRange = 3f;        // Random range if enabled

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private GameObject prefabReference;

    void Start()
    {
        // Store the original spawn position and prefab reference
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        prefabReference = gameObject;
    }

    void OnDestroy()
    {
        // Prevent this from running when exiting play mode
        if (!gameObject.scene.isLoaded) return;

        // Start the respawn coroutine
        Respawn_Last_Level_Objects.Instance.StartRespawn(prefabReference, spawnPosition, spawnRotation, respawnDelay, randomizeSpawn, randomRange);
    }
}
