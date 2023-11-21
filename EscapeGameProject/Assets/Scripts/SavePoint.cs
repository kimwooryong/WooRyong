using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool canInteract = false;
    private PlayerSpawner spawnPlayer;
    private Vector3 firstSpawnPosition;
    private Quaternion firstSpawnRotation;
    private TreeSpawn treeSpawner;
    private DayNightCycle dayNightCycle;
    private UIManager uiManager;
    private PlayerMovement playerMovement;

    public CanvasGroup panelCanvasGroup;

    public float fadeDuration = 5f;


    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        spawnPlayer = FindObjectOfType<PlayerSpawner>();
        treeSpawner = FindObjectOfType<TreeSpawn>();
        uiManager = FindObjectOfType<UIManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (spawnPlayer != null)
        {
            firstSpawnPosition = spawnPlayer.transform.position;
            firstSpawnRotation = spawnPlayer.transform.rotation;
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E) && dayNightCycle.isNight == true)
        {
            Time.timeScale = 0.0f;
            uiManager.sleepPanel.SetActive(true);



        }
        else if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            SetSavePoint();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void SetSavePoint()
    {
        if (spawnPlayer != null)
        {
            spawnPlayer.spawnPlace.position = transform.position;
            spawnPlayer.spawnPlace.rotation = transform.rotation;
        }
    }

    private void OnDestroy()
    {
        if (spawnPlayer != null)
        {
            spawnPlayer.spawnPlace.position = firstSpawnPosition;
            spawnPlayer.spawnPlace.rotation = firstSpawnRotation;
        }
    }

    public IEnumerator FadePanel()
    {
        
        {
        float timer = 0f;
        while (timer < fadeDuration)
        {

            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
                playerMovement.onMove = false;
                    yield return null;
        }
        SetSavePoint();
        treeSpawner.SpawnTrees();
        dayNightCycle.ResetDayNightCycle();
        panelCanvasGroup.alpha = 0f;
            playerMovement.onMove = true;
        }
    }
}

