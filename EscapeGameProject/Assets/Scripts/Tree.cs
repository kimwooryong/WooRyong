using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        currentHp = maxHp;
        dropCount = Random.Range(3, 4);

        if (dropItem.Length >= 2)
        {
            for (int i = 0; i < dropItem.Length - 2; i++)
            {
                danglingItemXZ = Random.Range(-2.0f, 2.0f);

                
                GameObject newDrop = DropAddItem(false);
                newDrop.transform.parent = transform;
                foreach (Rigidbody childRigidbody in GetComponentsInChildren<Rigidbody>())
                {
                    childRigidbody.detectCollisions = false;
                }
            }
        }
    }

    private void Update()
    {
        dropLocation = Random.Range(-0.3f, 0.3f);

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(player.playerDamage);
        }

        if (currentHp <= 0 && !isDrop)
        {
            
            treeDestroyTime += Time.deltaTime;
            if(treeDestroyTime >= 0.99f)
            {
                GrivtyCheck();
                foreach (Rigidbody childRigidbody in GetComponentsInChildren<Rigidbody>())
                {
                    childRigidbody.detectCollisions = true;
                }
            }
            if (treeDestroyTime >= 1)
            {
                for (int i = 0; i < dropCount; i++)
                {
                    dropLocation = Random.Range(-0.3f, 0.3f);
                    DropBasicItem();
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
            GameObject drop2 = Instantiate(dropItem[1], spawnPosition, Quaternion.identity);

            return drop1;
        }

        return null;
    }

    GameObject DropAddItem(bool applyGravity)
    {
        if (dropItem.Length > 2)
        {
            danglingItemY = Random.Range(4.6f, 4.8f);
            Vector3 spawnPosition = transform.position + new Vector3(danglingItemXZ, danglingItemY, danglingItemXZ);
            spawnPosition.x += dropLocation;
            spawnPosition.z += dropLocation;
            for (int i = 2; i < dropItem.Length; i++)
            {
                GameObject choiceItem = dropItem[i];
                if (choiceItem != null)
                {
                    GameObject newItem = Instantiate(choiceItem, spawnPosition, Quaternion.identity);

                    Rigidbody itemRigidbody = newItem.GetComponent<Rigidbody>();
                    if (itemRigidbody != null)
                    {
                        itemRigidbody.useGravity = applyGravity;
                    }

                    if (currentHp <= 0)
                    {
                        itemRigidbody.useGravity = true;
                    }

                    return newItem;
                }
            }



        }

        return null;
    }

    private void GrivtyCheck()
    {
        foreach (Transform child in transform)
        {
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();

            if (childRigidbody != null)
            {
                childRigidbody.useGravity = true;
            }

            child.parent = null;
        }
    }
}
