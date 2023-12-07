using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRide : MonoBehaviour
{
    private PlayerMovement player;

    public Transform boatSeat;
    public bool isRiding = false;
    private Animator ani;
    Transform left;
    Animator leftChild;
    Transform right;
    Animator rightChild;



    void Start()
    {
        left = gameObject.transform.Find("Left");
        leftChild = left.GetComponent<Animator>();

        right = gameObject.transform.Find("Right");
        rightChild = right.GetComponent<Animator>();

        player = FindObjectOfType<PlayerMovement>();    
    }
    
    void Update()
    {
        if(isRiding)
        {
            leftChild.SetTrigger("isRide");
            rightChild.SetTrigger("isRide");
        }
    }
}
