using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPlace;
    private int currentSpawnIndex;

    private void Start()
    {
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPlace.position, spawnPlace.rotation);
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPlace = newSpawnPoint;
    }
}
