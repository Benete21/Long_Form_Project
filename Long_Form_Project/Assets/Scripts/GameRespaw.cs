using System.Collections;
using UnityEngine;

public class GameRespaw : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint; // Each zone assigns its own respawn point

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawning();
        }
    }

    public void PlayerRespawning()
    {
        if (player == null || respawnPoint == null)
        {
            Debug.LogWarning("Player or respawn point not assigned on " + gameObject.name);
            return;
        }

        RespawnPlayer();
    }

    public void Hit()
    {
        if (player == null || respawnPoint == null) return;
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        player.position = respawnPoint.position;
        player.rotation = respawnPoint.rotation; // Also match rotation

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}