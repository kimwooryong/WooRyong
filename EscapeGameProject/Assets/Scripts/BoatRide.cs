using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRide : MonoBehaviour
{
    private PlayerMovement player;

    public Transform boatSeat;
    private bool isRiding = false;

    void Start()
    {
    player = FindObjectOfType<PlayerMovement>();    
    }
    
    void Update()
    {
        
    }
}
