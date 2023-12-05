using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button endButton;
    public GameObject OptionPanel;
    public Button OptionExitButton;

/*    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider effectSlider;*/

    public Toggle fullScreenToggle;
    public Toggle windowScreenToggle;

    void Start()
    {
        OptionPanel.SetActive(false);
        startButton.GetComponent<Button>().onClick.AddListener(ONStartButtonClick);
        optionButton.GetComponent<Button>().onClick.AddListener(OnOptionButtonClick);
        endButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClick);
        OptionExitButton.GetComponent<Button>().onClick.AddListener(OnOptionExitButtonClick);

       /* bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 1.0f);
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1.0f);*/
        fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        windowScreenToggle.isOn =  !fullScreenToggle.isOn;


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
    void OnExitButtonClick()
    {
        Application.Quit();
    }
    void OnOptionExitButtonClick()
    {
        OptionPanel.SetActive(false);
    }

/*    public void SetBgmBolime(float volume)
    {
        audioMixer.SetFloat("BgmVolume", volume); // 오디오 믹서 그룹 이름
    }
    public void SetEffectBolime(float volume)
    {
        audioMixer.SetFloat("EffectVolume", volume);
    }*/
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0);
        windowScreenToggle.isOn = !isFullScreen;
    }
    public void SetWindowScreen(bool isOn)
    {
        fullScreenToggle.isOn=!isOn;
        SetFullScreen(!isOn);
    }
}
