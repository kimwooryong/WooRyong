using UnityEngine;
public enum TreeType
{
    Apple,
    Palm,
    Bush
}
public class Tree : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    public GameObject[] dropItem;
    private float dropLocation;
    private PlayerStatus player;

    private int dropCount;

    private float treeDestroyTime;

    private bool isDrop = false;

    private float danglingItemY;
    private float danglingItemX;
    private float danglingItemZ;

    public TreeType treeType;



    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        currentHp = maxHp;
        dropCount = Random.Range(3, 4);

        if (dropItem.Length >= 2)
        {
            for (int i = 0; i < dropItem.Length -2 ; i++)
            {
                if (treeType == TreeType.Apple)
                {
                    float minRange = -1.0f;
                    float maxRange = 1.0f;
                    float excludedMin = -0.15f;
                    float excludedMax = 0.15f;
                    

                    do
                    {
                        danglingItemX = Random.Range(minRange, maxRange);
                        
                    } while (danglingItemX >= excludedMin && danglingItemX <= excludedMax);
                    do
                    {
                        danglingItemZ = Random.Range(minRange, maxRange);

                    } while (danglingItemZ >= excludedMin && danglingItemZ <= excludedMax);

                }
                else if (treeType == TreeType.Palm)
                {
                    float minRange = -0.2f;
                    float maxRange = 0.2f;
                    float excludedMin = -0.15f;
                    float excludedMax = 0.15f;

                    do
                    {
                        danglingItemX = Random.Range(minRange, maxRange);
                    } while (danglingItemX >= excludedMin && danglingItemX <= excludedMax);
                    do
                    {
                        danglingItemZ = Random.Range(minRange, maxRange);

                    } while (danglingItemZ >= excludedMin && danglingItemZ <= excludedMax);
                }
                else if (treeType == TreeType.Bush)
                {
                    float fixedCoordinate = 0.5f;
                    float randomCoordinate;


                    if (Random.Range(0, 2) == 0)
                    {
                        danglingItemX = fixedCoordinate;
                        float minRangeZ = -0.5f;
                        float maxRangeZ = 0.5f;
                        float excludedMinZ = -0.01f;
                        float excludedMaxZ = 0.01f;

                        do
                        {
                            danglingItemZ = Random.Range(minRangeZ, maxRangeZ);
                        } while (danglingItemZ >= excludedMinZ && danglingItemZ <= excludedMaxZ);
                    }
                    else
                    {
                        danglingItemZ = fixedCoordinate;
                        float minRangeX = -0.5f;
                        float maxRangeX = 0.5f;
                        float excludedMinX = -0.01f;
                        float excludedMaxX = 0.01f;

                        do
                        {
                            danglingItemX = Random.Range(minRangeX, maxRangeX);
                        } while (danglingItemX >= excludedMinX && danglingItemX <= excludedMaxX);
                    }
                }

                GameObject newDrop = DropAddItem(false);
                if (newDrop != null) // 체크 추가
                {
                    newDrop.transform.parent = transform;

                }
            }
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(player.playerDamage);
        }

        if (currentHp <= 0 && !isDrop)
        {

            treeDestroyTime += Time.deltaTime;
            if (treeDestroyTime >= 0.95f)
            {
                GravityCheck();
            }
            if (treeDestroyTime >= 1)
            {
                for (int i = 0; i < dropCount; i++)
                {

                    dropLocation = Random.Range(-0.3f, 0.3f);

                    if (dropItem[0] != null)
                    {
                        DropBasicItem();
                    }
                    

                    if (dropItem[1] != null)
                    {
                        DropBasicItem2();
                    }
                }

                Destroy(gameObject);
                isDrop = true;
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHp -= damage;
    }

    GameObject DropBasicItem()
    {
        if (dropItem.Length >= 2)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;

            GameObject drop1 = Instantiate(dropItem[0], spawnPosition, Quaternion.identity);

            return drop1;
        }

        return null;
    }
    GameObject DropBasicItem2()
    {
        if (dropItem.Length >= 2)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;

            GameObject drop2 = Instantiate(dropItem[1], spawnPosition, Quaternion.identity);

            return drop2;
        }

        return null;
    }

    GameObject DropAddItem(bool applyGravity)
    {
        if (dropItem.Length > 2)
        {
            if (treeType == TreeType.Apple)
            {
                danglingItemY = Random.Range(4.7f, 4.8f);
            }
            else if (treeType == TreeType.Palm)
            {
                danglingItemY = Random.Range(3.5f, 3.6f);
            }
            else if(treeType == TreeType.Bush)
            {
                danglingItemY = Random.Range(0.2f, 0.4f);
            }
            Vector3 spawnPosition = transform.position + new Vector3(danglingItemX, danglingItemY, danglingItemZ);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;

            float randomValue = Random.value * 100f;

            float cumulativeProbability = 0f;
            GameObject chosenItem = null;

            for (int i = dropItem.Length - 1; i >= 2; i--)
            {
                float itemProbability = 0f;
                if (treeType == TreeType.Apple || treeType == TreeType.Palm)
                {
                    switch (i)
                    {
                        case 4:
                            itemProbability = 2f;
                            break;
                        case 3:
                            itemProbability = 5f;
                            break;
                        case 2:
                            itemProbability = 8f;
                            break;
                        default:
                            itemProbability = 85f;
                            break;
                    }
                }
                else if (treeType == TreeType.Bush)
                {
                    switch (i)
                    {
                        case 4:
                            itemProbability = 50f;
                            break;
                        case 3:
                            itemProbability = 30f;
                            break;
                        case 2:
                            itemProbability = 20f;
                            break;
                        default:
                            itemProbability = 0f;
                            break;
                    }
                }
                

                cumulativeProbability += itemProbability;

                if (randomValue <= cumulativeProbability)
                {
                    chosenItem = dropItem[i];
                    break;
                }
            }

            if (chosenItem != null)
            {
                if (treeType == TreeType.Apple || treeType == TreeType.Bush)
                {
                    Quaternion appleTreeRotation = Quaternion.Euler(-90f, 0, 0);
                GameObject newItem = Instantiate(chosenItem, spawnPosition, appleTreeRotation);
                Rigidbody itemRigidbody = newItem.GetComponent<Rigidbody>();

                if (itemRigidbody != null)
                {
                    itemRigidbody.useGravity = applyGravity;
                    itemRigidbody.isKinematic = true;
                }

                if (currentHp <= 0)
                {
                    itemRigidbody.useGravity = true;
                    itemRigidbody.isKinematic = false;
                }

                return newItem;
                }

                if (treeType == TreeType.Palm)
                {

                    GameObject newItem = Instantiate(chosenItem, spawnPosition, Quaternion.identity);
                    Rigidbody itemRigidbody = newItem.GetComponent<Rigidbody>();

                    if (itemRigidbody != null)
                    {
                        itemRigidbody.useGravity = applyGravity;
                        itemRigidbody.isKinematic = true;
                    }

                    if (currentHp <= 0)
                    {
                        itemRigidbody.useGravity = true;
                        itemRigidbody.isKinematic = false;
                    }

                    return newItem;
                }
            }
        }

        return null;
    }

    private void GravityCheck()
    {
        foreach (Transform child in transform)
        {
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;

            child.parent = null;
        }
    }
}
