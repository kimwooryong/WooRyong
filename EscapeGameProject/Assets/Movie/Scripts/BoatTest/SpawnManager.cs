using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public string spawnName;
    public GameObject obstaclePrefab;
    public Vector3 spawnPos;
    public float startDelay;
    public float repeatRate;
    public bool isChangeWave;
}

public class SpawnManager : MonoBehaviour
{
    public SpawnData[] spawnDataArray;
    private float[] spawnStartTime;

    private void Start()
    {
        spawnStartTime = new float[spawnDataArray.Length];

        for (int i = 0; i < spawnDataArray.Length; i++)
        {
            spawnStartTime[i] = Time.time + spawnDataArray[i].startDelay;
        }
    }

    private void Update()
    {
        for (int i = 0; i < spawnDataArray.Length; i++)
        {
            if (Time.time > spawnStartTime[i])
            {
                SpawnObstacle(i);
                spawnStartTime[i] = Time.time + (spawnDataArray[i].repeatRate > 0 ? spawnDataArray[i].repeatRate : Mathf.Infinity);
            }
        }
    }

    private void SpawnObstacle(int index)
    {
        SpawnData spawnData = spawnDataArray[index];
        Instantiate(spawnData.obstaclePrefab, spawnData.spawnPos, Quaternion.identity);
    }
}
