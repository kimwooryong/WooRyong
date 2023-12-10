using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGps : MonoBehaviour
{
    public float yOffSet;
    private PlayerStatus playerMovement;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement != null)
        {
            gameObject.transform.position = new Vector3(playerMovement.transform.position.x, yOffSet, playerMovement.transform.position.z);
        }
        else if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerStatus>();
        }
    
    }
}
