using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public LayerMask spawnLayer;
    public LayerMask obstacleLayer;
    public float spawnInterval = 10f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Vector3 randomPosition = GenerateRandomSpawnPosition();
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomPosition.x, 100f, randomPosition.z), Vector3.down, out hit, 200f, spawnLayer))
            {

                randomPosition = hit.point;
            }

            if (!Physics.CheckSphere(randomPosition, 1f, obstacleLayer))
            {
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
                nextSpawnTime = Time.time + spawnInterval;
            }
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = Random.insideUnitSphere * maxDistance + transform.position;
        } while (Vector3.Distance(randomPosition, transform.position) < minDistance);

        return randomPosition;
    }
}