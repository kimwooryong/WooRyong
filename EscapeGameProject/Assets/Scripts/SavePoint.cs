using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool canInteract = false;
    private GameManager gameManager;
    private Transform firstSpawnPosition;
    private DayNightCycle dayNightCycle;
    private UIManager uiManager;
    private PlayerStatus player;

    public CanvasGroup panelCanvasGroup;

    public float fadeDuration = 5f;

    private void Awake()
    {
        panelCanvasGroup.gameObject.SetActive(false);
    }

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<PlayerStatus>();

        if (gameManager != null)
        {
            firstSpawnPosition = gameManager.spawnPlace.transform;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerStatus>();
        }
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
            player.rb.isKinematic = true;
            panelCanvasGroup.gameObject.SetActive(true);
            float timer = 0f;
            while (timer < fadeDuration)
            {

                panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                timer += Time.deltaTime;

                //playerMovement.onMove = false; 나중에 플레이어 들어오면 불값 줘서 움직임 막을 수 있게 하기
                yield return null;
            }
            SetSavePoint();
            player.rb.isKinematic = false;
            dayNightCycle.ResetDayNightCycle();
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.gameObject.SetActive(false);
        }
    }
}