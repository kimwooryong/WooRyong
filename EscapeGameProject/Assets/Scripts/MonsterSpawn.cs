using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefab;
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
                int randomPrefabCount = Random.Range(0, monsterPrefab.Length);
                Instantiate(monsterPrefab[randomPrefabCount], randomPosition, Quaternion.identity);
                nextSpawnTime = Time.time + spawnInterval;
            }
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;

        for (int attempts = 0; attempts < 10; attempts++)
        {
            randomPosition = Random.insideUnitSphere * maxDistance + transform.position;
            randomPosition.y = GetTerrainHeight(randomPosition);

            if (!Physics.CheckSphere(randomPosition, 1f, obstacleLayer) && Vector3.Distance(randomPosition, transform.position) >= minDistance)
            {
                return randomPosition;
            }
        }

        return Vector3.zero;
    }

    float GetTerrainHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(position.x, 100f, position.z), Vector3.down, out hit, 200f, spawnLayer))
        {
            return hit.point.y;
        }
        return 0f;
    }
}