using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{

    public Vector3 playerPosition;
    public Vector3 playerRotation;

    public bool isCaveEnter = false;

    private DayNightCycle dayNightCycle;
    private Quaternion savedRotation;
    private float savedFog;

    private MonsterSpawner monsterSpawner;

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            EnterCave();
            collision.transform.position = playerPosition;
            collision.transform.eulerAngles = playerRotation;


        }
    }
    void EnterCave()
    {
        isCaveEnter = !isCaveEnter;
        if (isCaveEnter)
        {

            dayNightCycle.gameObject.SetActive(false);
            monsterSpawner.enabled = false;


        }
        else
        {
            dayNightCycle.gameObject.SetActive(true);
            monsterSpawner.enabled = true;
        }
    }
}

