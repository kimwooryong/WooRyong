using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerPrefab;
    public Transform spawnPlace;
    private int currentSpawnIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPlace.position, spawnPlace.rotation);
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPlace = newSpawnPoint;
    }
    //Cursor
    public void VisibleCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 마우스 고정 해제
        Cursor.visible = true; // 마우스 클릭가능
    }
    public void InvisibleCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        Cursor.visible = false; // 마우스 클릭불가
    }
    public void InvisibleAndNoneCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 마우스 고정 해제
        Cursor.visible = false; // 마우스 클릭불가
    }
    void OnSceneLoad( UnityEngine.SceneManagement.Scene scene, LoadSceneMode load)
    {

    }

}
