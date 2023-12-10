using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public float fadeOutDuration = 1.5f;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume = 2f;
    [SerializeField]
    public AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume = 3f;
    AudioSource sfxPlayer;

    [Header("#UI")]
    public AudioClip[] uiClips;
    public float uiVolume = 3f;
    AudioSource uiPlayer;

    //플레이어의 움직임, 점프 관련
    [Header("#Player")]
    public AudioClip[] playerClips;
    public float playerVolume = 3f;
    AudioSource playerPlayer;

    [SerializeField]
    public string selectedClipName = "None";


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
        Init();
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
        sfxObject.transform.parent = transform;
        sfxPlayer = sfxObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.volume = sfxVolume;


        //UI 효과음 플레이어 초기화
        GameObject uiObject = new GameObject("UIPlayer");
        uiObject.transform.parent = transform;
        uiPlayer = uiObject.AddComponent<AudioSource>();
        uiPlayer.playOnAwake = false;
        uiPlayer.volume = uiVolume;

        //Player 소리 플레이어 초기화
        GameObject playerObject = new GameObject("PlayerPlayer");
        playerObject.transform.parent = transform;
        playerPlayer = playerObject.AddComponent<AudioSource>();
        playerPlayer.playOnAwake = false;
        playerPlayer.volume = playerVolume;

    }
    public void PlaySound(AudioSource player, AudioClip[] AudioClips, string clipName)
    {
        AudioClip selectedClip = null;

        foreach(AudioClip clip in AudioClips)
        {
            if (clip.name.Contains(clipName))
            {
                selectedClip = clip;
                selectedClipName = clip.name;
                break;
            }
        }

        if (selectedClip != null)
        {
            player.PlayOneShot(selectedClip);
        } else
        {
            Debug.Log("\"clipName\" sound is Empty.");
        }
    }

    //public void FadeOutAndStopSound()
    //{
    //    StartCoroutine(FadeOutCoroutine());
    //}

    //IEnumerator FadeOutCoroutine()
    //{
    //    float bgmVolume = bgmPlayer.volume;
    //    while(bgmPlayer.volume > 0)
    //    {
    //        bgmPlayer.volume -= bgmVolume * Time.deltaTime / fadeOutDuration;
    //        yield return null;
    //    }

    //    // 사운드 중지 후 
    //    bgmPlayer.Stop();
    //    // 기존 볼륨대로 초기화
    //    bgmPlayer.volume = bgmVolume;
    //}



    //Sound Test
    public void Start()
    {
        // 박쥐  
        //PlaySound(sfxPlayer, sfxClips, "Bat");
        //PlayBatNoiseWaitForSeconds();

        // 동굴 
        //PlaySound(bgmPlayer, bgmClips, "Cave");
        //StartCoroutine(PlayBatNoiseWaitForSeconds());
        //PlayBgmForestNight();
    }


    //BGM
    // 해변가
    public void PlayBgmBeach()
    {
        PlaySound(bgmPlayer, bgmClips, "Beach");

    }

    // 풀숲
    public void PlayBgmForest()
    {
        PlaySound(bgmPlayer, bgmClips, "Forest");
    }

    // 빗소리
    public void PlayBgmRain()
    {
        PlaySound(bgmPlayer, bgmClips, "Rain");
    }

    // 동굴안 
    public void PlayBgmInCave()
    {
        PlaySound(bgmPlayer, bgmClips, "Cave");
    }


    //SFX
    //동물 우는 소리
    public void PlayGrowlsAnimal()
    {
        PlaySound(sfxPlayer, sfxClips, "GrowlsAnimal");
    }

    // 동물이 걷는 소리
    //public void PlayAnimalWalking()
    //{
    //    PlaySound(sfxPlayer, sfxClips, "animalWalking"); // x
    //}

    // 불 지피는 소리 
    public void PlayCampfire()
    {
        PlaySound(sfxPlayer, sfxClips, "Campfire");  
    }

    // 갈매기
    public void PlaySeagullsSound()
    {
        PlaySound(sfxPlayer, sfxClips, "Seagulls");  
    }

    // 박쥐
    public void PlayBatNoise()
    {
        PlaySound(sfxPlayer, sfxClips, "Bat");
    }

    // 박쥐 : 2초 후 플레이 
    public IEnumerator PlayBatNoiseWaitForSeconds()
    {
        yield return new WaitForSeconds(2f);
        PlayBatNoise();
    }

    // 여우
    public void PlayFox()
    {
        PlaySound(sfxPlayer, sfxClips, "Fox");
    }


    // 멧돼지 쓰러지는 소리
    public void PlayBoarDie()
    {
        PlaySound(sfxPlayer, sfxClips, "BoarDie");
    }

    // 나무문 열리는 소리
    public void PlayOpenDoor()
    {
        PlaySound(sfxPlayer, sfxClips, "OpenDoor");
        //삐걱 , 끼익 
    }



    //UI
    // 건축물 설치/취소
    public void PlayOnOffBuilding()
    {
        PlaySound(uiPlayer, uiClips, "Building");   
    }

    // 건축창 On/Off
    public void PlayOnOffConstructionInventory()
    {
        PlaySound(uiPlayer, uiClips, "ConstructionInventory");  
    }

    // 아이템창 On/Off
    public void PlayOnOffItemInventory()
    {
        PlaySound(uiPlayer, uiClips, "ItemInventory");  
    }

    // 아이템 습득
    public void PlayGetItem()
    {
        PlaySound(uiPlayer, uiClips, "GetItem");  
    }

    // 아이템 드랍
    public void PlayDropItem()
    {
        PlaySound(uiPlayer, uiClips, "DropItem"); 
    }

    // 아이템 장착
    public void PlayEuipItem()
    {
        PlaySound(uiPlayer, uiClips, "EquipItem");  
    }

    // 인벤토리 내 - 아이템 위치 변경
    public void PlayArrangeItem()
    {
        if (!uiPlayer.isPlaying)
        {
        PlaySound(uiPlayer, uiClips, "ArrangeItem");
        }
    }



    //Player
    // 걷기
    public void PlayPlayerFootStep()
    {
        PlaySound(playerPlayer, playerClips, "PlayerFootStep");  
    }

    // 달리기
    public void PlayPlayerRun()
    {
        PlaySound(playerPlayer, playerClips, "PlayerRun");  
    }

    // 점프
    //public void PlayPlayerJump()
    //{
    //    PlaySound(playerPlayer, playerClips, "PlayerJump"); // x
    //}

    // 음식 먹기
    public void PlayPlayerEatFood()
    {
        PlaySound(playerPlayer, playerClips, "EatFood");  
    }

    // 무기로 나무를 가격
    public void PlayPlayerAttackTree()
    {
        PlaySound(playerPlayer, playerClips, "AttackTree");  
    }

    // 무기로 바위를 가격
    public void PlayPlayerAttackRock()
    {
        PlaySound(playerPlayer, playerClips, "AttackRock");   
    }

    // 공격 미스
    public void PlayPlayerAttackMiss()
    {
        PlaySound(playerPlayer, playerClips, "AttackMiss");
    }


    // 건축물 부수기
    public void PlayPlayerDestroyBuilding()
    {
        PlaySound(playerPlayer, playerClips, "EestroyBuilding");  
    }

}

