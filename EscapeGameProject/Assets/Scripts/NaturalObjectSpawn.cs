using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FaunaAndFlora
{
    Animal,
    Plant
}

public class NaturalObjectSpawn : MonoBehaviour
{
    public GameObject[] naturalObject;
    public int minTrees = 20;
    public int maxTrees = 30;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public LayerMask spawnLayer;
    public LayerMask obstacleLayer;

    private int maxSpawnObject = 1000;

    private GameObject emptyObject;

    [HideInInspector]
    public float randomSize;

    public FaunaAndFlora faunaAndFlora;

    private void Start()
    {
        emptyObject = new GameObject("TreeObject");

        randomRotation = Quaternion.identity;

        for (int i = 0; i < 5; i++)
        {
            if(faunaAndFlora == FaunaAndFlora.Plant)
            {
            SpawnTrees();
            }
            else if(faunaAndFlora == FaunaAndFlora.Animal) 
            {
                SpawnAnimal();
            }
        }
    }

    private void Update()
    {
        
    }

    private Quaternion randomRotation;

    public void SpawnTrees()
    {
        int currentSpawnObject = GameObject.FindGameObjectsWithTag("NaturalObject").Length;

        int remainingObjects = Mathf.Max(0, maxSpawnObject - currentSpawnObject);

        int numberOfObjects = Mathf.Min(Random.Range(minTrees, maxTrees + 1), remainingObjects);

        int randomPrefabCount = Random.Range(0, naturalObject.Length);

        for (int i = 0; i < numberOfObjects; i++)
        {
            randomRotation.y = Random.Range(0f, 4f);

            Vector3 randomPosition = GenerateRandomSpawnPosition();
            GameObject treeInstance = Instantiate(naturalObject[randomPrefabCount], randomPosition, randomRotation);
            randomSize = Random.Range(0.9f, 1.1f);
            treeInstance.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            treeInstance.transform.parent = emptyObject.transform;
        }
    }

private void SpawnAnimal()
{
    int currentSpawnObject = GameObject.FindGameObjectsWithTag("NaturalObject").Length;
    int remainingObjects = Mathf.Max(0, maxSpawnObject - currentSpawnObject);
    int numberOfObjects = Mathf.Min(Random.Range(minTrees, maxTrees + 1), remainingObjects);

    float[] prefabProbabilities = { 0.35f, 0.25f, 0.15f, 0.15f, 0.1f };

    for (int i = 0; i < numberOfObjects; i++)
    {
        int prefabIndex = WeightedRandomSelection(prefabProbabilities);

        randomRotation.y = Random.Range(0f, 4f);
        Vector3 randomPosition = GenerateRandomSpawnPosition();


            GameObject treeInstance = Instantiate(naturalObject[prefabIndex], randomPosition, randomRotation);

            if(prefabIndex == 3)
            { 
                float wolfSize = 2.0f;
                treeInstance.transform.localScale = new Vector3(wolfSize, wolfSize, wolfSize);
                treeInstance.transform.parent = emptyObject.transform;
            }
            else
            {
                randomSize = Random.Range(0.9f, 1.1f);
                treeInstance.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                treeInstance.transform.parent = emptyObject.transform;
            }

    }
}

private int WeightedRandomSelection(float[] probabilities)
{
    float totalWeight = 0f;

    foreach (float probability in probabilities)
    {
        totalWeight += probability;
    }

    float randomValue = Random.Range(0f, totalWeight);

    for (int i = 0; i < probabilities.Length; i++)
    {
        if (randomValue <= probabilities[i])
        {
            return i;
        }

        randomValue -= probabilities[i];
    }
    return probabilities.Length - 1;
}


    Vector3 GenerateRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;

        while (true)
        {
            randomPosition = Random.insideUnitSphere * Random.Range(minDistance, maxDistance) + transform.position;
            randomPosition.y = GetTerrainHeight(randomPosition);

            if (!Physics.CheckSphere(randomPosition, 1f, obstacleLayer) && Vector3.Distance(randomPosition, transform.position) >= minDistance)
            {
                return randomPosition;
            }
        }
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
