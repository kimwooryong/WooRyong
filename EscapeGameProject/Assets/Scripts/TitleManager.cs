using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public Button endButton;

    void Start()
    {
        startButton.GetComponent<Button>().onClick.AddListener(ONStartButtonClick);
        endButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClick);
    }

    void Update()
    {
        
    }

    void ONStartButtonClick()
    {
        SceneManager.LoadScene("TestScene");
    }
    void OnExitButtonClick()
    {
        Application.Quit();
    }

}
