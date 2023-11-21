using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;
    public Transform spawnPlace;

    private void Update()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Instantiate(playerPrefab, spawnPlace.position, spawnPlace.rotation);
        }

    }

}
