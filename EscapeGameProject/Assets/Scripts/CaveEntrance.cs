using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{

    public Vector3 playerPosition;
    public Vector3 playerRotation;

    public GameObject invisibleWall;

    private DayNightCycle dayNightCycle;
    private Quaternion savedRotation;
    private float savedFog;

    private MonsterSpawner monsterSpawner;

    private GameManager gameManager;

    [HideInInspector]
    public float newIntensityMultiplier = 1.0f;

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        gameManager = FindAnyObjectByType<GameManager>();
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
        gameManager.isCaveEnter = !gameManager.isCaveEnter;
        if (gameManager.isCaveEnter)
        {
            newIntensityMultiplier = 0.0f;
            RenderSettings.ambientIntensity *= newIntensityMultiplier;

            dayNightCycle.gameObject.SetActive(false);
            monsterSpawner.enabled = false;

            if(invisibleWall != null)
            {
                Destroy(invisibleWall.gameObject);
            }

        }
        else
        {
            newIntensityMultiplier = 1.0f;
            RenderSettings.reflectionIntensity *= newIntensityMultiplier;
            dayNightCycle.gameObject.SetActive(true);
            monsterSpawner.enabled = true;
        }
    }
}

