
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalItemDrop : MonoBehaviour
{


    private PlayerMovement player;
    private AnimalMovement animal;

    public GameObject[] dropItem;
    [SerializeField]
    private int minDropCount;
    [SerializeField]
    private int maxDropCount;

    private int dropCount;
    private float dropLocation;

    private bool isDrop = false;

    private float animalDieTime = 0;

    private void Start()
    {
        animal = GetComponent<AnimalMovement>();
        player = FindObjectOfType<PlayerMovement>();

        dropCount = Random.Range(minDropCount, maxDropCount);



    }
    private void Update()
    {


        if (animal.currentHp <= 0 && !isDrop)
        {
            Debug.Log("파 다달았음");
            animalDieTime += Time.deltaTime;

            if (animalDieTime >= animal.dieTime)
            {
                for (int i = 0; i < dropCount; i++)
                {
                    dropLocation = Random.Range(-0.3f, 0.3f);
                    Debug.Log("생성");
                    DropItem();
                    
                }
                isDrop = true;
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
}
