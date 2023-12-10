using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject sleepPanel;
    public Button sleepYesButton;
    public Button sleepNoButton;
    public SavePoint savePoint;

    private void Start()
    {
        sleepPanel.SetActive(false);
        sleepYesButton.onClick.AddListener(OnSleepYesButton);
        sleepNoButton.onClick.AddListener(OnSleepNoButton);
        StartCoroutine(UpdateSavePointCoroutine());
    }

    private IEnumerator UpdateSavePointCoroutine()
    {
        while (savePoint == null)
        {
            savePoint = FindObjectOfType<SavePoint>();
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnSleepYesButton()
    {
        GameManager.Instance.InvisibleCursor();
        StartCoroutine(savePoint.FadePanel());
        sleepPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void OnSleepNoButton()
    {
        sleepPanel.SetActive(false);
        GameManager.Instance.InvisibleCursor();
        Time.timeScale = 1.0f;
    }
}