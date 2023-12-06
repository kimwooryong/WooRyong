using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
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
        //bgmPlayer.clips = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxPlayer.transform.parent = transform;
        sfxPlayer = sfxObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.volume = sfxVolume;


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
    // 해변가
    public void PlayBgmBeach()
    {
        // Bgm - Beach
    }

    // 풀숲 - 밤
    public void PlayBgmForestNight()
    {
        // Bgm - ForestNight
    }

    // 빗소리
    public void PlayBgmRain()
    {
        // Bgm - Rain
    }


    //SFX
    //동물 우는 소리
    public void PlayAnimalNoise()
    {

    }

    // 동물이 걷는 소리
    public void PlayAnimalWalking()
    {

    }

    // 불 지피는 소리 
    public void PlayCampfire()
    {
        // Sfx - FireBurning
    }

    // 갈매기 소리
    public void PlaySeagullsSound()
    {
        // Sfx - Seagulls
    }


    //UI
    // 건축물 설치/취소
    public void PlayOnOffBuilding()
    {

    }

    // 건축창 On/Off
    public void PlayOnOffConstructionInventory()
    {

    }


    // 인벤토리 On/Off
    public void PlayOnOffItemInventory()
    {

    }

    // 아이템 습득
    public void PlayGetItem()
    {

    }

    // 아이템 드랍
    public void PlayDropItem()
    {

    }

    // 아이템 장착
    public void PlaySetItem()
    {
        // Sfx-SetItem
    }

    // 인벤토리 내 - 아이템 위치 변경
    public void PlayArrangeItem()
    {

    }

    //Player
    // 걷기
    public void PlayPlayerMove()
    {
        // Sfx - FootstepsGrass
    }

    // 달리기
    public void PlayPlayerRun()
    {
        // Player - footstepsRunning
    }

    // 점프
    public void PlayPlayerJump()
    {

    }

    // 음식 먹기
    public void PlayPlayerEatFood()
    {
        // Player-EatingFood
        // Player-EatingChips
    }

    // 무기로 나무를 가격
    public void PlayPlayerAttackTree()
    {

    }

    // 무기로 바위를 가격
    public void PlayPlayerAttackRock()
    {
        // Sfx - HeaviStonesFall
        // Sfx - StonesFall5
    }

    // 건축물 부수기
    public void PlayPlayerDestroyBuilding()
    {

    }

}
