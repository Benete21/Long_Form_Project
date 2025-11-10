using System.Collections;
using UnityEngine;

public class GameRespaw : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawing();
        }
    }

    public void PlayerRespawing()
    {
        if (player == null || respawnPoint == null)
        {
            Debug.LogWarning("Player or respawn point not assigned!");
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

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
