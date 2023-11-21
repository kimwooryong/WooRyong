using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject sleepPanel;
    public Button sleepYesButton;
    public Button sleepNoButton;
    private SavePoint savePoint;


    private void Start()
    {
        sleepPanel.SetActive(false);
        savePoint = FindObjectOfType<SavePoint>();
        sleepYesButton.onClick.AddListener(OnSleepYesButton);
        sleepNoButton.onClick.AddListener(OnSleepNoButton);

    }
    private void OnSleepYesButton()
    {
        StartCoroutine(savePoint.FadePanel());
        sleepPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void OnSleepNoButton()
    {
        sleepPanel.SetActive(false);
    }



}
