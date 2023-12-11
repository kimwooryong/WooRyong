using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerPrefab;
    public Transform spawnPlace;
    private PlayerStatus player;

    public bool isCaveEnter = false;

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
        player = FindObjectOfType<PlayerStatus>();
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
        player.rb.isKinematic = false;
        player.gameObject.SetActive(true);
        player.playerCurrentHp = 100;
        player.theCurrentStateOfHunger = 100;
        player.transform.position = spawnPlace.position;
        player.transform.rotation = spawnPlace.rotation;
        //Instantiate(playerPrefab, spawnPlace.position, spawnPlace.rotation);
        //ItemManager.Instance.ResetWhenPlayerDie();
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
}
