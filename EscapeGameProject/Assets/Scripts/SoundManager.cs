using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    AudioSource sfxPlayer;

    [Header("#UI")]
    public AudioClip[] uiClips;
    public float uiVolume;
    AudioSource uiPlayer;

    //플레이어의 움직임, 점프 관련
    [Header("#Player")]
    public AudioClip[] playerClips;
    public float playerVolume;
    AudioSource playerPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        sfxPlayer = go.AddComponent<AudioSource>();
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }



    public void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxPlayer.transform.parent = transform;
        sfxPlayer = sfxObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        //sfxPlayer.loop = false;
        sfxPlayer.volume = sfxVolume;

        //sfxPlayers = new AudioSource[channels]

        //UI 효과음 플레이어 초기화
        GameObject uiObject = new GameObject("UIPlayer");
        uiPlayer.transform.parent = transform;
        uiPlayer = uiObject.AddComponent<AudioSource>();
        uiPlayer.playOnAwake = false;
        uiPlayer.volume = uiVolume;

        //Player 소리 플레이어 초기화
        GameObject playerObject = new GameObject("PlayerPlayer");
        playerPlayer.transform.parent = transform;
        playerPlayer = playerObject.AddComponent<AudioSource>();
        playerPlayer.playOnAwake = false;
        playerPlayer.volume = playerVolume;

    }

    //BGM
    public void PlayMoveSound()
    {

    }

    //SFX


    //UI


    //Player
    public void PlaySoundPlayerMove()
    {
    }
    public void PlaySoundPlayerRun()
    {

    }
    public void PlaySoundPlayerJump()
    {

    }

}
