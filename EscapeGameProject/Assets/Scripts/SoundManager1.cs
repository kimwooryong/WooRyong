using System.Collections;
using UnityEngine;

public class SoundManager1 : MonoBehaviour
{
    public static SoundManager1 Instance;
    public float fadeOutDuration = 1.5f;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume = 0.5f;
    [SerializeField]
    public AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume = 0.7f;
    AudioSource sfxPlayer;

    [Header("#UI")]
    public AudioClip[] uiClips;
    public float uiVolume = 0.7f;
    AudioSource uiPlayer;

    //플레이어의 움직임, 점프 관련
    [Header("#Player")]
    public AudioClip[] playerClips;
    public float playerVolume = 0.7f;
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

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = sfxObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.volume = sfxVolume;

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


    public void PlayDeerHit()
    {
        PlaySound(sfxPlayer, sfxClips, "DeerHit");
    }

    public void PlayDeerDie()
    {
        PlaySound(sfxPlayer, sfxClips, "DeerDie");
    }
    public void PlayWolfHowl()
    {
        PlaySound(sfxPlayer, sfxClips, "WolfHowl");
    }
    public void PlayWolfDie()
    {
        PlaySound(sfxPlayer, sfxClips, "WolfDie");
    }
    public void PlayWolfAttack()
    {
        PlaySound(sfxPlayer, sfxClips, "WolfAttack");
    }
    public void PlayWolfHit()
    {
        PlaySound(sfxPlayer, sfxClips, "WolfHit");
    }
    public void PlayBoarAttack()
    {
        PlaySound(sfxPlayer, sfxClips, "BoarAttack");
    }
    public void PlayBoarHit()
    {
        PlaySound(sfxPlayer, sfxClips, "BoarHit");
    }
    public void PlayBearAttack()
    {
        PlaySound(sfxPlayer, sfxClips, "BearAttack");
    }
    public void PlayBearHit()
    {
        PlaySound(sfxPlayer, sfxClips, "BearHit");
    }
    public void PlayBearDie()
    {
        PlaySound(sfxPlayer, sfxClips, "BearDie");
    }



}

