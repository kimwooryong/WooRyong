using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tree : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    public GameObject[] treeObject;
    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); 
        currentHp = maxHp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

            TakeDamage(player.playerDamage);
            if(currentHp <= 0)
            {
                DropItem();
                Destroy(gameObject);
            }
        }
    }
    void TakeDamage(int damage)
    {
        currentHp -= damage;
    }
    void DropItem()
    {
        if(treeObject!= null) 
        {
        foreach (GameObject item in treeObject)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        }
    }

}
