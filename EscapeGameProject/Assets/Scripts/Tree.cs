using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
public enum TreeType
{
    Apple,
    Palm
}
public class Tree : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    public GameObject[] dropItem;
    private float dropLocation;
    private PlayerMovement player;

    private int dropCount;

    private float treeDestroyTime;

    private bool isDrop = false;

    private float danglingItemY;
    private float danglingItemXZ;

    public TreeType treeType;



    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        currentHp = maxHp;
        dropCount = Random.Range(3, 4);

        if (dropItem.Length >= 2)
        {
            for (int i = 0; i < dropItem.Length - 2; i++)
            {
                if (treeType == TreeType.Apple)
                {
                    float minRange = -1.0f;
                    float maxRange = 1.0f;
                    float excludedMin = -0.15f;
                    float excludedMax = 0.15f;

                    do
                    {
                        danglingItemXZ = Random.Range(minRange, maxRange);
                    } while (danglingItemXZ >= excludedMin && danglingItemXZ <= excludedMax);

                }
                else if (treeType == TreeType.Palm)
                {
                    float minRange = -0.2f;
                    float maxRange = 0.2f;
                    float excludedMin = -0.15f;
                    float excludedMax = 0.15f;

                    do
                    {
                        danglingItemXZ = Random.Range(minRange, maxRange);
                    } while (danglingItemXZ >= excludedMin && danglingItemXZ <= excludedMax);
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

                    DropBasicItem();
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
            Vector3 spawnPosition = transform.position + new Vector3(danglingItemXZ, danglingItemY, danglingItemXZ);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;

            float randomValue = Random.value * 100f;

            float cumulativeProbability = 0f;
            GameObject chosenItem = null;

            for (int i = dropItem.Length - 1; i >= 2; i--)
            {
                float itemProbability = 0f;

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

                cumulativeProbability += itemProbability;

                if (randomValue <= cumulativeProbability)
                {
                    chosenItem = dropItem[i];
                    break;
                }
            }

            if (chosenItem != null)
            {
                if (treeType == TreeType.Apple)
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
