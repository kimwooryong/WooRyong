using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRide : MonoBehaviour
{
    private PlayerMovement player;

    public Transform boatSeat;
    public bool isRiding = false;
    private Animator ani;
    

    void Start()
    {
    player = FindObjectOfType<PlayerMovement>();    
    }
    
    void Update()
    {
        if(isRiding)
        {
            ani.SetTrigger("isRide");
        }
    }
}
