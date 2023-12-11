using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    private GameManager gameManager;
    private Transform firstSpawnPosition;
    private PlayerCotroller playerCotroller;
    private PlayerStatus player;

    private bool isFading = false;

    public float fadeDuration = 5f;
    public CanvasGroup panelCanvasGroup;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerCotroller = FindObjectOfType<PlayerCotroller>();
        player = FindObjectOfType<PlayerStatus>();
        if (gameManager != null)
        {
            firstSpawnPosition = gameManager.spawnPlace.transform;
        }
    }

    void Update()
    {
        if (player == null && playerCotroller == null)
        {
            player = FindObjectOfType<PlayerStatus>();
        }

        if (!isFading && player != null && player.playerCurrentHp <= 0)
        {
            StartCoroutine(FadePanel());
        }
    }

    public IEnumerator FadePanel()
    {
        isFading = true;

        panelCanvasGroup.gameObject.SetActive(true);
        float timer = 0f;
        player.rb.isKinematic = true;
        while (timer < fadeDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        player.gameObject.SetActive(false);
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.gameObject.SetActive(false);

        isFading = false;
        
    }
}