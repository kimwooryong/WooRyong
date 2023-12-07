using UnityEngine;

public class Rock : MonoBehaviour
{
    private PlayerMovement player;

    public GameObject[] dropItem;
    [SerializeField]
    private int minDropCount = 5;
    [SerializeField]
    private int maxDropCount = 6;

    public int currentHp = 5;
    public int maxHp = 5;

    private int dropCount;
    private float dropLocation;

    private bool isDrop = false;

    private float animalDieTime = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();

        dropCount = Random.Range(minDropCount, maxDropCount);



    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(player.playerDamage);
        }

        if (currentHp <= 0 && !isDrop)
        {
            animalDieTime += Time.deltaTime;

            if (animalDieTime >= 1.0f)
            {
                for (int i = 0; i < dropCount; i++)
                {
                    dropLocation = Random.Range(-0.3f, 0.3f);
                    DropItem();

                }
                isDrop = true;
                Destroy(gameObject);
            }

        }
    }

    void DropItem()
    {
        if (dropItem.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItem.Length);
            GameObject choiceItem = dropItem[randomIndex];
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;
            Instantiate(choiceItem, spawnPosition, Quaternion.identity);
        }
    }
    void TakeDamage(int damage)
    {
        currentHp -= damage;
    }

}
