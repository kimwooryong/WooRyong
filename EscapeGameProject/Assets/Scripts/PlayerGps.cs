using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGps : MonoBehaviour
{
    public float yOffSet;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement != null)
        {
            gameObject.transform.position = new Vector3(playerMovement.transform.position.x, yOffSet, playerMovement.transform.position.z);
        }
    }
}
