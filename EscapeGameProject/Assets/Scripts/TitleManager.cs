using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button endButton;
    public GameObject OptionPanel;
    void Start()
    {
        startButton.GetComponent<Button>().onClick.AddListener(ONStartButtonClick);
        optionButton.GetComponent<Button>().onClick.AddListener(OnOptionButtonClick);
        endButton.GetComponent<Button>().onClick.AddListener(OnEndButtonClick);


        OptionPanel.SetActive(false);
    }

    void Update()
    {
        
    }

    void ONStartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
    void OnOptionButtonClick()
    {
        OptionPanel.SetActive(true);
    }
    void OnEndButtonClick()
    {
        Application.Quit();
    }
}
