using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    public GameObject[] treePrefab;
    public int minTrees = 20;
    public int maxTrees = 30;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public LayerMask spawnLayer;
    public LayerMask obstacleLayer;

    private int maxSpawnObject = 1000;

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            SpawnTrees();
        }
    }
    
    public void SpawnTrees()
    {
        
        int currentSpawnObject = GameObject.FindGameObjectsWithTag("NaturalObject").Length;

        int remainingObjects = Mathf.Max(0, maxSpawnObject - currentSpawnObject);

        int numberOfObjects = Mathf.Min(Random.Range(minTrees, maxTrees + 1), remainingObjects);

        int randomPrefabCount = Random.Range(0, treePrefab.Length);

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition = GenerateRandomSpawnPosition();
            Instantiate(treePrefab[randomPrefabCount], randomPosition, Quaternion.identity);
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
