using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Last_Level_Objects : MonoBehaviour
{
    public static Respawn_Last_Level_Objects Instance;

    void Awake()
    {
        // Singleton pattern — ensures one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRespawn(GameObject prefab, Vector3 position, Quaternion rotation, float delay, bool randomize, float range)
    {
        StartCoroutine(RespawnCoroutine(prefab, position, rotation, delay, randomize, range));
    }

    private IEnumerator RespawnCoroutine(GameObject prefab, Vector3 position, Quaternion rotation, float delay, bool randomize, float range)
    {
        yield return new WaitForSeconds(delay);

        if (randomize)
        {
            position += new Vector3(
                Random.Range(-range, range),
                0,
                Random.Range(-range, range)
            );
        }

        Instantiate(prefab, position, rotation);
    }
}


