using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool canInteract = false;
    private GameManager gameManager;
    private Transform firstSpawnPosition;
    private NaturalObjectSpawn naturalObjectSpawn;
    private DayNightCycle dayNightCycle;
    private UIManager uiManager;
    private PlayerCotroller playerCotroller;

    public CanvasGroup panelCanvasGroup;

    public float fadeDuration = 5f;
    private int randCount;

    private void Awake()
    {
        panelCanvasGroup.gameObject.SetActive(false);
    }

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        gameManager = FindObjectOfType<GameManager>();
        naturalObjectSpawn = FindObjectOfType<NaturalObjectSpawn>();
        uiManager = FindObjectOfType<UIManager>();
        playerCotroller = FindObjectOfType<PlayerCotroller>();

        if (gameManager != null)
        {
            firstSpawnPosition = gameManager.spawnPlace.transform;
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E) && dayNightCycle.isNight == true)
        {
            Time.timeScale = 0.0f;
            uiManager.sleepPanel.SetActive(true);
            GameManager.Instance.VisibleCursor();

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
        gameManager.SetSpawnPoint(transform);
    }

    private void OnDestroy()
    {
        gameManager.SetSpawnPoint(firstSpawnPosition);      
    }

    public IEnumerator FadePanel()
    {

        {
            panelCanvasGroup.gameObject.SetActive(true);
            float timer = 0f;
            while (timer < fadeDuration)
            {

                panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                timer += Time.deltaTime;

                //playerMovement.onMove = false; 나중에 플레이어 들어오면 불값 줘서 움직임 막을 수 있게 하기
                playerCotroller.speed = 0.0f;
                playerCotroller.runSpeed = 0.0f;
                playerCotroller.jumpForce = 0.0f;
                yield return null;
            }
            SetSavePoint();
            dayNightCycle.ResetDayNightCycle();
            panelCanvasGroup.alpha = 0f;
            playerCotroller.speed = 10.0f;
            playerCotroller.runSpeed = 15.0f;
            playerCotroller.jumpForce = 200.0f;
            panelCanvasGroup.gameObject.SetActive(false);
        }
    }
}