using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRespaw : MonoBehaviour
{ 
    [SerializeField] private Transform player;

[SerializeField] private Transform respawnPoint;


private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
            PlayerRespawing();
    }
}
    public void PlayerRespawing()
    {
        player.position = respawnPoint.position;
    }
    public void Hit()
    {
        player.position = respawnPoint.position;
    }
}
