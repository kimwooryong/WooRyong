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
    public CaveLight caveLight;

    private GameManager gameManager;

    [HideInInspector]
    public float newIntensityMultiplier = 1.0f;

    private void Awake()
    {
        if (caveLight == null)
        {
            CaveLight[] caveLights = GameObject.FindObjectsOfType<CaveLight>(true);

            if (caveLights.Length > 0)
            {
                caveLight = caveLights[0];
            }
        }

            caveLight.enabled = false;
        
    }
    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
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
            caveLight.gameObject.SetActive(true);

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
            caveLight.gameObject.SetActive(false);
        }
    }
}

