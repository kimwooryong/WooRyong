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
        Cursor.lockState = CursorLockMode.None; // ���콺 ���� ����
        Cursor.visible = true; // ���콺 Ŭ������
    }
    public void InvisibleCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ����
        Cursor.visible = false; // ���콺 Ŭ���Ұ�
    }
    public void InvisibleAndNoneCursor()
    {
        Cursor.lockState = CursorLockMode.None; // ���콺 ���� ����
        Cursor.visible = false; // ���콺 Ŭ���Ұ�
    }
    void OnSceneLoad( UnityEngine.SceneManagement.Scene scene, LoadSceneMode load)
    {

    }

}
