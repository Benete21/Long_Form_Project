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
        player.position = respawnPoint.position;
    }
}

}
